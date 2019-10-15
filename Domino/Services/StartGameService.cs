using Domino.Collections;
using Domino.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domino.Services
{
    public class StartGameService
    {
        public HandBaseCollection MyDominosCollection { get; }
        public HandBaseCollection OpponentDominosCollection { get; }
        public TableDominoResourceCollection TableDominoCollection { get; }

        public List<DominoModel> AllDominos { get; }

        public BankService BankService { get; }

        public StartGameService()
        {
            AllDominos = GetAllDominos();

            BankService = new BankService(AllDominos);

            var opponentsDominos = TakeSevenRandomDominos();
            var myDominos = TakeSevenRandomDominos();

            var unionDominos = myDominos.Union(opponentsDominos);
            var startDomino = unionDominos
                .Where(d => d.First == d.Second)
                .DefaultIfEmpty(unionDominos.Max())
                .Min();

            //TableDominoCollection = new TableDominoCollection(new List<DominoModel> { startDomino });
            TableDominoCollection = new TableDominoResourceCollection(startDomino);
            MyDominosCollection = new HandBaseCollection(myDominos, TableDominoCollection);
            OpponentDominosCollection = new HandBaseCollection(opponentsDominos, TableDominoCollection);

            if (myDominos.Contains(startDomino))
            {
                MyDominosCollection.Dominos.Remove(startDomino);
                /*while (!OpponentDominosCollection.HasDominoToPlace() && AllDominos.Count > 0)
                {
                    OpponentDominosCollection.AddNewDomino(GetRandomDomino());
                }*/
                //_aIService.StartTurn();
            }
            else
            {
                OpponentDominosCollection.Dominos.Remove(startDomino);
            }
        }

        private List<DominoModel> GetAllDominos()
        {
            var result = new List<DominoModel>();

            for (int i = 0; i < 7; i++)
            {
                for (int j = i; j < 7; j++)
                {
                    result.Add(new DominoModel(i, j));
                }
            }

            return result;
        }

        private List<DominoModel> TakeSevenRandomDominos()
        {
            var result = new List<DominoModel>();

            for (int i = 0; i < 7; i++)
            {
                result.Add(BankService.GetDominoFromBank());
            }

            return result;
        }
    }
}
