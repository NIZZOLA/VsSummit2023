using VsSummitApi.Models;

namespace VsSummitApi.Interfaces.Services;

public interface IProductService
{
	public ICollection<ProductModel> GetAll();
}
