using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ost.Core.Services
{
    internal class RegisteredServiceInfo
    {
        public RegisteredServiceInfo(Type serviceType, object serviceObject)
        {
            _serviceType = serviceType;
            _serviceObject = serviceObject;

            _onRegisterMethod = serviceType.GetMethod("OnRegistered", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            _onUnregisteredMethod = serviceType.GetMethod("OnUnregistered", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        public T? TryCastToType<T>() where T : class, IService 
        {
            return _serviceObject as T;
        }

        public void NotifyRegistered()
        {
            if (_onRegisterMethod != null) _onRegisterMethod.Invoke(_serviceObject, null);
        }

        public void NotifyUnregistered()
        {
            if (_onUnregisteredMethod != null) _onUnregisteredMethod.Invoke(_serviceObject, null);
        }

        private MethodInfo? _onRegisterMethod = null;
        private MethodInfo? _onUnregisteredMethod = null;

        private object _serviceObject;
        private Type _serviceType;
    }
}
