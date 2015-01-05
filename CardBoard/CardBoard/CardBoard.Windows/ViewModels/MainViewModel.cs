using CardBoard.BoardView;
using CardBoard.Models;
using System;
using System.Linq;

namespace CardBoard.ViewModels
{
    public class MainViewModel
    {
        private readonly Application _application;
        private readonly SelectionModel _selection;
        
        public MainViewModel(Application application, SelectionModel selection)
        {
            _application = application;
            _selection = selection;
        }

        public string BoardName
        {
            get { return _application.Board.Name; }
        }

        public BoardViewModel BoardDetail
        {
            get { return new BoardViewModel(_application, _application.Board, _selection); }
        }

        public bool CanDeleteCard
        {
            get { return _selection.SelectedCard != null; }
        }

        public void DeleteCard()
        {
            _application.HandleMessage(_application.Board.DeleteCard(
                _selection.SelectedCard.CardId));
        }

        public bool CanEditCard
        {
            get { return _selection.SelectedCard != null; }
        }

        public void EditCard()
        {
            DialogManager.ShowCardDetail(new CardDetailModel
            {
                Text = _selection.SelectedCard.Text
                    .OrderBy(c => c.MessageHash)
                    .Select(c => c.Value)
                    .FirstOrDefault()
            }, detail =>
            {
                _application.HandleMessage(
                    _selection.SelectedCard.SetText(detail.Text));
            });
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
