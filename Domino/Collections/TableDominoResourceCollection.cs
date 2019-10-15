using Domino.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino.Collections
{
    public class TableDominoResourceCollection
    {
        public List<DominoModel> TopDominos { get; }
        public List<DominoModel> BottomDominos { get; }
        public List<DominoModel> LeftDominos { get; }
        public List<DominoModel> RightDominos { get; }

        public List<DominoModel> Dominos { get; }

        public DominoModel NextDomino { get; private set; }
        private bool _isNextHorizontal;
        public DominoModel PreviousDomino { get; private set; }
        private bool _isPrevHorizontal;

        public int LeftNumber { get; private set; }
        public int RightNumber { get; private set; }

        public event Action<DominoModel> TableCollectionDominoAdded;

        public TableDominoResourceCollection(DominoModel startDomino)
        {
            TopDominos = GetHorizontalBackDominos().ToList();
            TopDominos[3].ImageResource = $"Images/_{startDomino.First}h{startDomino.Second}_.png";

            Dominos = new List<DominoModel>
            {
                startDomino
            };

            PreviousDomino = TopDominos[2];
            _isPrevHorizontal = true;
            NextDomino = TopDominos[4];
            _isNextHorizontal = true;
            LeftNumber = startDomino.First;
            RightNumber = startDomino.Second;
            BottomDominos = GetHorizontalBackDominos().ToList();
            LeftDominos = GetVerticalBackDominos().ToList();
            RightDominos = GetVerticalBackDominos().ToList();
        }

        private IEnumerable<DominoModel> GetHorizontalBackDominos()
        {
            for (int i = 0; i < 7; i++)
            {
                yield return new DominoModel(-1, -1)
                {
                    ImageResource = "Images/horizontalBack.png"
                };
            }
        }

        private IEnumerable<DominoModel> GetVerticalBackDominos()
        {
            for (int i = 0; i < 7; i++)
            {
                yield return new DominoModel(-1, -1)
                {
                    ImageResource = "Images/verticalBack.png"
                };
            }
        }

        public void TryAddDomino(DominoModel domino, DominoModel edgeDomino)
        {
            var edge = edgeDomino == PreviousDomino ? EdgeToAdd.Left : EdgeToAdd.Right;

            switch (edge)
            {
                case EdgeToAdd.Left:
                    {
                        MovePrevDomino(domino);
                        Dominos.Prepend(domino);
                        TableCollectionDominoAdded?.Invoke(domino);
                        LeftNumber = domino.First == LeftNumber ? domino.Second : domino.First;
                        break;
                    }
                case EdgeToAdd.Right:
                    {
                        MoveNextDomino(domino);
                        Dominos.Add(domino);
                        TableCollectionDominoAdded?.Invoke(domino);
                        RightNumber = domino.First == RightNumber ? domino.Second : domino.First;
                        break;
                    }
            }
        }

        public bool IsDominoOkForRight(DominoModel dominoModel)
        {
            return RightNumber == dominoModel.First || RightNumber == dominoModel.Second;
        }

        public bool IsDominoOkForLeft(DominoModel dominoModel)
        {
            return LeftNumber == dominoModel.First || LeftNumber == dominoModel.Second;
        }

        public void MakePreviousDominoSelectable()
        {
            var direction = _isPrevHorizontal ? "horizontal" : "vertical";
            PreviousDomino.ImageResource = $"Images/{direction}Selectable.png";
        }

        public void MakeNextDominoSelectable()
        {
            var direction = _isNextHorizontal ? "horizontal" : "vertical";
            NextDomino.ImageResource = $"Images/{direction}Selectable.png";
        }

        public void DeselectDominos()
        {
            var prevDirection = _isPrevHorizontal ? "horizontal" : "vertical";
            var nextDirection = _isNextHorizontal ? "horizontal" : "vertical";
            PreviousDomino.ImageResource = $"Images/{prevDirection}Back.png";
            NextDomino.ImageResource = $"Images/{nextDirection}Back.png";
        }

        private void MovePrevDomino(DominoModel domino)
        {
            var notEdgeSide = domino.First == LeftNumber ? domino.Second : domino.First;
            if (TopDominos.Exists(d => d == PreviousDomino))
            {
                domino.ImageResource = $"Images/_{notEdgeSide}h{LeftNumber}_.png";

                if (TopDominos.First() == PreviousDomino)
                {
                    TopDominos[0] = domino;
                    PreviousDomino = LeftDominos.First();
                    _isPrevHorizontal = false;
                }
                else
                {
                    var prevDominoIndex = TopDominos.FindIndex(d => d == PreviousDomino);
                    TopDominos[prevDominoIndex] = domino;
                    PreviousDomino = TopDominos[prevDominoIndex - 1];
                }
            }
            else if (LeftDominos.Exists(d => d == PreviousDomino))
            {
                domino.ImageResource = $"Images/_{LeftNumber}v{notEdgeSide}_.png";

                if (LeftDominos.Last() == PreviousDomino)
                {
                    LeftDominos[6] = domino;
                    PreviousDomino = BottomDominos.First();
                    _isPrevHorizontal = true;
                }
                else
                {
                    var prevDominoIndex = LeftDominos.FindIndex(d => d == PreviousDomino);
                    LeftDominos[prevDominoIndex] = domino;
                    PreviousDomino = LeftDominos[prevDominoIndex + 1];
                }
            }
            else if (RightDominos.Exists(d => d == PreviousDomino))
            {
                domino.ImageResource = $"Images/_{notEdgeSide}v{LeftNumber}_.png";

                if (RightDominos.First() == PreviousDomino)
                {
                    RightDominos[0] = domino;
                    PreviousDomino = TopDominos.Last();
                    _isPrevHorizontal = true;
                }
                else
                {
                    var prevDominoIndex = RightDominos.FindIndex(d => d == PreviousDomino);
                    RightDominos[prevDominoIndex] = domino;
                    PreviousDomino = RightDominos[prevDominoIndex - 1];
                }
            }
            else if (BottomDominos.Exists(d => d == PreviousDomino))
            {
                domino.ImageResource = $"Images/_{LeftNumber}h{notEdgeSide}_.png";

                if (BottomDominos.Last() == PreviousDomino)
                {
                    BottomDominos[6] = domino;
                    PreviousDomino = RightDominos.Last();
                    _isPrevHorizontal = false;
                }
                else
                {
                    var prevDominoIndex = BottomDominos.FindIndex(d => d == PreviousDomino);
                    BottomDominos[prevDominoIndex] = domino;
                    PreviousDomino = BottomDominos[prevDominoIndex + 1];
                }
            }
        }

        private void MoveNextDomino(DominoModel domino)
        {
            var notEdgeSide = domino.First == RightNumber ? domino.Second : domino.First;
            if (TopDominos.Exists(d => d == NextDomino))
            {
                domino.ImageResource = $"Images/_{RightNumber}h{notEdgeSide}_.png";

                if (TopDominos.Last() == NextDomino)
                {
                    TopDominos[6] = domino;
                    NextDomino = RightDominos.First();
                    _isNextHorizontal = false;
                }
                else
                {
                    var nextDominoIndex = TopDominos.FindIndex(d => d == NextDomino);
                    TopDominos[nextDominoIndex] = domino;
                    NextDomino = TopDominos[nextDominoIndex + 1];
                }
            }
            else if (LeftDominos.Exists(d => d == NextDomino))
            {
                domino.ImageResource = $"Images/_{notEdgeSide}v{RightNumber}_.png";

                if (LeftDominos.First() == NextDomino)
                {
                    LeftDominos[0] = domino;
                    NextDomino = TopDominos.First();
                    _isNextHorizontal = true;
                }
                else
                {
                    var nextDominoIndex = LeftDominos.FindIndex(d => d == NextDomino);
                    LeftDominos[nextDominoIndex] = domino;
                    NextDomino = LeftDominos[nextDominoIndex - 1];
                }
            }
            else if (RightDominos.Exists(d => d == NextDomino))
            {
                domino.ImageResource = $"Images/_{RightNumber}v{notEdgeSide}_.png";

                if (RightDominos.Last() == NextDomino)
                {
                    RightDominos[6] = domino;
                    NextDomino = BottomDominos.Last();
                    _isNextHorizontal = true;
                }
                else
                {
                    var nextDominoIndex = RightDominos.FindIndex(d => d == NextDomino);
                    RightDominos[nextDominoIndex] = domino;
                    NextDomino = RightDominos[nextDominoIndex + 1];
                }
            }
            else if (BottomDominos.Exists(d => d == NextDomino))
            {
                domino.ImageResource = $"Images/_{notEdgeSide}h{RightNumber}_.png";

                if (BottomDominos.First() == NextDomino)
                {
                    BottomDominos[0] = domino;
                    NextDomino = LeftDominos.Last();
                    _isNextHorizontal = false;
                }
                else
                {
                    var nextDominoIndex = BottomDominos.FindIndex(d => d == NextDomino);
                    BottomDominos[nextDominoIndex] = domino;
                    NextDomino = BottomDominos[nextDominoIndex - 1];
                }
            }
        }

        public enum EdgeToAdd
        {
            Right,
            Left
        }
    }
}
