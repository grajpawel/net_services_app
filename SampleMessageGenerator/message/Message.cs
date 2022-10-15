namespace SampleMessageGenerator.message
{
    public class Message : IMessage
    {
        public string Sender { get; set; }
        public int Value { get; set; }

        public Message(string sender, int value)
        {
            Sender = sender;
            Value = value;
        }
    }
}
