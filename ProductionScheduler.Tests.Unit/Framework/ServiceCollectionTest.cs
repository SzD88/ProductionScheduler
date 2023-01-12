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


            serviceCollection.AddScoped<IMessanger, Messanger>();
            serviceCollection.AddScoped<IMessanger, Messanger>();
            serviceCollection.AddScoped<IMessanger, Messanger>();
            serviceCollection.AddScoped<IMessanger, Messanger2>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var messanager = serviceProvider.GetRequiredService<IMessanger>();
            messanager.Send();


            var messanage2 = serviceProvider.GetRequiredService<IMessanger>();
            messanage2.Send();

            var messangers = serviceProvider.GetServices<IMessanger>();

            messanager.ShouldNotBe(null);
            messanager.ShouldBe(messanage2);

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
        private class Messanger2 : IMessanger
        {
            private readonly Guid _id = Guid.NewGuid();
            public void Send()
            {
                Console.WriteLine("zz");
            }
        }
    }


}

