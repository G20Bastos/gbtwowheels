using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gbtwowheels.Models
{
    [Table("lev_level_access")]
    public class LevelAccess
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("level_id")]
        public int LevelId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("level_name")]
        public string? LevelName { get; set; }
    }
}

