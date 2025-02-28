using System.ComponentModel.DataAnnotations;

namespace Mission08_Team0215_v2.Models
{
    // Create the Category table to match the database
    public class Category
    {
        [Key]
        [Required]
        public int CategoryId { get; set; }
        public required string CategoryName { get; set; }
    }

}
