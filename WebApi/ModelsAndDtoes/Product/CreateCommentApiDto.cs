using System.ComponentModel.DataAnnotations;

namespace WebApi.ModelsAndDtoes.Product
{
    public class CreateCommentApiDto
    {
        [Required]
        [MaxLength(200)]
        public string Text { get; set; }
        [Required]
        [Range(1, 5)]
        public byte Star { get; set; }
        [Required]
        public int ProductId { get; set; }
    }
    public class CreateVoteOnCommentApiDto
    {
        [Required]
        public long CommentId { get; set; }
        [Required]
        public bool WasHelpful { get; set; }
    }

    public class CommentFilterForUserSectionApiDto
    {
        [Required]
        public int ProductId { get; set; }
        public int? Page { get; set; } = 1;
        public int? CountInPage { get; set; } = 10;
    }
}
