namespace SampleMessageGenerator.message
{
    public interface IMessage
    {
        string Sender { get; set; }
        int Value { get; set; }
    }
}
