using System.Collections.Generic;
using System.Linq;
using System.Collections.Specialized;
using System.Windows.Controls;
using System.Windows.Media;

namespace Domino
{
    public class OpponentDominoCollection : HandDominoCollection
    {
        public OpponentDominoCollection(IEnumerable<DominoModel> dominos,
            TableDominoCollection tableDominoCollection) : base(dominos, tableDominoCollection, false)
        {
            Dominos.CollectionChanged += OpponentDominos_CollectionChanged;
        }

        private void OpponentDominos_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        var dominoLabel = GetHiddenLabel();
                        DominosLables.Add(dominoLabel);
                        break;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {
                        var dominoLabel = DominosLables.First();
                        DominosLables.Remove(dominoLabel);
                        break;
                    }
            }
        }

        public void PutDominoLeft(DominoModel domino)
        {
            if (TableDominosCollection.IsDominoOkForLeft(domino))
            {
                Dominos.Remove(domino);
                TableDominosCollection.Dominos.Insert(0, domino);
            }
        }

        public void PutDominoRight(DominoModel domino)
        {
            if (TableDominosCollection.IsDominoOkForRight(domino))
            {
                Dominos.Remove(domino);
                TableDominosCollection.Dominos.Add(domino);
            }
        }
    }
}
