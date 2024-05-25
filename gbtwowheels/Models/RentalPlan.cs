using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gbtwowheels.Models
{
    [Table("rpl_rental_plan")] 
    public class RentalPlan
    {
        [Key]
        [Column("rental_plan_id")] 
        public int RentalPlanId { get; set; }

        [Required]
        [Column("number_rental_days")]
        public int NumberRentalDays { get; set; }

        [Required]
        [Column("value_per_rental_day")]
        public decimal ValuePerRentalDay { get; set; }

        [Required]
        [Column("percentage_of_fine_lower_date")] 
        public int PercentageOfFineLowerDate { get; set; }

        [Required]
        [Column("additional_value_later_date")]
        public decimal AdditionalValueLaterDate { get; set; }

        public ICollection<Rent>? Rents { get; set; }

    }
}

