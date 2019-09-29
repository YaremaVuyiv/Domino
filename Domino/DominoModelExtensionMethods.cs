using System.Windows.Controls;

namespace Domino
{
    public static class DominoModelExtensionMethods
    {
        public static Label GetVisibleLabel(this DominoModel model)
        {
            var label = model.ToString().GetVisibleLabel();
            return label;
        }
    }
}
