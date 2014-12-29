using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardBoard.Messages
{
    public class Candidate<T>
    {
        private readonly MessageHash _messageHash;
        private readonly T _value;

        public Candidate(MessageHash messageHash, T value)
        {
            _messageHash = messageHash;
            _value = value;
        }

        public MessageHash MessageHash
        {
            get { return _messageHash; }
        }

        public T Value
        {
            get { return _value; }
        }
    }
}
