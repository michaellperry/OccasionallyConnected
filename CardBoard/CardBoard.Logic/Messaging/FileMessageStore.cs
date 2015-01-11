using CardBoard.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;

namespace CardBoard.Messaging
{
    public class FileMessageStore : Process
    {
        private static readonly Regex Punctuation = new Regex(@"[{}-]");

        private readonly string _folderName;

        private JsonSerializer _serializer = new JsonSerializer();

        public FileMessageStore(string folderName)
        {
            _folderName = folderName;
        }

        private async Task<StorageFile> CreateFileAsync(Guid objectId)
        {
            var cardBoardFolder = await ApplicationData.Current.LocalFolder
                .CreateFolderAsync(_folderName, CreationCollisionOption.OpenIfExists);
            string fileName = String.Format("obj_{0}.json",
                Punctuation.Replace(objectId.ToString(), "").ToLower());
            var objectFile = await cardBoardFolder
                .CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
            return objectFile;
        }

        private async Task<List<MessageMemento>> ReadMessagesAsync(StorageFile objectFile)
        {
            List<MessageMemento> messageList;
            var inputStream = await objectFile.OpenStreamForReadAsync();
            using (JsonReader reader = new JsonTextReader(new StreamReader(inputStream)))
            {
                messageList = _serializer.Deserialize<List<MessageMemento>>(reader);
            }

            if (messageList == null)
                messageList = new List<MessageMemento>();
            return messageList;
        }

        private async Task WriteMessagesAsync(StorageFile objectFile, List<MessageMemento> messageList)
        {
            var outputStream = await objectFile.OpenStreamForWriteAsync();
            using (JsonWriter writer = new JsonTextWriter(new StreamWriter(outputStream)))
            {
                _serializer.Serialize(writer, messageList);
            }
        }
    }
}
