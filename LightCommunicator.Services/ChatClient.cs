using System;
using System.ServiceModel;

namespace DotNetSoul.Services
{
    public class ChatClient
    {
        public ChatClient(Guid id, string login, OperationContext context)
        {
            Id = id;
            Login = login;
            Context = context;
        }

        public Guid Id { get; private set; }
        public string Login { get; private set; }
        public OperationContext Context { get; private set; }
    }
}