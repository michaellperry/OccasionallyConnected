using Assisticant.Collections;
using Assisticant.Fields;
using CardBoard.Messaging;
using System;
using System.Collections.Generic;

namespace CardBoard.Models
{
    public class Board
    {
        private Observable<string> _name = new Observable<string>("Pluralsight");
        private ObservableList<Card> _cards = new ObservableList<Card>();

        public string Name
        {
            get { return _name; }
        }

        public IEnumerable<Card> Cards
        {
            get { return _cards; }
        }

        public Message CreateCard(Guid cardId)
        {
            throw new NotImplementedException();
        }

        public Message DeleteCard(Guid cardId)
        {
            throw new NotImplementedException();
        }
    }
}
