using Assisticant.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardBoard.Messages
{
    public class Mutable<T>
    {
        private ObservableList<MessageHash> _predecessors = new ObservableList<MessageHash>();
        private ObservableList<Candidate<T>> _candidates = new ObservableList<Candidate<T>>();

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
                messageType,
                _candidates.Select(t => t.MessageHash),
                objectId,
                new { Value = value });
        }

        public void HandleMessage(Message message)
        {
            var messageHash = message.Hash;
            T value = message.Body.Value;
            var predecessors = message.Predecessors;

            if (!_predecessors.Contains(messageHash))
            {
                _candidates.Add(new Candidate<T>(messageHash, value));
            }

            var newPredecessors = predecessors.Except(_predecessors);
            _candidates.RemoveAll(c => newPredecessors.Contains(c.MessageHash));

            _predecessors.AddRange(newPredecessors);
        }
    }
}
