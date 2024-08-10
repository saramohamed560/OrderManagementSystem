using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.Services.Contracts
{
    public interface IResponseCachingService
    {
        Task CaschResponseAsync(string key,object response,TimeSpan timeToLive);
        Task<string?> GetCachedResponseAsync(string key); 
    }
}
