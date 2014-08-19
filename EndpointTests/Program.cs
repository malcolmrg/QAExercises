using System;
using Xero.Api.Core;
using Xero.Api.Example.TokenStores;
using Xero.Api.Infrastructure.OAuth;
using Xero.Api.Example.Applications.Private;

namespace EndpointTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var user = new ApiUser { Name = Environment.MachineName };
            var tokenStore = new SqliteTokenStore();

            var api = new Core()
            {
                UserAgent = "Xero Api - Listing example"
            };
            
            new Lister(api).List(); 
        }
    }
}
