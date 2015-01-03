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
            get { return new BoardViewModel(_application.Board); }
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
                    .Do(() => { });
            }
        }

        public ICommand NewCard
        {
            get
            {
                return MakeCommand
                    .When(() => _selection.SelectedCard != null)
                    .Do(() => { });
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
