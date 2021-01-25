using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DAL.Helpers;
using DAL.Models;
using DAL.Parsing.ParserContext;
using DAL.Parsing.Parsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DAL.DbContext
{
    public static class AppDbContextInit
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var parserContext = new ParserContext();
            await context.Database.MigrateAsync();
            LaunchThread(parserContext, configuration);
        }

        private static void LaunchThread(IParserContext parserContext, IConfiguration configuration)
        {
            var threadStart = new ThreadStart(async () => await CheckParsingTime(parserContext, configuration));
            var thread = new Thread(threadStart);
            thread.Start();
        }

        private static async Task CheckParsingTime(IParserContext parserContext, IConfiguration configuration)
        {
            while (true)
            {
                var optionsBuilderService = new OptionsBuilderService<AppDbContext>(configuration);
                var options = optionsBuilderService.BuildOptions();
                await using var context = new AppDbContext(options);
                var productsExist = await context.Products.AnyAsync();
                var storeParsingDates = await context.StoreParsingDates.ToListAsync();
                var weekExpired = storeParsingDates.Any(e => (e.LastParsingDate - DateTimeOffset.UtcNow).Days < 0);
                if (!productsExist || weekExpired)
                {
                    await ParseATBProducts(context, parserContext);
                }

                await context.SaveChangesAsync();
                Thread.Sleep(50000);
            }
        }

        private static async Task ParseATBProducts(AppDbContext context, IParserContext parserContext)
        {
            var productsFromATBToDelete =
                await context.Products.Where(e => e.StoreName.ToLower().Equals("ATB".ToLower())).ToListAsync();
            if (productsFromATBToDelete.Count != 0)
            {
                context.Products.RemoveRange(productsFromATBToDelete);
            }

            parserContext.SetParsingStrategy(new AtbParser());
            var productsFromATBToAdd = parserContext.Parse("крупа", 20, null);
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