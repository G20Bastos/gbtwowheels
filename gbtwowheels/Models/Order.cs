using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gbtwowheels.Models
{
    [Table("ord_order")] 
    public class Order
    {
        [Key]
        [Column("order_id")]
        public int OrderId { get; set; }

        [Required]
        [Column("user_order_creation_id")]
        public int UserOrderCreationId { get; set; }

        [Required]
        [Column("order_creation_date")]
        public DateTime OrderCreationDate { get; set; }

        [Required]
        [Column("user_delivery_id")]
        public int UserDeliveryId { get; set; }

        [Column("address_order")]
        public string? AddressOrder { get; set; }

        [Required]
        [Column("order_service_value", TypeName = "numeric(10,2)")]
        public decimal OrderServiceValue { get; set; }

        [Required]
        [Column("status_order_id")]
        public int StatusOrderId { get; set; }

        [Required]
        [Column("order_finish_date")]
        public DateTime OrderFinishDate { get; set; }

        [NotMapped]
        [ForeignKey("UserOrderCreationId")]
        public User? UserOrderCreation { get; set; }

        [NotMapped]
        [ForeignKey("UserDeliveryId")]
        public User? UserDelivery { get; set; }

        [ForeignKey("StatusOrderId")]
        public StatusOrder? StatusOrder { get; set; }
    }
}

