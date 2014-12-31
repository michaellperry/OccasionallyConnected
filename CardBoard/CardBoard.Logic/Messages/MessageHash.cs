using System.Linq;

namespace CardBoard.Messages
{
    public class MessageHash
    {
        private byte[] _value;

        public MessageHash(byte[] value)
        {
            _value = value;
        }

        public byte[] Value
        {
            get { return _value; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var that = (MessageHash)obj;
            return Enumerable.SequenceEqual(this._value, that._value);
        }

        public override int GetHashCode()
        {
            int hash = 0;
            foreach (var b in _value)
                hash = hash * 37 + b;

            return hash;
        }
    }
}
