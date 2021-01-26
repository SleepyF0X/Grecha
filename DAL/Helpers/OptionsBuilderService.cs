using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAL.Helpers
{
    public sealed class OptionsBuilderService<T> : IOptionsBuilderService<T> where T: Microsoft.EntityFrameworkCore.DbContext
    {
        private readonly IConfiguration _configuration;

        public OptionsBuilderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbContextOptions<T> BuildDefaultOptions()
        {
            var optBuilder = new DbContextOptionsBuilder<T>();
            optBuilder.UseSqlServer(_configuration.GetConnectionString("Docker"));
            return optBuilder.Options;
        }
    }
}