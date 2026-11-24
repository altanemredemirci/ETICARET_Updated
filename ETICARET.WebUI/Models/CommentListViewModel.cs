using ETICARET.Entities;

namespace ETICARET.WebUI.Models
{
    public record CommentListViewModel(int ProductId, List<Comment> Comments);
}
