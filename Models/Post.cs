using BlogMVC.Data;
using BlogMVC.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogMVC.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        [Required]
        public string AuthorId { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1}", MinimumLength = 2)]
        public string Title { get; set; }


        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} and at most {1}", MinimumLength = 2)]
        public string Preview { get; set; }


        [Required]
        public string Content { get; set; }
        public PostState PostState { get; set; }
        public string? Slug { get; set; }


        [DataType(DataType.Date)]
        [Display(Name ="Created date")]

        public DateTime Created { get; set; }
        [DataType(DataType.Date)]

        [Display(Name = "Updated date")]
        public DateTime Updated { get; set; }


        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public byte[]? ImageData { get; set; }
        public string? ContentType { get; set; }



        public virtual Blog Blog { get; set; }
        public virtual BlogUser Author { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; } = new HashSet<Comment>();
        public virtual ICollection<Tag>? Tags { get; set; } = new HashSet<Tag>();



    }
}
