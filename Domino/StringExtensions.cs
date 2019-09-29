using System.Windows.Controls;
using System.Windows.Media;

namespace Domino
{
    public static class StringExtensions
    {
        public static Label GetVisibleLabel(this string name)
        {
            var label = new Label
            {
                Height = 60,
                Width = 30,
                Background = Brushes.Turquoise,
                Content = name.Replace('i', '/').Substring(1, 3),
                Name = name
            };
            return label;
        }
    }
}
