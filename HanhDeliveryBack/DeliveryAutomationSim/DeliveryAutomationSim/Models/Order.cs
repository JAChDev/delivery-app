namespace DeliveryAutomationSim.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int OriginNodeId { get; set; }
        public int targetNodeId { get; set; }
        public int load { get; set; }
        public double value { get; set; }
        public DateTime deliveryDateUtc { get; set; }
        public DateTime expirationDateUtc { get; set; }

    }
}
