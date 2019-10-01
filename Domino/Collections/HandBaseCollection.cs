using System.Collections.Generic;
using System.Linq;
using System.Collections.Specialized;
using Domino.Models;

namespace Domino.Collections
{
    public class HandBaseCollection
    {
        public List<DominoModel> Dominos { get; }
        private TableDominoCollection _tableDominosCollection;

        public HandBaseCollection(IEnumerable<DominoModel> dominos, TableDominoCollection tableDominoCollection)
        {
            Dominos = dominos.ToList();
            _tableDominosCollection = tableDominoCollection;

            _tableDominosCollection.TableCollectionChanged += TableDominos_CollectionChanged;
        }

        public void AddNewDomino(DominoModel domino)
        {
            if (!HasDominoToPlace())
            {
                Dominos.Add(domino);
            }
        }

        private void TableDominos_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        var domino = e.NewItems.Cast<DominoModel>().Single();
                        if (Dominos.Contains(domino))
                        {
                            Dominos.Remove(domino);
                        }
                        break;
                    }
            }
        }

        public bool HasDominoToPlace()
        {
            var result = false;
            Dominos.ForEach(d =>
            {
                if (_tableDominosCollection.IsDominoOkForLeft(d) || _tableDominosCollection.IsDominoOkForRight(d))
                {
                    result = true;
                }
            });

            return result;
        }
    }
}
