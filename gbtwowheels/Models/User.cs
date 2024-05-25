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

        
        [Column("first_name")] 
        [MaxLength(100)]
        public string? FirstName { get; set; }

        
        [Column("last_name")] 
        [MaxLength(100)]
        public string? LastName { get; set; }

        
        [Column("cnpj")]
        [MaxLength(20)]
        public string? CNPJ { get; set; }

        
        [Column("date_of_birth")] 
        public DateTime? DateOfBirth { get; set; }

        
        [Column("category_license")] 
        [MaxLength(2)]
        public string? CategoryLicense { get; set; }

        
        [Column("license_number")] 
        public int LicenseNumber { get; set; }

        
        [Column("image_license")] 
        [MaxLength(100)]
        public string? ImageLicense { get; set; }

        
        [Column("user_email")]
        [MaxLength(100)]
        public string? UserEmail { get; set; }

        
        [Column("user_password")]
        [MaxLength(20)]
        public string? UserPassword { get; set; }

        [Column("level_id")]
        public int? LevelId { get; set; }

        [ForeignKey("LevelId")]
        public LevelAccess? LevelAccess { get; set; }

        public ICollection<Rent>? Rents { get; set; }

        [InverseProperty("UserOrderCreation")]
        public ICollection<Order>? OrdersCreatedByUser { get; set; }

        [InverseProperty("UserDelivery")]
        public ICollection<Order>? OrdersDeliveredByUser { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        [NotMapped]
        public string? TokenAccess { get; set; }


    }
}

