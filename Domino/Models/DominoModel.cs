using System;
using System.Linq;

namespace Domino.Models
{
    public class DominoModel : IComparable
    {
        public DominoModel(int first, int second)
        {
            First = first;
            Second = second;
        }

        public DominoModel(string stringContent)
        {
            var imageName = stringContent.Split('/').Last();
            var digits = imageName.Where(c => char.IsDigit(c));
            First = int.Parse(digits.First().ToString());
            Second = int.Parse(digits.Last().ToString());
        }

        public int First { get; private set; }

        public int Second { get; private set; }

        //public string StringValue { get => $"{First}/{Second}"; }

        public string ImageResource { get; set; }

        //public string ImageVerticalResource { get => $"Images/_{First}v{Second}_.png"; }

        //public string ImageHorizontalResource { get => $"Images/_{First}h{Second}_.png"; }

        public int CompareTo(object obj)
        {
            var compareObject = (DominoModel)obj;
            return (First + Second).CompareTo(compareObject.First + compareObject.Second);
        }

        public bool ContainsNumber(int number)
        {
            return number == First || number == Second;
        }

        public override bool Equals(object obj)
        {
            if (obj is DominoModel equalObject)
            {
                return (First == equalObject.First && Second == equalObject.Second) ||
                    (First == equalObject.Second && Second == equalObject.First);
            }
            return false;
        }

        public void SwapValues()
        {
            var swapValue = First;
            First = Second;
            Second = swapValue;
        }
    }
}
