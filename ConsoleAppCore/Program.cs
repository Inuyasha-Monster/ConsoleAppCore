using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleAppCore
{
    class Program
    {
        static void Main(string[] args)
        {

            var dic = new Dictionary<string, string>()
            {
                {"name","djl" },
                {"age","24" }
            };

            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(dic);
            builder.AddCommandLine(args);

            var configuration = builder.Build();

            Console.WriteLine($"name:{ configuration["name"] } age:{configuration["age"]}");

            //var services = new ServiceCollection();
            //// 默认构造
            //services.AddSingleton<IOperationSingleton, Operation>();
            //// 自定义传入Guid空值
            //services.AddSingleton<IOperationSingleton>(
            //    new Operation(Guid.Empty));
            //// 自定义传入一个New的Guid
            //Guid guid = Guid.NewGuid();
            //Console.WriteLine(guid);
            //services.AddSingleton<IOperationSingleton>(
            //    new Operation(guid));

            //var provider = services.BuildServiceProvider();

            //// 输出singletone1的Guid
            //var singletone1 = provider.GetService<IOperationSingleton>();
            //Console.WriteLine($"signletone1: {singletone1.OperationId}");

            //// 输出singletone2的Guid
            //var singletone2 = provider.GetService<IOperationSingleton>();
            //Console.WriteLine($"signletone2: {singletone2.OperationId}");
            //Console.WriteLine($"singletone1 == singletone2 ? : { singletone1 == singletone2 }");

            Console.WriteLine("------------------------------------");

            //var services1 = new ServiceCollection();
            //services1.AddTransient<IOperationTransient, Operation>();

            //var provider1 = services1.BuildServiceProvider();

            //var transient1 = provider1.GetService<IOperationTransient>();
            //Console.WriteLine($"transient1: {transient1.OperationId}");

            //var transient2 = provider1.GetService<IOperationTransient>();
            //Console.WriteLine($"transient2: {transient2.OperationId}");
            //Console.WriteLine($@"transient1 == transient2 ? : { transient1 == transient2 }");

            Console.WriteLine("------------------------------------");

            var services = new ServiceCollection()
                .AddScoped<IOperationScoped, Operation>()
                .AddTransient<IOperationTransient, Operation>()
                .AddSingleton<IOperationSingleton, Operation>();

            var provider = services.BuildServiceProvider();
            using (var scope1 = provider.CreateScope())
            {
                var p = scope1.ServiceProvider;

                var scopeobj1 = p.GetService<IOperationScoped>();
                var transient1 = p.GetService<IOperationTransient>();
                var singleton1 = p.GetService<IOperationSingleton>();

                var scopeobj2 = p.GetService<IOperationScoped>();
                var transient2 = p.GetService<IOperationTransient>();
                var singleton2 = p.GetService<IOperationSingleton>();

                Console.WriteLine(
                    $"scope1: { scopeobj1.OperationId }," +
                    $"transient1: {transient1.OperationId}, " +
                    $"singleton1: {singleton1.OperationId}");

                Console.WriteLine($"scope2: { scopeobj2.OperationId }, " +
                    $"transient2: {transient2.OperationId}, " +
                    $"singleton2: {singleton2.OperationId}");
            }

            Console.ReadKey();
        }
    }
}
