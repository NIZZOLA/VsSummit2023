using Microsoft.EntityFrameworkCore;

namespace VsSummitApi.Data
{
    public class VsSummitApiContext : DbContext
    {
        public VsSummitApiContext (DbContextOptions<VsSummitApiContext> options)
            : base(options)
        {
        }

        public DbSet<VsSummitApi.Models.ProductModel> ProductModel { get; set; } = default!;
    }
}
