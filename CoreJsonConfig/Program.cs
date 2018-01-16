using System;
using Microsoft.Extensions.Configuration;

namespace CoreJsonConfig
{
    class Program
    {
        static void Main(string[] args)
        {

            var builder = new ConfigurationBuilder().AddJsonFile("fuck.json");

            var config = builder.Build();

            Console.WriteLine($"{config["ClassName"]}");
            Console.WriteLine($"{config["No"]}");

            Console.WriteLine($"{config["Students:0:Name"]}");
            Console.WriteLine($"{config["Students:0:Age"]}");
            Console.WriteLine($"{config["Students:1:Name"]}");
            Console.WriteLine($"{config["Students:1:Age"]}");
            Console.WriteLine($"{config["Students:2:Name"]}");
            Console.WriteLine($"{config["Students:2:Age"]}");

            Console.ReadKey();
        }
    }
}
