using BlogMVC.Data;
using System.ComponentModel.DataAnnotations;

namespace BlogMVC.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string AuthorId { get; set; }


        [Required]
        [Display(Name ="Tag")]
        [StringLength(25, ErrorMessage ="The {0} must be at most {1} and at least {2}", MinimumLength =2)]
        public string Content { get; set; }


        public virtual Post Post { get; set; }
        public virtual BlogUser Author { get; set; }


    }
}
