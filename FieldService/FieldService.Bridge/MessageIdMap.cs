using RoverMob.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldService.Bridge
{
    public class MessageIdMap
    {
        private List<ObjectIdRecord> _objectIdRecords = new List<ObjectIdRecord>();
        private List<MessageHashRecord> _messageHashRecords = new List<MessageHashRecord>();

        public Task<Guid> GetOrCreateObjectId(string typeName, int id)
        {
            var objectIdRecord = _objectIdRecords.FirstOrDefault(r =>
                r.TypeName == typeName &&
                r.Id == id);

            if (objectIdRecord != null)
                return Task.FromResult(objectIdRecord.ObjectId);

            var objectId = Guid.NewGuid();
            _objectIdRecords.Add(new ObjectIdRecord
            {
                TypeName = typeName,
                Id = id,
                ObjectId = objectId
            });
            return Task.FromResult(objectId);
        }

        public Task SaveMessageHash(string typeName, string propertyName, int id, string value, MessageHash messageHash)
        {
            _messageHashRecords.Add(new MessageHashRecord
            {
                TypeName = typeName,
                PropertyName = propertyName,
                Id = id,
                Value = value,
                MessageHash = messageHash
            });
            return Task.FromResult(0);
        }

        public Task<MessageHash> GetMessageHash(string typeName, string propertyName, int id, string value)
        {
            var record = _messageHashRecords.FirstOrDefault(r =>
                r.TypeName == typeName &&
                r.PropertyName == propertyName &&
                r.Id == id &&
                r.Value == value);

            if (record != null)
                return Task.FromResult(record.MessageHash);

            throw new ApplicationException();
        }
    }
}
