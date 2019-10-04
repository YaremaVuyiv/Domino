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
        private Label _selectedDomino;

        private readonly LogicService _logicService;
        private readonly StartGameService _startGameService;
        private readonly AIService _aIService;

        public MainWindow()
        {
            InitializeComponent();
            _startGameService = new StartGameService();
            _logicService = new LogicService(_startGameService.TableDominoCollection,
                _startGameService.MyDominosCollection, _startGameService.BankService);

            _aIService = new AIService(new LogicService(_startGameService.TableDominoCollection,
                _startGameService.OpponentDominosCollection, _startGameService.BankService));
            _aIService.AiTurnFinished += AiService_AiTurnFinished;

            MyHandListView.ItemsSource = _startGameService.MyDominosCollection.Dominos;
            TableListView.ItemsSource = _startGameService.TableDominoCollection.Dominos;
            OpponentHandListView.ItemsSource = _startGameService.OpponentDominosCollection.Dominos;
            BankListView.ItemsSource = _startGameService.AllDominos;
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
            var label = sender as Label;
            DeselectSelectedDomino();

            _selectedDomino = label;

            if (_logicService.IsDominoOkForTableLeft(label.Content.ToString()))
            {
                TableLeftDomino.Visibility = Visibility.Visible;
            }

            if (_logicService.IsDominoOkForTableRight(label.Content.ToString()))
            {
                TableRightDomino.Visibility = Visibility.Visible;
            }
            label.Background = Brushes.Green;
        }

        private void TableLeftDomino_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DeselectSelectedDomino();
            _logicService.AddDominoToLeft(_selectedDomino.Content.ToString());
            RefreshAllItemSources();

            _aIService.StartTurn();
        }

        private void TableRightDomino_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DeselectSelectedDomino();
            _logicService.AddDominoToRight(_selectedDomino.Content.ToString());
            RefreshAllItemSources();

            _aIService.StartTurn();
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
