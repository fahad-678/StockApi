using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Stock
{
    public class CreateStockRequestDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "Symbol is required")]
        [MaxLength(10, ErrorMessage = "Symbol cannot exceed 10 characters")]
        public string Symbol { get; set; } = string.Empty;

        [Required]
        [MinLength(5, ErrorMessage = "Company cannot be less than 5 characters")]
        [MaxLength(20, ErrorMessage = "Company cannot exceed 20 characters")]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [Range(1, 1000000000)]
        public decimal Purchase { get; set; }

        [Required]
        [Range(0.001, 100)]
        public decimal LastDiv { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Industry cannot be less than 5 characters")]
        [MaxLength(10,ErrorMessage = "Industry Code cannot exceed 10 characters")]
        public string IndustryCode { get; set; } = string.Empty;

        [Required]
        [Range(1, 5000000000)]
        public long MarketCap { get; set; }
    }
}