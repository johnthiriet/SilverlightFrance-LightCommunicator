using System.Runtime.Serialization;

namespace LightCommunicator.Services
{
    [DataContract]
    public class ChatMessage
    {
        [DataMember]
        public string From { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}