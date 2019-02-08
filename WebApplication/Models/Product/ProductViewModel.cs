using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class ProductViewModel
    {
        public int ID { set; get; }

        [Required]
        public string Name { set; get; }
        [Required]
        public string Alias { set; get; }
        [Required]
        public int CategoryID { set; get; }
        [Required]
        public decimal Price { set; get; }

        public string CategoryName { set; get; }

        public string Description { set; get; }

        public DateTime? CreatedDate { set; get; }

        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        public string UpdatedBy { set; get; }

        public bool Status { set; get; }

        public virtual ProductCategoryViewModel ProductCategory { set; get; }
    }
}