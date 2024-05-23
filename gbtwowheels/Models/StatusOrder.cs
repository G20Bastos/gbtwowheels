using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gbtwowheels.Models
{
    [Table("status_order")] 
    public class StatusOrder
    {
        [Key]
        [Column("status_order_id")] 
        public int StatusOrderId { get; set; }

        [Required]
        [Column("status")] 
        [MaxLength(20)] 
        public string Status { get; set; }
    }
}

