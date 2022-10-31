namespace Flow.Net.Sdk.Core.Models
{
    public class FlowTransactionResponse : FlowTransactionBase
    {
        public string Id { get; set; }

        public FlowTransactionResult Result { get; set; }
    }
}