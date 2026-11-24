using ETICARET.Entities;

namespace ETICARET.WebUI.Models
{
    public record ProductDetailsViewModel(
     Product Product,
     List<Product> RelatedProducts,
     CommentListViewModel Comments);
}
