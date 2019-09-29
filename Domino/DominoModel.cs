using System;
using System.Linq;

namespace Domino
{
    public struct DominoModel : IComparable
    {
        public DominoModel(byte first, byte second)
        {
            First = first;
            Second = second;
        }

        public DominoModel(string name)
        {
            var splittedName = name.Split('i');
            First = Convert.ToByte(splittedName.First());
            Second = Convert.ToByte(splittedName.Last());
        }

        public byte First { get; private set; }

        public byte Second { get; private set; }

        public int CompareTo(object obj)
        {
            var compareObject = (DominoModel)obj;
            return (First + Second).CompareTo(compareObject.First + compareObject.Second);
        }

        public override string ToString()
        {
            return $"_{First}i{Second}_";
        }

        public override bool Equals(object obj)
        {
            var equalObject = (DominoModel)obj;
            return (First == equalObject.First && Second == equalObject.Second) ||
                (First == equalObject.Second && Second == equalObject.First);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void SwapValues()
        {
            var swapValue = First;
            First = Second;
            Second = swapValue;
        }
    }
}
