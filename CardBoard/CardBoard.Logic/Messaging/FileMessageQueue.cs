using CardBoard.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace CardBoard.Messaging
{
    public class FileMessageQueue : Process, IMessageQueue
    {
        private readonly string _folderName;

        private StorageFile _messageQueueFile;
        private JsonSerializer _serializer = new JsonSerializer();

        public FileMessageQueue(string folderName)
        {
            _folderName = folderName;
        }

        public void Enqueue(Message message)
        {
            Perform(() => EnqueueAsync(message));
        }

        public void Confirm(Message message)
        {
            Perform(() => ConfirmAsync(message));
        }

        private async Task EnqueueAsync(Message message)
        {
            await CreateFileAsync();
            var messageList = await ReadMessageList();

            dynamic memento = message.GetMemento();
            messageList.Add(memento);

            await WriteMessageList(messageList);
        }

        private async Task ConfirmAsync(Message message)
        {
            await CreateFileAsync();
            var messageList = await ReadMessageList();

            string hash = message.Hash.ToString();
            messageList.RemoveAll(o => ((dynamic)o).Hash == hash);

            await WriteMessageList(messageList);
        }

        private async Task CreateFileAsync()
        {
            if (_messageQueueFile == null)
            {
                var cardBoardFolder = await ApplicationData.Current.LocalFolder
                    .CreateFolderAsync(_folderName, CreationCollisionOption.OpenIfExists);
                _messageQueueFile = await cardBoardFolder
                    .CreateFileAsync("MessageQueue.json", CreationCollisionOption.OpenIfExists);
            }
        }

        private async Task<List<ExpandoObject>> ReadMessageList()
        {
            List<ExpandoObject> messageList;
            var inputStream = await _messageQueueFile.OpenStreamForReadAsync();
            using (JsonReader reader = new JsonTextReader(new StreamReader(inputStream)))
            {
                messageList = _serializer.Deserialize<List<ExpandoObject>>(reader);
            }

            if (messageList == null)
                messageList = new List<ExpandoObject>();
            return messageList;
        }

        private async Task WriteMessageList(List<ExpandoObject> messageList)
        {
            var outputStream = await _messageQueueFile.OpenStreamForWriteAsync();
            using (JsonWriter writer = new JsonTextWriter(new StreamWriter(outputStream)))
            {
                _serializer.Serialize(writer, messageList);
            }
        }
    }
}
