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
                    await ParseProducts(context, "ATB", "гречана", new AtbParser());
                    await context.SaveChangesAsync();
                    await ParseProducts(context, "Novus", "Крупа гречневая", new NovusParser());
                    await context.SaveChangesAsync();
                    await ParseProducts(context, "Fozzy", "Крупа+гречневая", new FozzyParser());
                    await context.SaveChangesAsync();
                }

               
                Thread.Sleep(300000);
            }
        }

        private static async Task ParseProducts(AppDbContext context, string shopName, string productName,
            IParser parser)
        {
            var productsToDelete =
                await context.Products.Where(e => e.Shop.ToLower().Equals(shopName.ToLower())).ToListAsync();
            if (productsToDelete.Count != 0)
            {
                context.Products.RemoveRange(productsToDelete);
            }

            _parserContext.SetParsingStrategy(parser);
            var productsToAdd = _parserContext.Parse(productName, 20, null);
            await context.Products.AddRangeAsync(productsToAdd);
            var lastParsingDate = await context.StoreParsingDates
                .Where(e => e.StoreName.ToLower().Equals(shopName.ToLower()))
                .FirstOrDefaultAsync();
            if (lastParsingDate != null)
            {
                context.Remove(lastParsingDate);
            }

            var parsingTime = new StoreParsingDate {LastParsingDate = DateTimeOffset.UtcNow, StoreName = shopName};
            await context.StoreParsingDates.AddAsync(parsingTime);
        }
    }
}