using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gbtwowheels.Models
{
    [Table("usr_user")] 
    public class User
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("first_name")] 
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [Column("last_name")] 
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [Column("cnpj")]
        [MaxLength(20)]
        public string CNPJ { get; set; }

        [Required]
        [Column("date_of_birth")] 
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Column("category_license")] 
        [MaxLength(2)]
        public string CategoryLicense { get; set; }

        [Required]
        [Column("license_number")] 
        public int LicenseNumber { get; set; }

        [Required]
        [Column("image_license")] 
        [MaxLength(100)]
        public string ImageLicense { get; set; }

        [Required]
        [Column("user_email")]
        [MaxLength(100)]
        public string UserEmail { get; set; }

        [Required]
        [Column("user_password")]
        [MaxLength(20)]
        public string UserPassword { get; set; }

        [Column("level_id")]
        public int? LevelId { get; set; }

        [ForeignKey("LevelId")]
        public LevelAccess LevelAccess { get; set; }

        public ICollection<Rent> Rents { get; set; }

        public ICollection<Order> OrdersCreatedByUser { get; set; }

        public ICollection<Order> OrdersDeliveredByUser { get; set; }

        


    }
}

