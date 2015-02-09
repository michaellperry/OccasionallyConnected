using Assisticant;
using CardBoard.BoardView;
using CardBoard.Models;

namespace CardBoard.ViewModels
{
    public class ViewModelLocator : ViewModelLocatorBase
    {
        private Application _application;
        private SelectionModel _selection;
        private CardDetailModel _cardDetail;

        public ViewModelLocator()
        {
            if (DesignMode)
                _application = Initializer.LoadDesignModeApplication();
            else
                _application = Initializer.LoadApplication();
            _selection = new SelectionModel();
            _cardDetail = new CardDetailModel();
        }

        public object Main
        {
            get
            {
                return ViewModel(() => new MainViewModel(
                    _application,
                    _selection,
                    _cardDetail));
            }
        }

        public object CardDetail
        {
            get
            {
                return ViewModel(() => new CardDetailViewModel(
                    _cardDetail,
                    _application,
                    _selection.SelectedCard));
            }
        }
    }
}
