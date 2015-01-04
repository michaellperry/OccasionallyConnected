using CardBoard.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;
using Newtonsoft.Json;
using System.Dynamic;
using Newtonsoft.Json.Linq;

namespace CardBoard.Messaging
{
    public class FileMessageQueue : Process, IMessageQueue
    {
        private readonly string _folderName;

        public FileMessageQueue(string folderName)
        {
            _folderName = folderName;
        }

        public void Enqueue(Message message)
        {
            Perform(() => EnqueueAsync(message));
        }

        private async Task EnqueueAsync(Message message)
        {
            var cardBoardFolder = await ApplicationData.Current.LocalFolder
                .CreateFolderAsync(_folderName, CreationCollisionOption.OpenIfExists);
            var messageQueueFile = await cardBoardFolder
                .CreateFileAsync("MessageQueue.json", CreationCollisionOption.OpenIfExists);

            JsonSerializer serializer = new JsonSerializer();

            List<ExpandoObject> messageList;
            var inputStream = await messageQueueFile.OpenStreamForReadAsync();
            using (JsonReader reader = new JsonTextReader(new StreamReader(inputStream)))
            {
                messageList = serializer.Deserialize<List<ExpandoObject>>(reader);
            }

            if (messageList == null)
                messageList = new List<ExpandoObject>();

            dynamic memento = new ExpandoObject();
            memento.Hash = message.Hash.ToString();
            memento.MessageType = message.Type;
            memento.Predecessors = message.Predecessors
                .Select(p => p.ToString())
                .ToList();
            memento.ObjectId = message.ObjectId;
            memento.Body = message.Body;
            messageList.Add(memento);

            var outputStream = await messageQueueFile.OpenStreamForWriteAsync();
            using (JsonWriter writer = new JsonTextWriter(new StreamWriter(outputStream)))
            {
                serializer.Serialize(writer, messageList);
            }
        }
    }
}
