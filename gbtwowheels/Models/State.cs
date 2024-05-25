using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gbtwowheels.Models
{
    [Table("sta_state")]
    public class State
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("state_id")]
        public int StateId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("state_name")]
        public string? StateName { get; set; }

        [ForeignKey("Country")]
        [Column("country_id")]
        public int? CountryId { get; set; }

        public Country? Country { get; set; }

        public ICollection<City>? Cities { get; set; }
    }
}

