using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CardBoard.Models;
using Assisticant;
using CardBoard.BoardView;

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

        public bool Busy
        {
            get { return _application.Busy; }
        }

        public string LastError
        {
            get { return _application.LastError; }
        }

        public BoardViewModel BoardDetail
        {
            get { return new BoardViewModel(_application.Board, _selection); }
        }

        public bool CanDeleteCard
        {
            get { return _selection.SelectedCard != null; }
        }

        public void DeleteCard()
        {
            _application.ReceiveMessage(_application.Board.DeleteCard(
                _selection.SelectedCard));
        }

        public bool CanEditCard
        {
            get { return _selection.SelectedCard != null; }
        }

        public void EditCard()
        {
            Card card = _selection.SelectedCard;
            DialogManager.ShowCardDetail(new CardDetailModel
            {
                Text = card.Text
                    .OrderBy(t => t.MessageHash)
                    .Select(t => t.Value)
                    .FirstOrDefault()
            }, detail =>
            {
                _application.ReceiveMessage(card.SetText(detail.Text));
            });
        }

        public void NewCard()
        {
            DialogManager.ShowCardDetail(new CardDetailModel(), detail =>
            {
                var cardId = Guid.NewGuid();
                _application.ReceiveMessage(_application.Board.CreateCard(cardId));
                var card = _application.Board.Cards.Single(c => c.CardId == cardId);
                _application.ReceiveMessage(card.SetText(detail.Text));
                _application.ReceiveMessage(card.MoveTo(Column.ToDo));
            });
        }

        public void Refresh()
        {
            _application.Refresh();
        }
    }
}
