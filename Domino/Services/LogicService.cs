using System;
using System.Collections.Generic;
using System.Linq;
using Domino.Collections;
using Domino.Models;

namespace Domino.Services
{
    public class LogicService
    {
        public List<DominoModel> MyDominos { get => _myDominosCollection.Dominos; }
        public List<DominoModel> TableDominos { get => _tableDominoCollection.Dominos.ToList(); }

        public byte TableLeftNumber { get => _tableDominoCollection.LeftNumber; }
        public byte TableRightNumber { get => _tableDominoCollection.RightNumber; }

        private readonly HandBaseCollection _myDominosCollection;
        private readonly TableDominoCollection _tableDominoCollection;

        private readonly BankService _bankService;

        public LogicService(TableDominoCollection tableDominoCollection, HandBaseCollection handCollection,
            BankService bankService)
        {
            _tableDominoCollection = tableDominoCollection;

            _myDominosCollection = handCollection;

            _bankService = bankService;

        }

        public void TakeDominoFromBank()
        {
            if (!_myDominosCollection.HasDominoToPlace())
            {
                var bankDomino = _bankService.GetDominoFromBank();
                if (bankDomino != null)
                {
                    _myDominosCollection.AddNewDomino(bankDomino);
                }
            }
        }

        public bool IsBankEmpty()
        {
            return _bankService.IsBankEmpty();
        }

        public bool IsDominoOkForTableLeft(string domino)
        {
            return IsDominoOkForTableLeft(new DominoModel(domino));
        }

        public bool IsDominoOkForTableLeft(DominoModel domino)
        {
            return _tableDominoCollection.IsDominoOkForLeft(domino);
        }

        public bool IsDominoOkForTableRight(string domino)
        {
            return IsDominoOkForTableRight(new DominoModel(domino));
        }

        public bool IsDominoOkForTableRight(DominoModel domino)
        {
            return _tableDominoCollection.IsDominoOkForRight(domino);
        }

        public void AddDominoToLeft(string domino)
        {
            AddDominoToLeft(new DominoModel(domino));
        }

        public void AddDominoToRight(string domino)
        {
            AddDominoToRight(new DominoModel(domino));
        }

        public void AddDominoToLeft(DominoModel domino)
        {
            if (IsDominoOkForTableLeft(domino))
            {
                _tableDominoCollection.AddToLeft(domino);
            }

            if(_bankService.IsBankEmpty() && MyDominos.Count == 0)
            {

            }
        }

        public void AddDominoToRight(DominoModel domino)
        {
            if (IsDominoOkForTableRight(domino))
            {
                _tableDominoCollection.AddToRight(domino);
            }

            if (_bankService.IsBankEmpty() && MyDominos.Count == 0)
            {

            }
        }
    }
}
