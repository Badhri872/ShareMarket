namespace Services.AngelWebSocket
{
    public class MessageEventArgs : EventArgs
    {
        public MessageEventArgs(StrikeLTPData message) 
        {
            Message = message;
        }
        public StrikeLTPData Message { get; }
    }
}
