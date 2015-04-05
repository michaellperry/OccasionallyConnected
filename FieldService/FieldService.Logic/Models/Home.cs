using RoverMob.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FieldService.Models
{
    public class Home : IMessageHandler
    {
        private readonly Guid _guid;

        private Mutable<string> _address = new Mutable<string>(string.Empty);

        public Home(Guid guid)
        {
            _guid = guid;            
        }

        public IEnumerable<Candidate<string>> Address
        {
            get { return _address.Candidates; }
        }

        public IEnumerable<IMessageHandler> Children
        {
            get { throw new NotImplementedException(); }
        }

        public Guid GetObjectId()
        {
            throw new NotImplementedException();
        }

        public void HandleAllMessages(IEnumerable<Message> messages)
        {
            throw new NotImplementedException();
        }

        public void HandleMessage(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
