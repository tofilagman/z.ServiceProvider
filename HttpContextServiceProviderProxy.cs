using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace z.ServiceProvider
{
    public class HttpContextServiceProviderProxy : IServiceProviderProxy
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IServiceProvider serviceProvider;

        public HttpContextServiceProviderProxy(IHttpContextAccessor contextAccessor, IServiceProvider serviceProvider)
        {
            this.contextAccessor = contextAccessor;
            this.serviceProvider = serviceProvider;
        }

        public T GetService<T>()
        {
            var context = contextAccessor.HttpContext;
            if (context == null)
                return serviceProvider.GetService<T>();
            var services = context.RequestServices ?? serviceProvider;
            return services.GetService<T>();
        }

        public IEnumerable<T> GetServices<T>()
        {
            var service = contextAccessor.HttpContext?.RequestServices.GetServices<T>();
            if (service == null)
                return serviceProvider.GetServices<T>();
            return service;
        }

        public object GetService(Type type)
        { 
            var context = contextAccessor.HttpContext;
            if (context == null)
                return serviceProvider.GetService(type);
            var services = context.RequestServices ?? serviceProvider;
            return services.GetService(type);
        }

        public IEnumerable<object> GetServices(Type type)
        {
            var service = contextAccessor.HttpContext?.RequestServices.GetServices(type);
            if (service == null)
                return serviceProvider.GetServices(type);
            return service;
        }

        
    }
}
