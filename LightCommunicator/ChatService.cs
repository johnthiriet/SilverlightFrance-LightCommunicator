using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LightCommunicator.Silverlight.ChatServer;

namespace LightCommunicator.Silverlight
{
    public class ChatService
    {
        private static readonly ChatService _instance = new ChatService();

        private ChatServiceClient _client;
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
                        _client = new ChatServiceClient("PollingHttpBinding_IChatServer");
                        _client.PushMessageReceived += (s, a) => observer.OnNext(a.message);
                        _client.ConnectCompleted += (s, a) => _token = a.Result;
                        _client.ChannelFactory.Closed += (s, a) => observer.OnCompleted();
                        _client.ChannelFactory.Faulted += (s, a) => observer.OnCompleted();

                        if (_login != null)
                            _client.ConnectAsync(_login);
                    }
                }
                catch (Exception e)
                {
                    observer.OnError(e);
                }

                return () =>
                {
                    _client.CloseAsync();
                    _client = null;
                };
            });
        }

        public void Connect(string login)
        {
            _login = login;
            if (_client == null)
                return;

            _client.ConnectAsync(login);
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
            _client.SendAsync(_token, message);
        }
    }
}
