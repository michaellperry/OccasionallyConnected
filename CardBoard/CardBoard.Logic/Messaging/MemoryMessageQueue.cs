﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardBoard.Messaging
{
    public class MemoryMessageQueue : IMessageQueue
    {
        public Task<ImmutableList<Message>> LoadAsync()
        {
            return Task.FromResult(ImmutableList<Message>.Empty);
        }

        public void Confirm(Message message)
        {
        }

        public void Enqueue(Message message)
        {
        }

        public Exception Exception
        {
            get { return null; }
        }
    }
}
