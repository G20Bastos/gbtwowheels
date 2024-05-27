using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gbtwowheels.Models
{
    [Table("ren_rent")] 
    public class Rent
    {
        [Key]
        [Column("rent_id")] 
        public int RentId { get; set; }

        [Required]
        [Column("user_delivery_id")] 
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Rents")]
        public User? User { get; set; }

        [Required]
        [ForeignKey("Motorcycle")] 
        [Column("motorcycle_id")] 
        public int MotorcycleId { get; set; }
        public Motorcycle? Motorcycle { get; set; }

        [Required]
        [ForeignKey("RentalPlan")] 
        [Column("rental_plan_id")] 
        public int RentalPlanId { get; set; }
        public RentalPlan? RentalPlan { get; set; }

        [Column("start_rent_date")] 
        public DateTime? StartRentDate { get; set; }

        [Column("expected_end_rent_date")] 
        public DateTime? ExpectedEndRentDate { get; set; }

        [Column("end_rent_date")]
        public DateTime? EndRentDate { get; set; }

        [Column("cost_all_days")]
        public decimal CostAllDays { get; set; }

        [Column("cost_by_day_not_used")]
        public decimal CostByDayNotUsed { get; set; }

        [Column("cost_by_aditional_day")]
        public decimal CostByAditionalDay { get; set; }

        [Column("total_cost")]
        public decimal TotalCost { get; set; }
        
    }
}

