using System.ComponentModel.DataAnnotations;

namespace FilipWebsite.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        public string Body { get; set; }

        public string Author { get; set; }

        public int RecipeId { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}