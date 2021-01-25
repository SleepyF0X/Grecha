using System;
using System.Threading;
using System.Threading.Tasks;
using DAL.DbContext;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Grecha.Services
{
    public sealed class ParsingService : IHostedService
    {
        private readonly IConfiguration _configuration;

        public ParsingService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await AppDbContextInit.InitializeAsync(_configuration);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
