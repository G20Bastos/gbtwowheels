using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gbtwowheels.Models
{
    [Table("cit_city")]
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("city_id")]
        public int CityId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("city_name")]
        public string? CityName { get; set; }

        [ForeignKey("State")]
        [Column("state_id")]
        public int? StateId { get; set; }

        [ForeignKey("Country")]
        [Column("country_id")]
        public int? CountryId { get; set; }

        public State? State { get; set; }
        public Country? Country { get; set; }
    }
}

