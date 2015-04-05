using RoverMob.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FieldService.Models
{
    public class Home : IMessageHandler
    {
        private readonly Guid _homeId;

        private Mutable<string> _address = new Mutable<string>(string.Empty);

        public Home(Guid homeId)
        {
            _homeId = homeId;            
        }

        public IEnumerable<Candidate<string>> Address
        {
            get { return _address.Candidates; }
        }

        public IEnumerable<IMessageHandler> Children
        {
            get { return Enumerable.Empty<IMessageHandler>(); }
        }

        public Guid GetObjectId()
        {
            return _homeId;
        }

        public void HandleAllMessages(IEnumerable<Message> messages)
        {
        }

        public void HandleMessage(Message message)
        {
        }
    }
}
