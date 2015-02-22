using Assisticant;
using CardBoard.BoardView;
using CardBoard.Models;
using CardBoard.Notification;

namespace CardBoard.ViewModels
{
    public class ViewModelLocator : ViewModelLocatorBase
    {
        private Application _application;
        private SelectionModel _selection;
        
        public ViewModelLocator()
        {
			if (DesignMode)
                _application = Initializer.LoadDesignModeApplication();
			else
                _application = Initializer.LoadApplication(
                    new PushNotificationSubscription());
            _selection = new SelectionModel();
        }

        public object Main
        {
            get { return ViewModel(() => new MainViewModel(_application, _selection)); }
        }
    }
}
