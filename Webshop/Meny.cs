using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Models;

namespace Webshop
{
    internal class CustomerManager
    {
        public void ShowCustomerPage()
        {
            using (var context = new MyDbContext())
            {
                Console.Clear();
                Console.WriteLine("\nVälkommen till kundsidan!");

                // Hämta tre produkter där IsChosen == true
                var chosenProducts = context.Products
                    .Where(p => p.IsChosen)
                    .Take(3)
                    .ToList();

                Console.WriteLine("\nUtvalda produkter:");
                if (chosenProducts.Any())
                {
                    DisplayChosenProducts(chosenProducts); // Visa produkter i rektanglar
                }
                else
                {
                    Console.WriteLine("Inga utvalda produkter tillgängliga.");
                }

                // Visa kategorier
                Console.WriteLine("\nTillgängliga kategorier:");
                var categories = context.Categories.ToList();

                if (categories.Any())
                {
                    foreach (var category in categories)
                    {
                        Console.WriteLine($"- {category.Name}");
                    }
                }
                else
                {
                    Console.WriteLine("Inga kategorier tillgängliga.");
                }
            }

            Console.WriteLine("\nTryck på valfri tangent för att gå tillbaka till startsidan.");
            Console.ReadKey();
        }

        private void DisplayChosenProducts(List<Product> products)
        {
            // Bygg rader för produkternas rektanglar
            var productBoxes = products.Select(p =>
            {
                var name = $"Namn: {p.Name}";
                var price = $"Pris: {p.Price} kr";

                // Maxbredd på rektangeln baserat på längsta raden
                int boxWidth = Math.Max(name.Length, price.Length) + 4;

                var topBottomBorder = new string('-', boxWidth);
                var paddedName = name.PadRight(boxWidth - 2);
                var paddedPrice = price.PadRight(boxWidth - 2);

                return new[]
                {
                $"+{topBottomBorder}+",
                $"| {paddedName} |",
                $"| {paddedPrice} |",
                $"+{topBottomBorder}+"
            };
            }).ToList();

            // Skriv ut raderna bredvid varandra
            for (int i = 0; i < productBoxes[0].Length; i++) // Alla boxar har samma antal rader
            {
                Console.WriteLine(string.Join("   ", productBoxes.Select(box => box[i])));
            }
        }
    }
}
