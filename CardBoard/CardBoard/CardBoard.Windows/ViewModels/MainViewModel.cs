using CardBoard.BoardView;
using CardBoard.Models;
using System;
using System.Linq;

namespace CardBoard.ViewModels
{
    public class MainViewModel
    {
        private readonly Application _application;
        
        public MainViewModel(Application application)
        {
            _application = application;
        }

        public string BoardName
        {
            get { return _application.Board.Name; }
        }

        public BoardViewModel BoardDetail
        {
            get { return new BoardViewModel(_application, _application.Board); }
        }

        public void NewCard()
        {
            DialogManager.ShowCardDetail(new CardDetailModel(), detail =>
            {
                var cardId = Guid.NewGuid();
                _application.HandleMessage(_application.Board.CreateCard(cardId));
                var card = _application.Board.Cards.Single(c => c.CardId == cardId);
                _application.HandleMessage(card.SetText(detail.Text));
                _application.HandleMessage(card.MoveTo(Column.ToDo));
            });
        }
    }
}
