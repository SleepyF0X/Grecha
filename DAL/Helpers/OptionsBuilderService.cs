using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAL.Helpers
{
    internal sealed class OptionsBuilderService<T> where T : Microsoft.EntityFrameworkCore.DbContext
    {
        private readonly IConfiguration _configuration;

        public OptionsBuilderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbContextOptions<T> BuildOptions()
        {
            var optBuilder = new DbContextOptionsBuilder<T>();
            optBuilder.UseSqlServer(_configuration.GetConnectionString("Develop"));
            return optBuilder.Options;
        }
    }
}