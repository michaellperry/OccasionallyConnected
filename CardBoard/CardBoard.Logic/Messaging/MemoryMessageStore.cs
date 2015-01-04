using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardBoard.Messaging
{
    public class MemoryMessageStore : IMessageStore
    {
        public Task<ImmutableList<Message>> LoadAsync(Guid objectId)
        {
            return Task.FromResult(ImmutableList<Message>.Empty);
        }

        public void Save(Message message)
        {
        }

        public Exception Exception
        {
            get { return null; }
        }
    }
}
