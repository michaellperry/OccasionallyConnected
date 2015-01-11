
namespace CardBoard.Messaging
{
    public interface IMessageStore
    {
        void Save(Message message);
    }
}
