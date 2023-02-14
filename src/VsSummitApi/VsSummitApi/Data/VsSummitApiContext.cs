using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VsSummitApi.Models;

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
