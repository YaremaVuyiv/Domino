using System.Collections.Generic;
using Domino.Models;
using System.Linq;
using System;

namespace Domino.Services
{
    public class AIService
    {
        public event Action AiTurnFinished;

        private readonly LogicService _logicService;

        public AIService(LogicService logicService)
        {
            _logicService = logicService;
            if (_logicService.MyDominos.Count == 7)
            {
                StartTurn();
            }
        }

        public void StartTurn()
        {
            while (!IsAnyMove() && !_logicService.IsBankEmpty())
            {
                _logicService.TakeDominoFromBank();
            }

            if(GetAvailableDominos().Count == 0)
            {
                return;
            }

            if (IsOnlyMove(out var onlyDomino))
            {
                if (_logicService.IsDominoOkForTableLeft(onlyDomino))
                {
                    _logicService.AddDominoToLeft(onlyDomino);
                }
                else
                {
                    _logicService.AddDominoToRight(onlyDomino);
                }

                AiTurnFinished?.Invoke();
                return;
            }

            if (HasPressure(out var pressureValue))
            {
                var pressureDomino = GetPressureDomino(pressureValue);
                if (pressureDomino != null)
                {
                    if (_logicService.IsDominoOkForTableLeft(pressureDomino))
                    {
                        _logicService.AddDominoToLeft(pressureDomino);
                    }
                    else
                    {
                        _logicService.AddDominoToRight(pressureDomino);
                    }

                    AiTurnFinished?.Invoke();
                    return;
                }
            }

            var defenderDomino = GetDefenderDomino();

            if (_logicService.IsDominoOkForTableLeft(defenderDomino))
            {
                _logicService.AddDominoToLeft(defenderDomino);
            }
            else
            {
                _logicService.AddDominoToRight(defenderDomino);
            }
            AiTurnFinished?.Invoke();
        }

        private DominoModel GetDefenderDomino()
        {
            var maxVal = 0;
            var domino = GetAvailableDominos().First();
            GetAvailableDominos().ForEach(d =>
            {
                var laterPryd = LaterPryd(d);
                if (laterPryd > maxVal)
                {
                    maxVal = laterPryd;
                    domino = d;
                }
            });

            return domino;
        }

        private DominoModel GetPressureDomino(byte pressureValue)
        {
            var availableDominosWithPressure = GetAvailableDominos()?.Where(d => d.ContainsNumber(pressureValue)).ToList();
            if (availableDominosWithPressure.Exists(d => d.First == d.Second))
            {
                return availableDominosWithPressure.Last(d => d.First == d.Second);
            }

            return availableDominosWithPressure
                .Where(d =>
                {
                    var notPressureValue = d.First == pressureValue ? d.Second : d.First;
                    return notPressureValue == _logicService.TableLeftNumber || notPressureValue == _logicService.TableRightNumber;
                }).LastOrDefault();
        }

        private bool HasPressure(out byte value)
        {
            var array = new byte[7];
            for (int i = 6; i >= 0; i--)
            {
                array[i] = (byte)_logicService.MyDominos.Where(d => d.ContainsNumber((byte)i)).Count();
            }

            var index = array.ToList().IndexOf(array.Max());
            value = (byte)index;

            return array[index] >= 3;
        }

        private bool IsAnyMove()
        {
            var leftAvailableDominos = _logicService.MyDominos.Where(d => _logicService.IsDominoOkForTableLeft(d));
            var rightAvailableDominos = _logicService.MyDominos.Where(d => _logicService.IsDominoOkForTableRight(d));

            return leftAvailableDominos.Count() != 0 || rightAvailableDominos.Count() != 0;
        }

        private bool IsOnlyMove(out DominoModel domino)
        {
            var availableDominos = GetAvailableDominos();
            if (availableDominos.Count == 1)
            {
                domino = availableDominos.Single();
                return true;
            }

            domino = null;
            return false;
        }

        private List<DominoModel> GetAvailableDominos()
        {
            var leftAvailableDominos = _logicService.MyDominos.Where(d => _logicService.IsDominoOkForTableLeft(d));
            var rightAvailableDominos = _logicService.MyDominos.Where(d => _logicService.IsDominoOkForTableRight(d));
            return leftAvailableDominos.Union(rightAvailableDominos).ToList();
        }

        private int LaterPryd(DominoModel domino)
        {
            var availableDominosWithoutSelected = GetAvailableDominos().Where(d => !d.Equals(domino)).ToList();
            var counter = 0;
            counter = availableDominosWithoutSelected.Exists(d => d.First == domino.First || d.Second == domino.First) ?
                ++counter : counter;

            counter = availableDominosWithoutSelected.Exists(d => d.First == domino.Second || d.Second == domino.Second) ?
                ++counter : counter;

            return counter;
        }
    }
}
