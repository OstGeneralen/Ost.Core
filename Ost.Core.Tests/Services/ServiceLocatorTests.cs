using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ost.Core.Services;

namespace Ost.Core.Tests.Services
{
    [TestClass]
    public sealed class ServiceLocatorTests
    {
        public class TestService_Basic : IService<TestService_Basic>
        { 
        }
        public class TestService_WithInvokes : IService<TestService_WithInvokes>
        {
            public bool OnRegisteredInvoked { get; private set; } = false;
            public bool OnUnregisteredInvoked { get; private set; } = false;

            void OnRegistered() => OnRegisteredInvoked = true;
            void OnUnregistered() => OnUnregisteredInvoked = true;
        }

        [TestMethod("Register_service_allows_access_to_registered_service")]
        public void RegisterServiceTest()
        {
            var locator = new ServiceLocator();
            var service = new TestService_Basic();
            locator.Register(service);
            var gottenService = locator.GetService<TestService_Basic>();
            Assert.AreSame(service, gottenService);
        }

        [TestMethod("Emplace_service_allows_access_to_service")]
        public void EmplaceServiceTest()
        {
            var locator = new ServiceLocator();
            locator.EmplaceRegister<TestService_Basic>();
            Assert.IsNotNull(locator.TryGetService<TestService_Basic>());
        }

        [TestMethod("Locator_invokes_callbacks_if_the_exist_on_registered_service")]
        public void RegisterCallbackInvokeTest()
        {
            var locator = new ServiceLocator();

            var service = new TestService_WithInvokes();
            
            locator.Register(service);
            Assert.IsTrue(service.OnRegisteredInvoked);
            Assert.IsFalse(service.OnUnregisteredInvoked);

            locator.Unregister<TestService_WithInvokes>();
            Assert.IsTrue(service.OnUnregisteredInvoked);
        }

        [TestMethod("Locator_invokes_callbacks_if_the_exist_on_emplaced_service")]
        public void EmplaceCallbackInvokeTest()
        {
            var locator = new ServiceLocator();

            locator.EmplaceRegister<TestService_WithInvokes>();

            var service = locator.GetService<TestService_WithInvokes>();
            Assert.IsTrue(service.OnRegisteredInvoked);
            Assert.IsFalse(service.OnUnregisteredInvoked);

            locator.Unregister<TestService_WithInvokes>();
            Assert.IsTrue(service.OnUnregisteredInvoked);
        }
    }
}
