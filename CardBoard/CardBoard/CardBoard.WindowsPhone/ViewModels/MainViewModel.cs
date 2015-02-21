using CardBoard.BoardView;
using CardBoard.Models;
using System;

namespace CardBoard.ViewModels
{
    public class MainViewModel
    {
        private readonly Application _application;
        private readonly SelectionModel _selection;
        private readonly CardDetailModel _cardDetail;

        public MainViewModel(Application application, SelectionModel selection, CardDetailModel cardDetail)
        {
            _application = application;
            _selection = selection;
            _cardDetail = cardDetail;
        }

        public string BoardName
        {
            get { return _application.Board.Name; }
        }

        public BoardViewModel BoardDetail
        {
            get { return new BoardViewModel(_application, _application.Board, _selection); }
        }

        public Card SelectedCard
        {
            get { return _selection.SelectedCard; }
        }

        public void ClearSelection()
        {
            _selection.SelectedCard = null;
        }

        public void PrepareNewCard()
        {
            _cardDetail.Clear();
        }

        public void PrepareEditCard(Card card)
        {
            _cardDetail.FromCard(card);
        }

        public void Refresh()
        {
            _application.SendAndReceiveMessages();
        }

        public bool HasError
        {
            get { return _application.Exception != null; }
        }

        public string ErrorMessage
        {
            get
            {
                return _application.Exception == null ? null :
                    _application.Exception.Message;
            }
        }
    }
}
