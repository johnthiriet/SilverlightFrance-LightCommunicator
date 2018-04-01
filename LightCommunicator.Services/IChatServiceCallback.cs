using System.ServiceModel;

namespace LightCommunicator.Services
{
    public interface IChatServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void PushMessage(ChatMessage message);
    }
}
