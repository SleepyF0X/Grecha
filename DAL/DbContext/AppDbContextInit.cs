using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DAL.Helpers;
using DAL.Models;
using DAL.Parsing.ParserContext;
using DAL.Parsing.Parsers;
using Microsoft.EntityFrameworkCore;

namespace DAL.DbContext
{
    public static class AppDbContextInit
    {
        private static IParserContext _parserContext;
        private static DbContextOptions<AppDbContext> _options;

        public static async Task InitializeAsync(IOptionsBuilderService<AppDbContext> optionsBuilderService)
        {
            _options = optionsBuilderService.BuildDefaultOptions();
            await using var context = new AppDbContext(_options);
            _parserContext = new ParserContext();
            await context.Database.MigrateAsync();
            LaunchThread();
        }

        private static void LaunchThread()
        {
            var threadStart = new ThreadStart(async () => await CheckParsingTime());
            var thread = new Thread(threadStart);
            thread.Start();
        }

        private static async Task CheckParsingTime()
        {
            while (true)
            {
                await using var context = new AppDbContext(_options);
                var productsExist = await context.Products.AnyAsync();
                var storeParsingDates = await context.StoreParsingDates.ToListAsync();
                var weekExpired = storeParsingDates.Any(e => (e.LastParsingDate - DateTimeOffset.UtcNow).Days < 0);
                if (!productsExist || weekExpired)
                {
                    await ParseATBProducts(context);
                }

                await context.SaveChangesAsync();
                Thread.Sleep(300000);
            }
        }

        private static async Task ParseATBProducts(AppDbContext context)
        {
            var productsFromATBToDelete =
                await context.Products.Where(e => e.StoreName.ToLower().Equals("ATB".ToLower())).ToListAsync();
            if (productsFromATBToDelete.Count != 0)
            {
                context.Products.RemoveRange(productsFromATBToDelete);
            }

            _parserContext.SetParsingStrategy(new AtbParser());
            var productsFromATBToAdd = _parserContext.Parse("крупа", 20, null);
            await context.Products.AddRangeAsync(productsFromATBToAdd);
            var lastParsingDate = await context.StoreParsingDates
                .Where(e => e.StoreName.ToLower().Equals("ATB".ToLower()))
                .FirstOrDefaultAsync();
            if (lastParsingDate != null)
            {
                context.Remove(lastParsingDate);
            }

            var parsingTime = new StoreParsingDate {LastParsingDate = DateTimeOffset.UtcNow, StoreName = "ATB"};
            await context.StoreParsingDates.AddAsync(parsingTime);
        }
    }
}