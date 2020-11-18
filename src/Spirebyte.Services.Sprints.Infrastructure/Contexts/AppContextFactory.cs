using Convey.MessageBrokers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Spirebyte.Services.Sprints.Application;

namespace Spirebyte.Services.Sprints.Infrastructure.Contexts
{
    internal sealed class AppContextFactory : IAppContextFactory
    {
        private readonly ICorrelationContextAccessor _contextAccessor;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppContextFactory(ICorrelationContextAccessor contextAccessor, IHttpContextAccessor httpContextAccessor)
        {
            _contextAccessor = contextAccessor;
            _httpContextAccessor = httpContextAccessor;
        }

        public IAppContext Create()
        {
            if (_contextAccessor.CorrelationContext is { })
            {
                var payload = JsonConvert.SerializeObject(_contextAccessor.CorrelationContext);

                return string.IsNullOrWhiteSpace(payload)
                    ? AppContext.Empty
                    : new AppContext(JsonConvert.DeserializeObject<CorrelationContext>(payload));
            }

            var context = _httpContextAccessor.GetCorrelationContext();

            return context is null ? AppContext.Empty : new AppContext(context);
        }
    }
}