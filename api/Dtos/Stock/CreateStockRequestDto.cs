using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Stock
{
    public class CreateStockRequestDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "Symbol must be at least 1 character long")]
        [MaxLength(10, ErrorMessage = "Symbol must be at most 10 characters long")]
        public string Symbol { get; set; } = string.Empty;

        [Required]
        [MinLength(5, ErrorMessage = "Company name must be at least 5 characters long")]
        [MaxLength(100, ErrorMessage = "Company name must be at most 100 characters long")]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Purchase must be greater than 0")]
        public decimal Purchase { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Current must be greater than 0")]
        public decimal LastDiv { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Industry must be at least 5 characters long")]
        [MaxLength(100, ErrorMessage = "Industry must be at most 100 characters long")]
        public string Industry { get; set; } = string.Empty;

        [Required]
        [Range(0, long.MaxValue, ErrorMessage = "Market cap must be greater than 0")]
        public long MarketCap { get; set; }
    }
}
