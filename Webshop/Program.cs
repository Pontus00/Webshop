using System;
using Webshop.Models;

namespace Webshop
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var admin = new Admin();
          
            var productManager = new ProductManager(); // Hanterar admin-funktioner
            var customerManager = new CustomerManager(); // Hanterar kundfunktioner
            


            while (true)
            {
                Console.Clear();
                Console.WriteLine("Välj roll:");
                Console.WriteLine("1. Kund");
                Console.WriteLine("2. Admin");
                Console.WriteLine("3. Avsluta");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await customerManager.ShowCustomerPage(); // Öppnar kundsidan
                        break;
                    case "2":
                        admin.ShowAdminPage();  // Öppnar adminmenyn
                        break;
                    case "3":
                        Console.WriteLine("Tack för besöket! Programmet avslutas.");
                        return;
                    default:
                        Console.WriteLine("Ogiltigt val, försök igen.");
                        break;
                }
            }
        }
    }
}