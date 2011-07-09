namespace iGoat.Domain.Entities
{
    public class DeliveryItem
    {
        public virtual int Id { get; set; }

        public virtual DeliveryItemStatus Status { get; set; }
        
        public virtual DeliveryItemType ItemType { get; set; }
    }
}