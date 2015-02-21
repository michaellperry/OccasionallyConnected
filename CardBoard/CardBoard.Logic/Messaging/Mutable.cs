using Assisticant.Collections;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace CardBoard.Messaging
{
    public class Mutable<T>
    {
        private readonly string _topic;

        private HashSet<MessageHash> _predecessors = new HashSet<MessageHash>();
        private ObservableList<Candidate<T>> _candidates = new ObservableList<Candidate<T>>();
        
        public Mutable(string topic)
        {
            _topic = topic;
        }
        
        public IEnumerable<Candidate<T>> Candidates
        {
            get { return _candidates; }
        }

        public Message CreateMessage(
            string messageType,
            Guid objectId,
            T value)
        {
            return Message.CreateMessage(
                _topic,
                messageType,
                _candidates.Select(t => t.MessageHash),
                objectId,
                new
                {
                    Value = value
                });
        }

        public void HandleMessage(Message message)
        {
            var messageHash = message.Hash;
            T value = (T)message.Body.Value;
            var predecessors = message.Predecessors;

            if (!_predecessors.Contains(messageHash))
            {
                _candidates.Add(new Candidate<T>(messageHash, value));
            }

            var newPredecessors = predecessors.Except(_predecessors);
            _candidates.RemoveAll(c => newPredecessors.Contains(c.MessageHash));

            foreach (var predecessor in newPredecessors)
                _predecessors.Add(predecessor);
        }

        public void HandleAllMessages(IEnumerable<Message> messages)
        {
            var predecessors = messages
                .SelectMany(m => m.Predecessors)
                .ToLookup(h => h);
            var candidates = messages
                .Where(m => !predecessors.Contains(m.Hash))
                .Select(m => new Candidate<T>(m.Hash, (T)m.Body.Value));

            _candidates.RemoveAll(c => predecessors.Contains(c.MessageHash));
            foreach (var pair in predecessors)
                _predecessors.Add(pair.Key);
            _candidates.AddRange(candidates);
        }
    }
}
