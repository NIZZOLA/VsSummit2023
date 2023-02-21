using VsSummitApi.Interfaces.Services;
using VsSummitApi.Models;

namespace VsSummitApi.Services
{
    public class ProductService : IProductService
    {
        public ICollection<ProductModel> GetAll()
        {
            return new List<ProductModel>();
        }
    }
}
