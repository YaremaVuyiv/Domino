using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Domino
{
    public class MyDominosCollection : HandDominoCollection
    {
        private DominoModel _selectedDomino;

        public MyDominosCollection(IEnumerable<DominoModel> dominos, TableDominoCollection tableDominoCollection)
            : base(dominos, tableDominoCollection, true)
        {
            Dominos.CollectionChanged += MyDominos_CollectionChanged;
            TableDominosCollection.LeftDominoClicked += TableDominosCollection_LeftDominoClicked;
            TableDominosCollection.RightDominoClicked += TableDominosCollection_RightDominoClicked;

            DominosLables.ForEach(label =>
            {
                label.MouseLeftButtonDown += DominoLabel_MouseLeftButtonDown;
            });
        }

        private void TableDominosCollection_RightDominoClicked(FrameworkElement obj)
        {
            if (TableDominosCollection.IsDominoOkForRight(_selectedDomino))
            {
                TableDominosCollection.SetRightTableDominoColor(Brushes.Turquoise);
                TableDominosCollection.SetLeftTableDominoColor(Brushes.Turquoise);
                Dominos.Remove(_selectedDomino);
                TableDominosCollection.Dominos.Add(_selectedDomino);
            }
        }

        private void TableDominosCollection_LeftDominoClicked(FrameworkElement obj)
        {
            if (TableDominosCollection.IsDominoOkForLeft(_selectedDomino))
            {
                TableDominosCollection.SetRightTableDominoColor(Brushes.Turquoise);
                TableDominosCollection.SetLeftTableDominoColor(Brushes.Turquoise);
                Dominos.Remove(_selectedDomino);
                TableDominosCollection.Dominos.Insert(0, _selectedDomino);
            }
        }

        private void MyDominos_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        var domino = e.NewItems.Cast<DominoModel>().Single();
                        var dominoLabel = domino.GetVisibleLabel();
                        dominoLabel.MouseLeftButtonDown += DominoLabel_MouseLeftButtonDown;
                        DominosLables.Add(dominoLabel);
                        break;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {
                        var domino = e.OldItems.Cast<DominoModel>().Single();
                        var dominoLabel = DominosLables.FirstOrDefault(d => d.Name == domino.ToString());
                        if (dominoLabel != null)
                        {
                            dominoLabel.MouseLeftButtonDown -= DominoLabel_MouseLeftButtonDown;
                            DominosLables.Remove(dominoLabel);
                        }
                        break;
                    }
            }
        }

        private void DominoLabel_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //Change to image when we began to use it
            var label = sender as Label;
            var previousSelectedLabel = DominosLables
                .FirstOrDefault(d => d.Name == _selectedDomino.ToString()) as Label;
            TableDominosCollection.SetRightTableDominoColor(Brushes.Turquoise);
            TableDominosCollection.SetLeftTableDominoColor(Brushes.Turquoise);
            if (previousSelectedLabel != null)
            {
                previousSelectedLabel.Background = Brushes.Turquoise;
            }
            _selectedDomino = Dominos.First(d => d.ToString() == label.Name);
            label.Background = Brushes.Green;

            if (TableDominosCollection.IsDominoOkForRight(_selectedDomino))
            {
                TableDominosCollection.SetRightTableDominoColor(Brushes.Green);
            }

            if (TableDominosCollection.IsDominoOkForLeft(_selectedDomino))
            {
                TableDominosCollection.SetLeftTableDominoColor(Brushes.Green);
            }
        }

        public bool HasDominosToPut()
        {
            return Dominos.Any(d =>
            {
                return d.First == TableDominosCollection.LeftNumber || d.First == TableDominosCollection.RightNumber
                || d.Second == TableDominosCollection.RightNumber || d.Second == TableDominosCollection.LeftNumber;
            });
        }
    }
}
