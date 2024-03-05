using BlogMVC.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogMVC.Models
{
    public class Blog
    {
        public int Id { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage ="The {0} must be at least {2} and at most {1}", MinimumLength =2)]
        public string Name { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} and at most {1}", MinimumLength = 2)]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name="Created Date")]
        public DateTime Created { get; set; }
        [Display(Name = "Updated Date")]
        public DateTime? Updated { get; set; }

        public byte[]? ImageData { get; set; }
        public string? ContentType { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }



        public virtual BlogUser Author { get; set; }
        public virtual ICollection<Post>? Posts { get; set; }=new HashSet<Post>();
    }
}
