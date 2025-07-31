namespace WebApi.Areas.Admin.ModelsAndDtoes.Comments
{
    public class CommentFilterForAdminSectionApiDto
    {
        public int? ProductId { get; set; }
        public bool? Confirmation { get; set; }
        public string? UserId { get; set; }
        public int? Page { get; set; } = 1;
        public int? CountInPage { get; set; } = 10;
    }
}
