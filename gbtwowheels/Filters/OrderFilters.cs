using System;
using gbtwowheels.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace gbtwowheels.Filters
{
	public class OrderFilters
	{
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? AddressOrder { get; set; }
        public decimal? OrderServiceValue { get; set; }
        public int? StatusOrderId { get; set; }
    }
}

