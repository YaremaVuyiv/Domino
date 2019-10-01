using System;

namespace Domino.Models
{
    public struct DominoModel : IComparable
    {
        public DominoModel(byte first, byte second)
        {
            First = first;
            Second = second;
        }

        public DominoModel(string stringContent)
        {
            First = byte.Parse(stringContent.Split('/')[0]);
            Second = byte.Parse(stringContent.Split('/')[1]);
        }

        public byte First { get; private set; }

        public byte Second { get; private set; }

        public string StringValue { get => $"{First}/{Second}"; }

        public int CompareTo(object obj)
        {
            var compareObject = (DominoModel)obj;
            return (First + Second).CompareTo(compareObject.First + compareObject.Second);
        }

        public override bool Equals(object obj)
        {
            if (obj is DominoModel)
            {
                var equalObject = (DominoModel)obj;
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
