using System;

namespace AzexWebAPI
{
    class Program
    {
        static void Main(string[] args)
        {

            AzexApiService service = new AzexApiService("your apikey", "your secretkey");
            //Get all currencies
            var platformCurrencies = service.GetCurrencies().GetAwaiter().GetResult();
            //Get balance
            var balance = service.GetBalance(new System.Collections.Generic.List<string>() { "btc", "eth" }).GetAwaiter().GetResult(); 
            Console.ReadKey();
        }
    }
}
