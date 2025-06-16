using BusinessObjects.Cart;
using BusinessObjects.Entities.AI;
using BusinessObjects.Identity;
using BusinessObjects.Orders;
using BusinessObjects.Reviews;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Products
{
    // CÓ THỂ kế thừa BaseEntity - cần tracking đầy đủ
    public class Product : BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(12,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        public decimal? SalePrice { get; set; }

        [MaxLength(100)]
        public string? Sku { get; set; }

        [Required]
        public int Quantity { get; set; } = 0;

        public Guid? CategoryId { get; set; }

        [MaxLength(100)]
        public string? Material { get; set; }

        [MaxLength(50)]
        public string? Season { get; set; }

        // JSON arrays
        public string? AvailableColors { get; set; } // JSON: ["Red", "Blue", "White"]
        public string? AvailableSizes { get; set; } // JSON: ["S", "M", "L", "XL"]
        public string? Images { get; set; } // JSON array of image URLs

        [MaxLength(20)]
        public string Status { get; set; } = "Active";

        // Navigation properties
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual ApplicationUser? Creator { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<AiRecommendation> AiRecommendations { get; set; } = new List<AiRecommendation>();
    }
}