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

        private Mutable<string> _address;

        public Home(Guid homeId)
        {
            _homeId = homeId;
            _address = new Mutable<string>(String.Empty);
        }

        public IEnumerable<Candidate<string>> Address
        {
            get { return _address.Candidates; }
        }

        public IEnumerable<IMessageHandler> Children
        {
            get { yield break; }
        }

        public Guid GetObjectId()
        {
            return _homeId;
        }

        public void HandleAllMessages(IEnumerable<Message> messages)
        {
            _address.HandleAllMessages(messages
                .Where(m => m.Type == "HomeAddress"));
        }

        public void HandleMessage(Message message)
        {
            if (message.Type == "HomeAddress")
                _address.HandleMessage(message);
        }
    }
}
