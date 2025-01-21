using System;
using Webshop.Models;

namespace Webshop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var productManager = new ProductManager(); // Hanterar admin-funktioner
            var customerManager = new CustomerManager(); // Hanterar kundfunktioner

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Välkommen till Webshop!");
                Console.WriteLine("Välj roll:");
                Console.WriteLine("1. Kund");
                Console.WriteLine("2. Admin");
                Console.WriteLine("3. Avsluta");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        customerManager.ShowCustomerPage(); // Öppnar kundsidan
                        break;
                    case "2":
                        ShowAdminMenu(productManager); // Öppnar adminmenyn
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

        static void ShowAdminMenu(ProductManager productManager)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\nAdminmeny:");
                Console.WriteLine("1. Lägg till kategori");
                Console.WriteLine("2. Lägg till leverantör");
                Console.WriteLine("3. Lägg till produkt");
                Console.WriteLine("4. Ändra en produkt");
                Console.WriteLine("5. Ta bort en produkt");
                Console.WriteLine("6. Tillbaka till startsidan");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        productManager.AddCategory();
                        break;
                    case "2":
                        productManager.AddSupplier();
                        break;
                    case "3":
                        productManager.AddProduct();
                        break;
                    case "4":
                        productManager.UpdateProduct();
                        break;
                    case "5":
                        productManager.DeleteProduct();
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("Ogiltigt val, försök igen.");
                        break;
                }
            }
        }
    }
}