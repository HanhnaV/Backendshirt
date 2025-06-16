using BusinessObjects.Cart;
using BusinessObjects.Identity;
using BusinessObjects.Orders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.CustomDesigns
{
    public class CustomDesign : BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string DesignName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string ShirtType { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string BaseColor { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Size { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? DesignImageUrl { get; set; }

        [MaxLength(255)]
        public string? LogoText { get; set; }

        [MaxLength(50)]
        public string? LogoPosition { get; set; }

        public string? SpecialRequirements { get; set; }

        [Required]
        [Column(TypeName = "decimal(12,2)")]
        public decimal TotalPrice { get; set; }

        [MaxLength(30)]
        public string Status { get; set; } = "Draft"; // Draft, Submitted, Approved, Rejected, In Production, Completed

        public Guid? StaffId { get; set; }
        public string? StaffNotes { get; set; }

        // Navigation properties
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual ApplicationUser? Staff { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}