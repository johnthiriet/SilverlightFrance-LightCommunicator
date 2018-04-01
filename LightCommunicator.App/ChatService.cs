using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.ServiceModel;
using System.Text;
using LightCommunicator.App.ChatServer;

namespace LightCommunicator.App
{
    public sealed class ChatService
    {
        private static readonly ChatService _instance = new ChatService();

        private ChatServiceClient _client;
        private ChatServiceClientCallback _clientCallback;
        private IObservable<ChatMessage> _messages;
        private Guid _token;
        private Queue<string> _previousMessages = new Queue<string>(5);
        private string _login;

        public static ChatService Instance
        {
            get
            {
                return _instance;
            }
        }

        private ChatService()
        {
            _messages = Observable.Create<ChatMessage>(observer =>
            {
                try
                {
                    if (_client == null)
                    {
                        _clientCallback = new ChatServiceClientCallback(observer);
                        _client = new ChatServiceClient(new InstanceContext(_clientCallback));
                        _client.ChannelFactory.Closed += (s, a) => { observer.OnCompleted(); };
                        _client.ChannelFactory.Faulted += (s, a) => { observer.OnCompleted(); };

                        if (_login != null)
                            _token = _client.Connect(_login);
                    }
                }
                catch (Exception e)
                {
                    observer.OnError(e);
                }

                return () =>
                {
                    _client.Close();
                    _client = null;
                };
            });
        }

        public void Connect(string login)
        {
            _login = login;
            if (_client == null)
                return;

            _token = _client.Connect(login);
        }

        public IObservable<ChatMessage> Messages
        {
            get
            {
                return _messages;
            }
        }

        public void SendMessage(string message)
        {
            _client.Send(_token, message);
        }

        #region Private Classes
        private class ChatServiceClientCallback : IChatServiceCallback
        {
            private readonly IObserver<ChatMessage> _observer;

            public ChatServiceClientCallback(IObserver<ChatMessage> observer)
            {
                _observer = observer;
            }

            public void PushMessage(ChatMessage message)
            {
                _observer.OnNext(message);
            }
        }

        #endregion
    }
}
