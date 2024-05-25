using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gbtwowheels.Models
{
    [Table("cou_country")]
    public class Country
	{

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("country_id")]
        public int CountryId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("country_name")]
        public string? CountryName { get; set; }

        public ICollection<State>? States { get; set; }
        public ICollection<City>? Cities { get; set; }
    }
}

