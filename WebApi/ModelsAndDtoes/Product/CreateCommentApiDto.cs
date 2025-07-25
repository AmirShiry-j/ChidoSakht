namespace WebApi.ModelsAndDtoes.Product
{
    public class CreateCommentApiDto
    {
        public string Text { get; set; }
        public byte Star { get; set; }
        public int ProductId { get; set; }
    }
    public class VoteOnCommentApiDto
    {
        public long CommentId { get; set; }
        public bool WasHelpful { get; set; }
    }
}
