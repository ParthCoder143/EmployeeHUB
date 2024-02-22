using EmployeeDAA.Core.Infrastructure;
using EmployeeDAA.Services.Settings;
using LinqToDB.Common.Internal.Cache;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeDAA.Api.InfraStructure
{
    public static class Message
    {
        public const string ListLoadSuccessMessageTemplate = "{0} data loaded successfully.";
        public const string NoDataFound = "No data found.";
        public const string Success = "Success";
        public const string InvalidRequestParmeters = "Invalid request parameters.";
        public static string GetMessage(string TagCode)
        {
            IMemoryCache cache = new HttpContextAccessor().HttpContext.RequestServices.GetService(typeof(IMemoryCache)) as IMemoryCache;
            List<SystemMessage> result = new();
            if (!Common.GetCache(cache, "_cacheOfSystemMessage", out object CacheSystemMessage))
            {
                ISystemMessageService _IMessageRepository = new HttpContextAccessor().HttpContext.RequestServices.GetService(typeof(ISystemMessageService)) as ISystemMessageService;
                result = Task.Run(() => _IMessageRepository.GetAllAsync()).Result;
                Common.SetCache(cache, "_cacheOfSystemMessage", result);
            }
            else
            {
                result = new List<SystemMessage>((IEnumerable<SystemMessage>)CacheSystemMessage);
            }
            return result?.Where(x => x.TagCode == TagCode)?.Select(x => x.TagMsg)?.FirstOrDefault() ?? "";
        }
    }
}
