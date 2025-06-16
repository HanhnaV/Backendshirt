using BusinessObjects.Entities.Orders;
using BusinessObjects.Identity;
using BusinessObjects.Products;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Reviews
{
    // CÓ THỂ kế thừa BaseEntity - cần tracking để quản lý
    public class Review : BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }

        public Guid? ProductId { get; set; }
        public Guid? OrderId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; } // 1-5 stars

        [Required]
        public string Content { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Status { get; set; } = "Approved"; // Approved, Hidden

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }
    }
}