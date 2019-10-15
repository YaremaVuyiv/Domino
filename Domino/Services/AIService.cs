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

            if (GetAvailableDominos().Count == 0)
            {
                return;
            }

            if(GetPressureCalculatedWithTable(out var superPressureDomino))
            {
                _logicService.AddDominoIfAvailable(superPressureDomino);

                AiTurnFinished?.Invoke();
                return;
            }

            if (IsOnlyMove(out var onlyDomino))
            {
                _logicService.AddDominoIfAvailable(onlyDomino);

                AiTurnFinished?.Invoke();
                return;
            }

            if (HasPressure(out var pressureValue))
            {
                var pressureDomino = GetAvailableDominos(pressureValue).LastOrDefault();// GetPressureDomino(pressureValue);
                if (pressureDomino != null)
                {
                    _logicService.AddDominoIfAvailable(pressureDomino);

                    AiTurnFinished?.Invoke();
                    return;
                }
            }

            var defenderDomino = GetDefenderDomino();

            _logicService.AddDominoIfAvailable(defenderDomino);
            AiTurnFinished?.Invoke();
        }

        private bool GetPressureCalculatedWithTable(out DominoModel domino)
        {
            /*var edgeValues = new List<byte> { _logicService.TableRightNumber, _logicService.TableLeftNumber };
            _logicService.MyDominos.Join(edgeValues, d=>d.First, d=>d, (a, b)=> new {First = a. })*/

            var dictionary = new SortedDictionary<int, int>();
            _logicService.MyDominos.ForEach(d =>
            {
                if (dictionary.ContainsKey(d.First))
                {
                    dictionary[d.First]++;
                }
                else
                {
                    dictionary.Add(d.First, 1);
                }

                if (dictionary.ContainsKey(d.Second))
                {
                    dictionary[d.Second]++;
                }
                else
                {
                    dictionary.Add(d.Second, 1);
                }
            });

            _logicService.TableDominos.ForEach(d =>
            {
                if (dictionary.ContainsKey(d.First))
                {
                    dictionary[d.First]++;
                }

                if (dictionary.ContainsKey(d.Second))
                {
                    dictionary[d.Second]++;
                }
            });

            DominoModel result = null;

            dictionary.Where(p => p.Value >= 6).ToList().ForEach(d =>
            {
                var availableDominos = GetAvailableDominos(d.Key);
                if (availableDominos.Count != 0)
                {
                    result = availableDominos.LastOrDefault();
                }
            });

            if (result != null)
            {
                domino = result;
                return true;
            }

            domino = null;
            return false;
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

        private DominoModel GetPressureDomino(int pressureValue)
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

        private bool HasPressure(out int value)
        {
            var array = new int[7];
            for (int i = 6; i >= 0; i--)
            {
                array[i] = _logicService.MyDominos.Where(d => d.ContainsNumber(i)).Count();
            }

            var index = array.ToList().IndexOf(array.Max());
            value = index;

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

        private List<DominoModel> GetAvailableDominos(int pressureNumber)
        {
            return _logicService.MyDominos
                .Where(d =>
                {
                    var isFirstNumberOkForTable = d.First == _logicService.TableLeftNumber
                    || d.First == _logicService.TableRightNumber;
                    var isSecondNumberOkForTable = d.Second == _logicService.TableLeftNumber
                    || d.Second == _logicService.TableRightNumber;
                    var isFirstNumberOkForPressure = d.First == pressureNumber;
                    var isSecondNumberOkForPressure = d.Second == pressureNumber;

                    return (isFirstNumberOkForTable && isSecondNumberOkForPressure) ||
                    (isSecondNumberOkForTable && isFirstNumberOkForPressure);
                }).ToList();
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
