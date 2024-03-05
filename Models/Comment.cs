using BlogMVC.Data;
using BlogMVC.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BlogMVC.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string AuthorId { get; set; }
        public string ModeratorId { get; set; }


        [Required]
        [Display(Name ="Commend")]
        [StringLength(500, MinimumLength = 2)]
        public string Body { get; set; }

        [Display(Name = "Moderated Commend")]
        [StringLength(500, MinimumLength = 2)]
        public string? ModeratedBody { get; set; }

        public ModeratedReason ModeratedReason { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }


        public virtual Post Post { get; set; }
        public virtual BlogUser Author { get; set; }
        public virtual BlogUser Moderator { get; set; }

    }
}
