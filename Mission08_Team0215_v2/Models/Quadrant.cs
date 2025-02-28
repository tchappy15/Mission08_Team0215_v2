using System.ComponentModel.DataAnnotations;

namespace Mission08_Team0215_v2.Models
{
    public class Quadrant
    {
        [Key]
        [Required]
        public int TaskId { get; set; }
        public string? TaskName { get; set; }

        public string DueDate { get; set; }

        public string? QuadrantNum { get; set; }

        [Required]
        public int? CategoryId { get; set; } // (foreign key)

        public Category? Category { get; set; } // //attatch a public instance of category and call it category

        public bool Completed { get; set; }

    }
}
