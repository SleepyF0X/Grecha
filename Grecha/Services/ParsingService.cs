using System;
using System.Threading;
using System.Threading.Tasks;
using DAL.DbContext;
using DAL.Helpers;
using Microsoft.Extensions.Hosting;

namespace Grecha.Services
{
    public sealed class ParsingService : IHostedService
    {
        private readonly IOptionsBuilderService<AppDbContext> _optionsBuilderService;

        public ParsingService(IOptionsBuilderService<AppDbContext> optionsBuilderService)
        {
            _optionsBuilderService = optionsBuilderService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await AppDbContextInit.InitializeAsync(_optionsBuilderService);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}