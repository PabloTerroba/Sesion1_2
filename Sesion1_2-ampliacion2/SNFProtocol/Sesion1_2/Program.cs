using System;
using System.Threading;
using System.Threading.Tasks;

namespace SNFProtocol
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting SNF system test...");
            // Ejecutar el servidor en un hilo separado
            Task.Run(() =>
            {
                SNFServer server = new SNFServer();
                server.Run();
            });

            // Esperar un poco para que el servidor inicie
            Thread.Sleep(2000);

            // Ejecutar el cliente
            SNFClient client = new SNFClient();
            client.Run();

            Console.WriteLine("Test end.");
        }
    }
}
