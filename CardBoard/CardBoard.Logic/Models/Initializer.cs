using CardBoard.Messaging;
using System;
using System.Linq;

namespace CardBoard.Models
{
    public class Initializer
    {
        public static Application LoadApplication()
        {
            string folderName = "CardBoard";
            var store = new FileMessageStore(folderName);
            var queue = new FileMessageQueue(folderName);
            var bookmarkStore = new FileBookmarkStore(folderName);
            var pump = new HttpMessagePump(
                new Uri("http://localhost:15871/api/distributor/", UriKind.Absolute),
                queue,
                bookmarkStore);
            var application = new Application(store, queue, pump);
            application.Load();
            return application;
        }

        public static Application LoadDesignModeApplication()
        {
            var application = new Application();
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
