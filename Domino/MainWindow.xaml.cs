using Domino.Collections;
using Domino.Models;
using Domino.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Domino
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Image _selectedDomino;

        private readonly LogicService _logicService;
        private readonly StartGameService _startGameService;
        private readonly AIService _aIService;

        public MainWindow()
        {
            InitializeComponent();
            _startGameService = new StartGameService();
            _logicService = new LogicService(_startGameService.TableDominoCollection,
                _startGameService.MyDominosCollection, _startGameService.BankService);

            _startGameService.MyDominosCollection.Dominos.ForEach(d =>
            {
                d.ImageResource = $"Images/_{d.First}v{d.Second}_.png";
            });
            MyHandListView.ItemsSource = _startGameService.MyDominosCollection.Dominos;
            var startDomino = _startGameService.TableDominoCollection.Dominos.First();
            ////////////////////
            TopTableListView.ItemsSource = _startGameService.TableDominoCollection.TopDominos;
            LeftTableListView.ItemsSource = _startGameService.TableDominoCollection.LeftDominos;
            RightTableListView.ItemsSource = _startGameService.TableDominoCollection.RightDominos;
            BotTableListView.ItemsSource = _startGameService.TableDominoCollection.BottomDominos;
            ////////////////////
            OpponentHandListView.ItemsSource = _startGameService.OpponentDominosCollection.Dominos;
            BankListView.ItemsSource = _startGameService.AllDominos;

            _aIService = new AIService(new LogicService(_startGameService.TableDominoCollection,
                _startGameService.OpponentDominosCollection, _startGameService.BankService));
            _aIService.AiTurnFinished += AiService_AiTurnFinished;

            RefreshAllItemSources();
        }

        private void AiService_AiTurnFinished()
        {
            RefreshAllItemSources();
        }

        private void TakeButton_Click(object sender, RoutedEventArgs e)
        {
            _logicService.TakeDominoFromBank();

            RefreshAllItemSources();
        }

        private void MyHandLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var image = sender as Image;
            DeselectSelectedDomino();

            _selectedDomino = image;
            var domino = image.DataContext as DominoModel;
            if (_startGameService.TableDominoCollection.IsDominoOkForLeft(domino))
            {
                _startGameService.TableDominoCollection.MakePreviousDominoSelectable();
            }

            if (_startGameService.TableDominoCollection.IsDominoOkForRight(domino))
            {
                _startGameService.TableDominoCollection.MakeNextDominoSelectable();
            }
            TopTableListView.Items.Refresh();
            LeftTableListView.Items.Refresh();
            RightTableListView.Items.Refresh();
            BotTableListView.Items.Refresh();
        }

        private void DeselectSelectedDomino()
        {
            if (_selectedDomino != null)
            {
                _startGameService.TableDominoCollection.DeselectDominos();
            }
        }

        private void RefreshAllItemSources()
        {
            TopTableListView.Items.Refresh();
            LeftTableListView.Items.Refresh();
            RightTableListView.Items.Refresh();
            BotTableListView.Items.Refresh();
            MyHandListView.Items.Refresh();
            OpponentHandListView.Items.Refresh();
            BankListView.Items.Refresh();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var image = sender as Image;

            if (image.Source.ToString().Contains("Selectable"))
            {
                _startGameService.TableDominoCollection.TryAddDomino(_selectedDomino.DataContext as DominoModel,
                    image.DataContext as DominoModel);

                _selectedDomino = null;
                _startGameService.TableDominoCollection.DeselectDominos();
                _aIService.StartTurn();
                RefreshAllItemSources();
            }
        }

        private void PassButton_Click(object sender, RoutedEventArgs e)
        {
            if (_logicService.IsBankEmpty())
            {
                _aIService.StartTurn();
                RefreshAllItemSources();
            }
        }
    }
}
