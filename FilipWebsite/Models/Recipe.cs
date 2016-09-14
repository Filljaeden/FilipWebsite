using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FilipWebsite.Models
{
    public class Recipe
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        public string AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public virtual ApplicationUser Author { get; set; }

         public int CategoryId { get; set; }

         public virtual Category Category { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

    }
}