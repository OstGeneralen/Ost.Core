using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ost.Core.Services
{
    public class ServiceLocator
    {
        public ServiceLocator() 
        {
            _services = new Dictionary<Type, RegisteredServiceInfo>();
        }
        
        public void EmplaceRegister<T>() where T: IService, new()
        {
            Register(new T());
        }
        public void Register(IService service)
        {
            if (_services.ContainsKey(service.ServiceType)) throw new Exception($"Service {service.ServiceType.Name} is already registered with locator");

            var serviceInfo = new RegisteredServiceInfo(service.ServiceType, service);
            _services.Add(service.ServiceType, serviceInfo);
            serviceInfo.NotifyRegistered();
        }
        public void Unregister<T>() where T: IService
        {
            if(_services.TryGetValue(typeof(T), out var serviceInfo))
            {
                serviceInfo.NotifyUnregistered();
                _services.Remove(typeof(T));
            }
        }
        
        public void Clear()
        {
            foreach(var serviceInfo in _services.Values)
            {
                serviceInfo.NotifyUnregistered();
            }
            _services.Clear();
        }
        
        public T? TryGetService<T>() where T: class, IService
        {
            if(_services.TryGetValue(typeof(T), out var info))
            {
                var castService = info.TryCastToType<T>();
                if (castService == null) throw new Exception($"Service found but failed to cast to {typeof(T).Name}");
                return castService;
            }
            return null;
        }
        public T GetService<T>() where T: class, IService
        {
            var service = TryGetService<T>();
            if(service != null)
            {
                return service;
            }
            throw new Exception($"No service of type {typeof(T).Name} registered");
        }

        private Dictionary<Type, RegisteredServiceInfo> _services;
    }
}
