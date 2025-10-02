using System.ComponentModel.DataAnnotations;

namespace Team_06.DTOs
{
    public class ToolDto
    {
        public string Id { get; set; } = string.Empty;
        public string FriendlyName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public string? Category { get; set; }
        public string? Supplier { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public decimal? UnitCost { get; set; }
        public string? Currency { get; set; }
        public string QRData { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateToolDto
    {
        [Required]
        [StringLength(255)]
        public string FriendlyName { get; set; } = string.Empty;

        public string? Description { get; set; }

        [StringLength(100)]
        public string? Make { get; set; }

        [StringLength(100)]
        public string? Model { get; set; }

        [StringLength(100)]
        public string? Category { get; set; }

        [StringLength(100)]
        public string? Supplier { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public decimal? UnitCost { get; set; }

        [StringLength(3)]
        public string? Currency { get; set; }

        [Required]
        public string QRData { get; set; } = string.Empty;
    }

    public class UpdateToolDto
    {
        [StringLength(255)]
        public string? FriendlyName { get; set; }

        public string? Description { get; set; }

        [StringLength(100)]
        public string? Make { get; set; }

        [StringLength(100)]
        public string? Model { get; set; }

        [StringLength(100)]
        public string? Category { get; set; }

        [StringLength(100)]
        public string? Supplier { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public decimal? UnitCost { get; set; }

        [StringLength(3)]
        public string? Currency { get; set; }

        public string? QRData { get; set; }
    }
}