using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomeMiddlewareDemo
{
    class Program
    {
        private static List<Func<RequestDeleagte, RequestDeleagte>> _list = new List<Func<RequestDeleagte, RequestDeleagte>>();

        private static void Use(Func<RequestDeleagte, RequestDeleagte> reFunc)
        {
            _list.Add(reFunc);
        }

        static void Main(string[] args)
        {
            RequestDeleagte end = context =>
            {
                Console.WriteLine("end");
                return Task.CompletedTask;
            };

            // 这里 next 其实就是 end
            Use(next =>
            {
                return context =>
                {
                    Console.WriteLine("1");
                    return next(context);
                };
            });


            Use(next =>
            {
                return context =>
                {
                    Console.WriteLine("2");
                    return next(context);
                };
            });

            foreach (var func in _list)
            {
                end = func.Invoke(end);
            }

            end(new Context());

            Console.ReadKey();
        }
    }
}
