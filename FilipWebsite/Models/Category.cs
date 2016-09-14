using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FilipWebsite.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Names { get; set; }

        public virtual ICollection<Recipe> Recipes { get; set; }
    }

}