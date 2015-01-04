using Assisticant;
using CardBoard.BoardView;
using CardBoard.Models;
using System;
using System.Linq;
using CardBoard.Messaging;

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
            var messageQueue = new FileMessageQueue("CardBoard");
            var messagePump = new HttpMessagePump(new Uri("http://localhost/distributor"), messageQueue);
            var messageStore = new FileMessageStore("CardBoard");
            var application = new Application(messageQueue, messagePump, messageStore);
            application.Load();
            return application;
        }

        private Application LoadDesignModeApplication()
        {
            var messageQueue = new MemoryMessageQueue();
            var messagePump = new MemoryMessagePump();
            var messageStore = new MemoryMessageStore();
            var application = new Application(messageQueue, messagePump, messageStore);
            CreateCard(application, "Record the demo", Column.Doing);
            CreateCard(application, "Edit the demo", Column.ToDo);
            CreateCard(application, "Publish the course", Column.ToDo);
            return application;
        }

        private static void CreateCard(Application application, string text, Column column)
        {
            var cardId = Guid.NewGuid();
            application.EmitMessage(application.Board.CreateCard(cardId));
            var card = application.Board.Cards.Single(c => c.CardId == cardId);
            application.EmitMessage(card.SetText(text));
            application.EmitMessage(card.MoveTo(column));
        }
    }
}
