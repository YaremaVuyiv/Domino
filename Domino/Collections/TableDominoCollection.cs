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

        private byte _leftNumber;
        private byte _rightNumber;

        public event Action<object, NotifyCollectionChangedEventArgs> TableCollectionChanged;

        public TableDominoCollection(IEnumerable<DominoModel> dominos)
        {
            Dominos = new ObservableCollection<DominoModel>(dominos);
            
            _leftNumber = dominos.ToList().SingleOrDefault().First;
            _rightNumber = dominos.ToList().SingleOrDefault().Second;

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
                if(_leftNumber != domino.Second)
                {
                    domino.SwapValues();
                }

                _leftNumber = domino.First;
                Dominos.Insert(0, domino);
            }
        }

        public void AddToRight(DominoModel domino)
        {
            if (IsDominoOkForRight(domino))
            {
                if(_rightNumber != domino.First)
                {
                    domino.SwapValues();
                }

                _rightNumber = domino.Second;
                Dominos.Add(domino);
            }
        }

        public bool IsDominoOkForRight(DominoModel dominoModel)
        {
            return _rightNumber == dominoModel.First || _rightNumber == dominoModel.Second;
        }

        public bool IsDominoOkForLeft(DominoModel dominoModel)
        {
            return _leftNumber == dominoModel.First || _leftNumber == dominoModel.Second;
        }
    }
}
