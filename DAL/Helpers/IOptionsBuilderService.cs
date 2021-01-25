using Microsoft.EntityFrameworkCore;

namespace DAL.Helpers
{
    public interface IOptionsBuilderService<T> where T : Microsoft.EntityFrameworkCore.DbContext
    {
        DbContextOptions<T> BuildDefaultOptions();
    }
}