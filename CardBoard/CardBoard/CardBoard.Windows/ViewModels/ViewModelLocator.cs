using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assisticant;
using CardBoard.Models;
using CardBoard.BoardView;

namespace CardBoard.ViewModels
{
    public class ViewModelLocator : ViewModelLocatorBase
    {
        private Application _application;
        private SelectionModel _selection;

        public ViewModelLocator()
        {
			if (DesignMode)
                _application = LoadDesignModeApplication();
			else
                _application = LoadApplication();
            _selection = new SelectionModel();
        }

        public object Main
        {
            get { return ViewModel(() => new MainViewModel(_application, _selection)); }
        }

        private Application LoadApplication()
		{
            Application application = new Application();
            return application;
		}

        private Application LoadDesignModeApplication()
		{
            Application application = new Application();
            CreateCard(application, "Record the demo", Column.Doing);
            CreateCard(application, "Edit the demo", Column.ToDo);
            CreateCard(application, "Publish the course", Column.ToDo);
            return application;
        }

        private static void CreateCard(Application application, string text, Column column)
        {
            Card card = application.Board.NewCard();
            card.SetText(text);
            card.MoveTo(column);
        }
    }
}
