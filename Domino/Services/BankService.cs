using Domino.Models;
using System;
using System.Collections.Generic;

namespace Domino.Services
{
    public class BankService
    {
        private readonly List<DominoModel> _allDominos;
        public BankService(List<DominoModel> dominos)
        {
            _allDominos = dominos;
        }

        public DominoModel GetDominoFromBank()
        {
            if (_allDominos.Count == 0)
            {
                return null;
            }

            var rnd = new Random();
            var index = rnd.Next(0, _allDominos.Count - 1);
            var result = _allDominos[index];
            _allDominos.RemoveAt(index);
            return result;
        }

        public bool IsBankEmpty()
        {
            return _allDominos.Count == 0;
        }
    }
}
