using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Domino
{
    public abstract class HandDominoCollection : DominosBaseCollection
    {
        public TableDominoCollection TableDominosCollection { get; private set; }

        public HandDominoCollection(IEnumerable<DominoModel> dominos,
            TableDominoCollection tableDominos, bool isVisible)
            : base(dominos, isVisible)
        {
            TableDominosCollection = tableDominos;
        }
    }
}
