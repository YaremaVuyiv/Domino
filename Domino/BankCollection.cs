using System.Collections.Generic;
using System.Collections.Specialized;

namespace Domino
{
    class BankCollection : DominosBaseCollection
    {
        public BankCollection(IEnumerable<DominoModel> dominos) : base(dominos, false)
        {
            Dominos.CollectionChanged += Dominos_CollectionChanged;
        }

        private void Dominos_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    {
                        DominosLables.RemoveAt(0);

                        break;
                    }
            }
        }
    }
}
