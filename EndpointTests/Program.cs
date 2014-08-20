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
            // Create new API connection. This is a private application so only the signed certificate and consumer key are required. These are found in the App.config.
            var api = new Core()
            {
                UserAgent = "Xero Api - Endpoint Listing example"
            };
            
            // Instantiate new Lister class that will list information by connecting to the various endpoints.
            new Lister(api).List(); 
        }
    }
}
