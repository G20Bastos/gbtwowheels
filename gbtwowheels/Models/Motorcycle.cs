using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gbtwowheels.Models
{
    [Table("mot_motorcycle")] 
    public class Motorcycle
    {
        [Key]
        [Column("motorcycle_id")] 
        public int MotorcycleId { get; set; }

        [Required]
        [Column("year")] 
        public int Year { get; set; }

        [Required]
        [Column("model")] 
        [MaxLength(100)] 
        public string Model { get; set; }

        [Required]
        [Column("license_plate")] 
        [MaxLength(20)] 
        public string LicensePlate { get; set; }

        [Column("color")] 
        [MaxLength(50)] 
        public string Color { get; set; }

        [Column("engine_capacity")] 
        public int? EngineCapacity { get; set; } 

        [Required]
        [Column("is_available")] 
        public bool IsAvailable { get; set; }

        public ICollection<Rent> Rents { get; set; }

    }
}

