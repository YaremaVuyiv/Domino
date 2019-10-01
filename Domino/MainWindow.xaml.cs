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
        private Label _selectedDomino;

        private List<DominoModel> _allDominos;

        private HandBaseCollection _myDominosCollection;
        private TableDominoCollection _tableDominoCollection;
        private HandBaseCollection _opponentDominoCollection;
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
            TableListView.ItemsSource = _tableDominoCollection.Dominos;

            _opponentDominoCollection = new HandBaseCollection(opponentsDominos, _tableDominoCollection);
            OpponentHandListView.ItemsSource = _opponentDominoCollection.Dominos;

            _myDominosCollection = new HandBaseCollection(myDominos, _tableDominoCollection);
            MyHandListView.ItemsSource = _myDominosCollection.Dominos;

            BankListView.ItemsSource = _allDominos;

            if (myDominos.Contains(startDomino))
            {
                _myDominosCollection.Dominos.Remove(startDomino);
            }
            else
            {
                _opponentDominoCollection.Dominos.Remove(startDomino);
            }
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
                result.Add(GetRandomDomino().Value);
            }

            return result;
        }

        private DominoModel? GetRandomDomino()
        {
            if(_allDominos.Count == 0)
            {
                return null;
            }

            var rnd = new Random();
            var index = rnd.Next(0, _allDominos.Count - 1);
            var result = _allDominos[index];
            _allDominos.RemoveAt(index);
            return result;
        }

        private void TakeButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_myDominosCollection.HasDominoToPlace())
            {
                var bankDomino = GetRandomDomino();
                if (bankDomino.HasValue)
                {
                    _myDominosCollection.AddNewDomino(bankDomino.Value);
                    RefreshAllItemSources();
                }
            }
        }

        private void MyHandLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var label = sender as Label;
            DeselectSelectedDomino();

            _selectedDomino = label;

            if (_tableDominoCollection.IsDominoOkForLeft(new DominoModel(label.Content.ToString())))
            {
                TableLeftDomino.Visibility = Visibility.Visible;
            }

            if (_tableDominoCollection.IsDominoOkForRight(new DominoModel(label.Content.ToString())))
            {
                TableRightDomino.Visibility = Visibility.Visible;
            }
            label.Background = Brushes.Green;
        }

        private void TableLeftDomino_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DeselectSelectedDomino();
            var domino = new DominoModel(_selectedDomino.Content.ToString());
            _tableDominoCollection.AddToLeft(domino);
            RefreshAllItemSources();
        }

        private void TableRightDomino_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DeselectSelectedDomino();
            var domino = new DominoModel(_selectedDomino.Content.ToString());
            _tableDominoCollection.AddToRight(domino);
            RefreshAllItemSources();
        }

        private void DeselectSelectedDomino()
        {
            if (_selectedDomino != null)
            {
                _selectedDomino.Background = Brushes.Turquoise;
                TableLeftDomino.Visibility = Visibility.Collapsed;
                TableRightDomino.Visibility = Visibility.Collapsed;
            }
        }

        private void RefreshAllItemSources()
        {
            TableListView.Items.Refresh();
            MyHandListView.Items.Refresh();
            OpponentHandListView.Items.Refresh();
            BankListView.Items.Refresh();
        }
    }
}
