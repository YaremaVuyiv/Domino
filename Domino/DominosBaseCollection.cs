using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Domino
{
    public abstract class DominosBaseCollection
    {
        public ObservableCollection<DominoModel> Dominos { get; private set; }
        public List<FrameworkElement> DominosLables { get; private set; }

        public DominosBaseCollection(IEnumerable<DominoModel> dominos, bool isVisible)
        {
            Dominos = new ObservableCollection<DominoModel>(dominos);

            DominosLables = dominos.ToList().Select(d => isVisible ? d.GetVisibleLabel() 
            : GetHiddenLabel() as FrameworkElement).ToList();
        }

        protected Label GetHiddenLabel()
        {
            var label = new Label
            {
                Height = 60,
                Width = 30,
                Background = Brushes.Black
            };
            return label;
        }
    }
}
