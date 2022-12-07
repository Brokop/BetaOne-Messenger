namespace BetaOne
{
    internal class Program
    {

        const int port = 7767;

        static void Main(string[] args)
        {
            Server server = new Server(port);
            server.init();

            Client client = new Client();
            client.Init("127.0.0.1", port);


        }
    }



}