using System.ServiceModel;
using System;
using System.ServiceModel.Activation;

namespace LightCommunicator.Services
{
    [ServiceContract(CallbackContract = typeof(IChatServiceCallback))]
    public interface IChatService
    {
        [OperationContract()]
        Guid Connect(string login);

        [OperationContract(IsOneWay = true)]
        void Send(Guid token, string message);

        [OperationContract(IsOneWay = true)]
        void Disconnect(Guid token);
    }
}
