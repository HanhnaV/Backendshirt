using BusinessObjects.Cart;
using BusinessObjects.Common;
using BusinessObjects.CustomDesigns;
using BusinessObjects.Entities.AI;
using BusinessObjects.Entities.Orders;
using BusinessObjects.Products;
using BusinessObjects.Reviews;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [NotMapped]
        public string FullName => $"{LastName} {FirstName}".Trim();

        public string Address { get; set; } = string.Empty;

        public Gender Gender { get; set; }
        public bool IsFirstLogin { get; set; } = true;

        // Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation properties
        public virtual ICollection<Product> CreatedProducts { get; set; } = new List<Product>();
        public virtual ICollection<CustomDesign> CustomDesigns { get; set; } = new List<CustomDesign>();
        public virtual ICollection<CustomDesign> StaffDesigns { get; set; } = new List<CustomDesign>();
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<Order> AssignedOrders { get; set; } = new List<Order>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<AiRecommendation> AiRecommendations { get; set; } = new List<AiRecommendation>();
    }
}