using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Threading;
using DotNetSoul.Services;

namespace LightCommunicator.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ChatService : IChatService
    {
        private static readonly List<ChatClient> _connectedClients;
        private static readonly object _locker = new object();

        static ChatService()
        {
            _connectedClients = new List<ChatClient>();
        }

        public Guid Connect(string login)
        {
            Guid clientId = Guid.NewGuid();

            lock (_locker)
            {
                _connectedClients.Add(new ChatClient(clientId, login, OperationContext.Current));

                IChatServiceCallback callback = OperationContext.Current.GetCallbackChannel<IChatServiceCallback>();
                callback.PushMessage(new ChatMessage { From = "Server", Message = "Bienvenue " + login });
            }

            return clientId;
        }

        public void Send(Guid token, string message)
        {
            ChatClient from = _connectedClients.FirstOrDefault(it => it.Id == token);

            if (from == null)
                return;

            Stack<ChatClient> toRemove = new Stack<ChatClient>();

            foreach (var client in _connectedClients/*.Where(c => c.Id != token)*/)
            {
                if (client.Context.Channel.State == CommunicationState.Opened)
                {
                    ChatClient c = client; // Closure !
                    WaitCallback wc = state =>
                        c.Context
                        .GetCallbackChannel<IChatServiceCallback>()
                        .PushMessage(new ChatMessage { From = from.Login, Message = message });

                    ThreadPool.QueueUserWorkItem(wc, client);
                }
                else
                {
                    toRemove.Push(client);
                }
            }

            if (toRemove.Count > 0)
            {
                lock (_locker)
                {
                    while (toRemove.Count > 0)
                    {
                        var client = toRemove.Pop();
                        _connectedClients.Remove(client);
                    }
                }
            }
        }

        public void Disconnect(Guid token)
        {
            var client = _connectedClients.FirstOrDefault(c => c.Id == token);
            if (client == null)
                return;

            lock (_locker)
            {
                _connectedClients.Remove(client);
            }
        }
    }
}
