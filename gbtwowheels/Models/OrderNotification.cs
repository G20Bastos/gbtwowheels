using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gbtwowheels.Models
{
    [Table("orn_order_notification")] 
    public class OrderNotification
    {

        [Key]
        [Column("order_notification_id")]
        public int OrderNotificationId { get; set; }

        [Column("order_id")]
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        [InverseProperty("OrderNotifications")]
        public Order? Order { get; set; }


        [Column("user_id")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("OrderNotifications")]
        public User? User { get; set; }

        [Column("message")]
        public string? Message { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }

}

