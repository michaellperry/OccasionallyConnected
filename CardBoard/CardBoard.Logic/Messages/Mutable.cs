using System.Collections.Generic;
using System.Linq;

namespace CardBoard.Messages
{
    public class Mutable<T>
    {
        private List<MessageHash> _predecessors = new List<MessageHash>();
        private List<Candidate<T>> _candidates = new List<Candidate<T>>();

        public IEnumerable<Candidate<T>> Candidates
        {
            get { return _candidates; }
        }

        public void HandleMessage(Message message)
        {
            var messageHash = message.Hash;
            var value = message.Body.Value<T>("Value");
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
