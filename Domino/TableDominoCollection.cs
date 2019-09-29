using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Domino
{
    public class TableDominoCollection : DominosBaseCollection
    {
        public FrameworkElement LeftDominoElement { get; private set; }
        public FrameworkElement RightDominoElement { get; private set; }

        public byte LeftNumber { get; private set; }
        public byte RightNumber { get; private set; }

        public event Action<FrameworkElement> LeftDominoClicked;
        public event Action<FrameworkElement> RightDominoClicked;

        public TableDominoCollection(IEnumerable<DominoModel> dominos) : base(dominos, true)
        {
            Dominos.CollectionChanged += TableDominos_CollectionChanged;

            LeftDominoElement = "_xix_".GetVisibleLabel();
            LeftDominoElement.MouseLeftButtonDown += LeftDominoElement_MouseLeftButtonDown;
            RightDominoElement = "_xix_".GetVisibleLabel();
            RightDominoElement.MouseLeftButtonDown += RightDominoElement_MouseLeftButtonDown;
            DominosLables.Insert(0, LeftDominoElement);
            DominosLables.Add(RightDominoElement);
            LeftNumber = dominos.ToList().SingleOrDefault().First;
            RightNumber = dominos.ToList().SingleOrDefault().Second;
        }

        private void RightDominoElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RightDominoClicked?.Invoke(sender as FrameworkElement);
        }

        private void LeftDominoElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LeftDominoClicked?.Invoke(sender as FrameworkElement);
        }

        private void TableDominos_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var domino = e.NewItems.Cast<DominoModel>().Single();
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        
                        if (Dominos.First().Equals(domino))
                        {
                            if(LeftNumber == domino.First)
                            {
                                LeftNumber = domino.Second;
                                domino.SwapValues();
                            }
                            else
                            {
                                LeftNumber = domino.First;
                            }
                            var dominoLabel = domino.GetVisibleLabel();
                            DominosLables.Insert(1, dominoLabel);
                        }
                        else
                        {
                            if(RightNumber == domino.First)
                            {
                                RightNumber = domino.Second;
                            }
                            else
                            {
                                RightNumber = domino.First;
                                domino.SwapValues();
                            }
                            var dominoLabel = domino.GetVisibleLabel();
                            DominosLables.Insert(DominosLables.Count - 1, dominoLabel);
                        }
                        break;
                    }
            }
        }

        public void SetLeftTableDominoColor(SolidColorBrush color)
        {
            (LeftDominoElement as Label).Background = color;
        }

        public void SetRightTableDominoColor(SolidColorBrush color)
        {
            (RightDominoElement as Label).Background = color;
        }

        public bool IsDominoOkForRight(DominoModel dominoModel)
        {
            return RightNumber == dominoModel.First || RightNumber == dominoModel.Second;
        }

        public bool IsDominoOkForLeft(DominoModel dominoModel)
        {
            return LeftNumber == dominoModel.First || LeftNumber == dominoModel.Second;
        }
    }
}
