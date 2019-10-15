using Domino.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Domino.Collections
{
    public class TableDominoCollection
    {
        public ObservableCollection<DominoModel> Dominos { get; }

        public int LeftNumber { get; private set; }
        public int RightNumber { get; private set; }

        public event Action<object, NotifyCollectionChangedEventArgs> TableCollectionChanged;

        public TableDominoCollection(IEnumerable<DominoModel> dominos)
        {
            Dominos = new ObservableCollection<DominoModel>(dominos);
            
            LeftNumber = dominos.ToList().SingleOrDefault().First;
            RightNumber = dominos.ToList().SingleOrDefault().Second;

            Dominos.CollectionChanged += Dominos_CollectionChanged;
        }

        private void Dominos_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            TableCollectionChanged.Invoke(sender, e);
        }

        public void AddToLeft(DominoModel domino)
        {
            if (IsDominoOkForLeft(domino))
            {
                if(LeftNumber != domino.Second)
                {
                    domino.SwapValues();
                }

                LeftNumber = domino.First;
                Dominos.Insert(0, domino);
            }
        }

        public void AddToRight(DominoModel domino)
        {
            if (IsDominoOkForRight(domino))
            {
                if(RightNumber != domino.First)
                {
                    domino.SwapValues();
                }

                RightNumber = domino.Second;
                Dominos.Add(domino);
            }
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
