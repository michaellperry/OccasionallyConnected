﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardBoard.Messaging
{
    public interface IMessageStore
    {
        Task<ImmutableList<Message>> LoadAsync(Guid objectId);
        void Save(Message message);
        Exception Exception { get; }
    }
}
