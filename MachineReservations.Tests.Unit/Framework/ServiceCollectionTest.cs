using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;

namespace MachineReservations.Tests.Unit.Framework
{
    public class ServiceCollectionTest
    {
        [Fact]
        public void test()
        {

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddTransient<IMessanger, Messanger>();

            var servicePovider = serviceCollection.BuildServiceProvider();

            var messanager = servicePovider.GetRequiredService< IMessanger>();

            messanager.ShouldNotBe(null);
        }

            private interface IMessanger
        {
            void Send();
        }
        private class Messanger : IMessanger
        {
            private readonly Guid _id = Guid.NewGuid();
            public void Send()
            {
                Console.WriteLine("zz");
            }
        }
    }


}
 
