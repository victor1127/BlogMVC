using Humanizer;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using BlogMVC.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using BlogMVC.Models;

namespace BlogMVC.Data
{
    public class BlogUser : IdentityUser
    {
        [Required]
        [Display(Name ="First name")]
        [StringLength(50, ErrorMessage="The {0} must be at least {2} and at most {1}",
          MinimumLength = 2)]
        public string FirstName { get; set; }
        
        [Required]
        [Display(Name = "Last name")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at most {1}",
          MinimumLength = 2)]
        public string LastName { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        [Required]
        [DataType(DataType.Date)]
        [ProtectedPersonalData]
        [Display(Name ="Date of birth")]
        public DateTime DateOfBirth { get; set; }

        public Gender Gender { get; set; } = Gender.Other;

        public byte[]? UserImageData { get; set; }
        public string? ContentType { get; set; }



        public ICollection<Blog> Blogs { get; set; } = new HashSet<Blog>();
        public ICollection<Post> Posts { get; set; } = new HashSet<Post>();


    }
}
