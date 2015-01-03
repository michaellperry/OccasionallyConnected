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

        public ICommand DeleteCard
        {
            get
            {
                return MakeCommand
                    .When(() => _selection.SelectedCard != null)
                    .Do(() => _application.Board.DeleteCard(_selection.SelectedCard));
            }
        }

        public ICommand EditCard
        {
            get
            {
                return MakeCommand
                    .When(() => _selection.SelectedCard != null)
                    .Do(() =>
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
                    });
            }
        }

        public ICommand NewCard
        {
            get
            {
                return MakeCommand
                    .Do(() =>
                    {
                        DialogManager.ShowCardDetail(new CardDetailModel(), detail =>
                        {
                            var cardId = Guid.NewGuid();
                            _application.ReceiveMessage(_application.Board.CreateCard(cardId));
                            var card = _application.Board.Cards.Single(c => c.CardId == cardId);
                            _application.ReceiveMessage(card.SetText(detail.Text));
                            _application.ReceiveMessage(card.MoveTo(Column.ToDo));
                        });
                    });
            }
        }

        public ICommand Refresh
        {
            get
            {
                return MakeCommand
                    .Do(() => _application.Refresh());
            }
        }
    }
}
