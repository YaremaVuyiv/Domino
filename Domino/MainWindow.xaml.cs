using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Domino
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<DominoModel> _allDominos;

        private MyDominosCollection _myDominosCollection;
        private TableDominoCollection _tableDominoCollection;
        private OpponentDominoCollection _opponentDominoCollection;
        private BankCollection _bankCollection;
        public MainWindow()
        {
            
            InitializeComponent();
            _allDominos = GetAllDominos();
            StartFirstTurn();
        }

        private void StartFirstTurn()
        {
            var opponentsDominos = TakeSevenRandomDominos();
            var myDominos = TakeSevenRandomDominos();

            var unionDominos = myDominos.Union(opponentsDominos);
            var startDomino = unionDominos
                .Where(d => d.First == d.Second)
                .DefaultIfEmpty(unionDominos.Max())
                .Min();

            _tableDominoCollection = new TableDominoCollection(new List<DominoModel> { startDomino });
            _tableDominoCollection.Dominos.CollectionChanged += Dominos_CollectionChanged;
            TableListView.ItemsSource = _tableDominoCollection.DominosLables;

            _opponentDominoCollection = new OpponentDominoCollection(opponentsDominos, _tableDominoCollection);
            _opponentDominoCollection.Dominos.CollectionChanged += Dominos_CollectionChanged;
            OpponentHandListView.ItemsSource = _opponentDominoCollection.DominosLables;

            _myDominosCollection = new MyDominosCollection(myDominos, _tableDominoCollection);
            _myDominosCollection.Dominos.CollectionChanged += Dominos_CollectionChanged;
            MyHandListView.ItemsSource = _myDominosCollection.DominosLables;

            _bankCollection = new BankCollection(_allDominos);
            BankListView.ItemsSource = _bankCollection.DominosLables;

            if (myDominos.Contains(startDomino))
            {
                _myDominosCollection.Dominos.Remove(startDomino);
            }
            else
            {
                _opponentDominoCollection.Dominos.Remove(startDomino);
            }
        }

        private void Dominos_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            TableListView.Items.Refresh();
            MyHandListView.Items.Refresh();
            OpponentHandListView.Items.Refresh();
            BankListView.Items.Refresh();
        }

        private List<DominoModel> GetAllDominos()
        {
            var result = new List<DominoModel>();

            for (byte i = 0; i < 7; i++)
            {
                for (byte j = i; j < 7; j++)
                {
                    result.Add(new DominoModel(i, j));
                }
            }

            return result;
        }

        private List<DominoModel> TakeSevenRandomDominos()
        {
            var result = new List<DominoModel>();

            for (int i = 0; i < 7; i++)
            {
                result.Add(GetRandomDomino());
            }

            return result;
        }

        private DominoModel GetRandomDomino()
        {
            var rnd = new Random();
            var index = rnd.Next(0, _allDominos.Count() - 1);
            var result = _allDominos[index];
            _allDominos.RemoveAt(index);
            return result;
        }

        private void TakeButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_myDominosCollection.HasDominosToPut() && _allDominos.Count != 0)
            {
                var domino = GetRandomDomino();
                _bankCollection.Dominos.Remove(domino);
                _myDominosCollection.Dominos.Add(domino);
            }
        }
    }
}
