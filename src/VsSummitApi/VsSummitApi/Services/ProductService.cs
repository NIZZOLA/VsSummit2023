using VsSummitApi.Interfaces.Services;
using VsSummitApi.Models;

namespace VsSummitApi.Services;

public class ProductService : IProductService
{
    private readonly ILogger _logger;

    public ProductService(ILogger<ProductService> logger)
    {
        _logger= logger;
    }
    public ICollection<ProductModel> GetAll()
    {
        _logger.LogDebug("Entrou no método aqui !");
        var list = new List<ProductModel>();
        _logger.LogDebug($"Obteve {list.Count} itens para retorno");
        return list;
    }
}
