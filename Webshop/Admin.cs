using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Models;

namespace Webshop
{
    internal class ProductManager
    {
        public void AddCategory()
        {
            Console.Write("Ange kategorins namn: ");
            string? name = Console.ReadLine();

            var category = new Category { Name = name };

            using (var context = new MyDbContext())
            {
                context.Categories.Add(category);
                context.SaveChanges();
            }

            Console.WriteLine($"Kategorin '{name}' har lagts till.");
        }

        public void AddSupplier()
        {
            Console.Write("Ange leverantörens namn: ");
            string? name = Console.ReadLine();

            var supplier = new Supplier { Name = name };

            using (var context = new MyDbContext())
            {
                context.Suppliers.Add(supplier);
                context.SaveChanges();
            }

            Console.WriteLine($"Leverantören '{name}' har lagts till.");
        }

        public void AddProduct()
        {
            using (var context = new MyDbContext())
            {
                Console.WriteLine("Tillgängliga kategorier:");
                foreach (var category in context.Categories)
                {
                    Console.WriteLine($"{category.Id}: {category.Name}");
                }

                Console.Write("Ange kategori-ID: ");
                int categoryId = int.Parse(Console.ReadLine() ?? "0");

                Console.WriteLine("Tillgängliga leverantörer:");
                foreach (var supplier in context.Suppliers)
                {
                    Console.WriteLine($"{supplier.Id}: {supplier.Name}");
                }

                Console.Write("Ange leverantörs-ID: ");
                int supplierId = int.Parse(Console.ReadLine() ?? "0");

                Console.Write("Ange produktnamn: ");
                string? name = Console.ReadLine();

                Console.Write("Ange produktbeskrivning: ");
                string? description = Console.ReadLine();

                Console.Write("Ange pris (decimal): ");
                decimal price = decimal.Parse(Console.ReadLine() ?? "0");

                Console.Write("Ange färg: ");
                string? color = Console.ReadLine();

                Console.Write("Är vald (true/false): ");
                bool isChosen = bool.Parse(Console.ReadLine() ?? "false");

                Console.Write("Ange antal i lager: ");
                int stock = int.Parse(Console.ReadLine() ?? "0");

                var product = new Product
                {
                    Name = name,
                    Description = description,
                    Price = price,
                    Color = color,
                    CategoryId = categoryId,
                    SupplierId = supplierId,
                    IsChosen = isChosen,
                    Stock = stock
                };

                context.Products.Add(product);
                context.SaveChanges();
            }

            Console.WriteLine("Produkten har lagts till i databasen.");
        }

        public void UpdateProduct()
        {
            using (var context = new MyDbContext())
            {
                Console.WriteLine("Befintliga produkter:");
                foreach (var product in context.Products)
                {
                    Console.WriteLine($"{product.Id}: {product.Name} - {product.Price} kr");
                }

                Console.Write("Ange ID för produkten du vill ändra: ");
                int productId = int.Parse(Console.ReadLine() ?? "0");

                var productToUpdate = context.Products.Find(productId);
                if (productToUpdate == null)
                {
                    Console.WriteLine("Produkten med det angivna ID:t hittades inte.");
                    return;
                }

                Console.WriteLine($"Ändrar produkt: {productToUpdate.Name}");

                Console.Write("Nytt namn (lämna tomt för att behålla): ");
                string? newName = Console.ReadLine();
                if (!string.IsNullOrEmpty(newName))
                {
                    productToUpdate.Name = newName;
                }

                Console.Write("Ny beskrivning (lämna tomt för att behålla): ");
                string? newDescription = Console.ReadLine();
                if (!string.IsNullOrEmpty(newDescription))
                {
                    productToUpdate.Description = newDescription;
                }

                Console.Write("Nytt pris (lämna tomt för att behålla): ");
                string? newPriceInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(newPriceInput))
                {
                    productToUpdate.Price = decimal.Parse(newPriceInput);
                }

                Console.Write("Ny färg (lämna tomt för att behålla): ");
                string? newColor = Console.ReadLine();
                if (!string.IsNullOrEmpty(newColor))
                {
                    productToUpdate.Color = newColor;
                }

                Console.Write("Nytt antal i lager (lämna tomt för att behålla): ");
                string? newStockInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(newStockInput))
                {
                    productToUpdate.Stock = int.Parse(newStockInput);
                }

                Console.WriteLine("Tillgängliga kategorier:");
                foreach (var category in context.Categories)
                {
                    Console.WriteLine($"{category.Id}: {category.Name}");
                }

                Console.Write("Ange nytt kategori-ID (lämna tomt för att behålla): ");
                string? newCategoryIdInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(newCategoryIdInput))
                {
                    int newCategoryId = int.Parse(newCategoryIdInput);
                    if (context.Categories.Any(c => c.Id == newCategoryId))
                    {
                        productToUpdate.CategoryId = newCategoryId;
                    }
                }

                Console.WriteLine("Tillgängliga leverantörer:");
                foreach (var supplier in context.Suppliers)
                {
                    Console.WriteLine($"{supplier.Id}: {supplier.Name}");
                }

                Console.Write("Ange nytt leverantörs-ID (lämna tomt för att behålla): ");
                string? newSupplierIdInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(newSupplierIdInput))
                {
                    int newSupplierId = int.Parse(newSupplierIdInput);
                    if (context.Suppliers.Any(s => s.Id == newSupplierId))
                    {
                        productToUpdate.SupplierId = newSupplierId;
                    }
                }

                context.SaveChanges();
                Console.WriteLine("Produkten har uppdaterats.");
            }
        }

        public void DeleteProduct()
        {
            using (var context = new MyDbContext())
            {
                Console.WriteLine("Befintliga produkter:");
                foreach (var product in context.Products)
                {
                    Console.WriteLine($"{product.Id}: {product.Name} - {product.Price} kr");
                }

                Console.Write("Ange ID för produkten du vill ta bort: ");
                int productId = int.Parse(Console.ReadLine() ?? "0");

                var productToDelete = context.Products.Find(productId);
                if (productToDelete == null)
                {
                    Console.WriteLine("Produkten med det angivna ID:t hittades inte.");
                    return;
                }

                context.Products.Remove(productToDelete);
                context.SaveChanges();
                Console.WriteLine($"Produkten '{productToDelete.Name}' har tagits bort.");
            }
        }
    }
    internal class Admin
    {
        public void ShowAdminPage()
        {
            using (var context = new MyDbContext())
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("\nAdmin-sida");

                    Console.WriteLine("1. Visa alla kunder");
                    Console.WriteLine("2. Visa orderdetaljer för en kund");
                    Console.WriteLine("3. Lägg till kategori");
                    Console.WriteLine("4. Lägg till leverantör");
                    Console.WriteLine("5. Lägg till produkt");
                    Console.WriteLine("6. Uppdatera produkt");
                    Console.WriteLine("7. Ta bort produkt");
                    Console.WriteLine("0. Avsluta");

                    var input = Console.ReadLine();
                    if (input == "1")
                    {
                        ShowAllCustomers(context);
                    }
                    else if (input == "2")
                    {
                        Console.Write("Ange kund-ID: ");
                        if (int.TryParse(Console.ReadLine(), out int customerId))
                        {
                            ShowCustomerOrders(context, customerId);
                        }
                        else
                        {
                            Console.WriteLine("Ogiltigt kund-ID. Försök igen.");
                        }
                    }
                    else if (input == "3")
                    {
                        AddCategory();
                    }
                    else if (input == "4")
                    {
                        AddSupplier();
                    }
                    else if (input == "5")
                    {
                        AddProduct();
                    }
                    else if (input == "6")
                    {
                        UpdateProduct();
                    }
                    else if (input == "7")
                    {
                        DeleteProduct();
                    }
                    else if (input == "0")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Ogiltigt val. Försök igen.");
                    }

                    Console.WriteLine("\nTryck på valfri tangent för att fortsätta.");
                    Console.ReadKey();
                }
            }
        }

        private void ShowAllCustomers(MyDbContext context)
        {
            var customers = context.Customers.ToList();
            if (!customers.Any())
            {
                Console.WriteLine("Inga kunder hittades.");
                return;
            }

            Console.WriteLine("\nKunder:");
            foreach (var customer in customers)
            {
                Console.WriteLine($"Kund-ID: {customer.Id}, Namn: {customer.Name}, E-post: {customer.Email}");
            }
        }

        private void ShowCustomerOrders(MyDbContext context, int customerId)
        {
            var customer = context.Customers.FirstOrDefault(c => c.Id == customerId);
            if (customer == null)
            {
                Console.WriteLine("Kunden hittades inte.");
                return;
            }

            var orders = context.Orders.Where(o => o.CustomerId == customerId).ToList();
            if (!orders.Any())
            {
                Console.WriteLine("Inga orders hittades för denna kund.");
                return;
            }

            Console.WriteLine($"\nOrders för kund: {customer.Name}");
            foreach (var order in orders)
            {
                ShowOrderDetails(order);
            }
        }

        private void ShowOrderDetails(Order order)
        {
            Console.WriteLine("\nOrderdetaljer:");
            Console.WriteLine($"Order-ID: {order.Id}");
            Console.WriteLine($"Orderdatum: {order.OrderDate}");
            Console.WriteLine($"Betalningsmetod: {order.PaymentMethod}");
            Console.WriteLine($"Fraktmetod: {order.ShippingMethod}");
            Console.WriteLine($"Fraktpris: {order.ShippingPrice} kr");
            Console.WriteLine($"Moms: {order.VAT} kr");
            Console.WriteLine($"Totalpris: {order.TotalPrice} kr");

            Console.WriteLine("\nOrderrader:");
            foreach (var item in order.Items)
            {
                var product = item.Product;
                Console.WriteLine($"Produkt-ID: {item.ProductId}, Namn: {product?.Name}, Antal: {item.Amount}, Pris: {product?.Price} kr");
            }
        }

        public void AddCategory()
        {
            Console.Write("Ange kategorins namn: ");
            string? name = Console.ReadLine();

            var category = new Category { Name = name };

            using (var context = new MyDbContext())
            {
                context.Categories.Add(category);
                context.SaveChanges();
            }

            Console.WriteLine($"Kategorin '{name}' har lagts till.");
        }

        public void AddSupplier()
        {
            Console.Write("Ange leverantörens namn: ");
            string? name = Console.ReadLine();

            var supplier = new Supplier { Name = name };

            using (var context = new MyDbContext())
            {
                context.Suppliers.Add(supplier);
                context.SaveChanges();
            }

            Console.WriteLine($"Leverantören '{name}' har lagts till.");
        }

        public void AddProduct()
        {
            using (var context = new MyDbContext())
            {
                Console.WriteLine("Tillgängliga kategorier:");
                foreach (var category in context.Categories)
                {
                    Console.WriteLine($"{category.Id}: {category.Name}");
                }

                Console.Write("Ange kategori-ID: ");
                int categoryId = int.Parse(Console.ReadLine() ?? "0");

                Console.WriteLine("Tillgängliga leverantörer:");
                foreach (var supplier in context.Suppliers)
                {
                    Console.WriteLine($"{supplier.Id}: {supplier.Name}");
                }

                Console.Write("Ange leverantörs-ID: ");
                int supplierId = int.Parse(Console.ReadLine() ?? "0");

                Console.Write("Ange produktnamn: ");
                string? name = Console.ReadLine();

                Console.Write("Ange produktbeskrivning: ");
                string? description = Console.ReadLine();

                Console.Write("Ange pris (decimal): ");
                decimal price = decimal.Parse(Console.ReadLine() ?? "0");

                Console.Write("Ange färg: ");
                string? color = Console.ReadLine();

                Console.Write("Är vald (true/false): ");
                bool isChosen = bool.Parse(Console.ReadLine() ?? "false");

                Console.Write("Ange antal i lager: ");
                int stock = int.Parse(Console.ReadLine() ?? "0");

                var product = new Product
                {
                    Name = name,
                    Description = description,
                    Price = price,
                    Color = color,
                    CategoryId = categoryId,
                    SupplierId = supplierId,
                    IsChosen = isChosen,
                    Stock = stock
                };

                context.Products.Add(product);
                context.SaveChanges();
            }

            Console.WriteLine("Produkten har lagts till i databasen.");
        }

        public void UpdateProduct()
        {
            using (var context = new MyDbContext())
            {
                Console.WriteLine("Befintliga produkter:");
                foreach (var product in context.Products)
                {
                    Console.WriteLine($"{product.Id}: {product.Name} - {product.Price} kr");
                }

                Console.Write("Ange ID för produkten du vill ändra: ");
                int productId = int.Parse(Console.ReadLine() ?? "0");

                var productToUpdate = context.Products.Find(productId);
                if (productToUpdate == null)
                {
                    Console.WriteLine("Produkten med det angivna ID:t hittades inte.");
                    return;
                }

                Console.WriteLine($"Ändrar produkt: {productToUpdate.Name}");

                Console.Write("Nytt namn (lämna tomt för att behålla): ");
                string? newName = Console.ReadLine();
                if (!string.IsNullOrEmpty(newName))
                {
                    productToUpdate.Name = newName;
                }

                Console.Write("Ny beskrivning (lämna tomt för att behålla): ");
                string? newDescription = Console.ReadLine();
                if (!string.IsNullOrEmpty(newDescription))
                {
                    productToUpdate.Description = newDescription;
                }

                Console.Write("Nytt pris (lämna tomt för att behålla): ");
                string? newPriceInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(newPriceInput))
                {
                    productToUpdate.Price = decimal.Parse(newPriceInput);
                }

                Console.Write("Ny färg (lämna tomt för att behålla): ");
                string? newColor = Console.ReadLine();
                if (!string.IsNullOrEmpty(newColor))
                {
                    productToUpdate.Color = newColor;
                }

                Console.Write("Nytt antal i lager (lämna tomt för att behålla): ");
                string? newStockInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(newStockInput))
                {
                    productToUpdate.Stock = int.Parse(newStockInput);
                }

                Console.WriteLine("Tillgängliga kategorier:");
                foreach (var category in context.Categories)
                {
                    Console.WriteLine($"{category.Id}: {category.Name}");
                }

                Console.Write("Ange nytt kategori-ID (lämna tomt för att behålla): ");
                string? newCategoryIdInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(newCategoryIdInput))
                {
                    int newCategoryId = int.Parse(newCategoryIdInput);
                    if (context.Categories.Any(c => c.Id == newCategoryId))
                    {
                        productToUpdate.CategoryId = newCategoryId;
                    }
                }

                Console.WriteLine("Tillgängliga leverantörer:");
                foreach (var supplier in context.Suppliers)
                {
                    Console.WriteLine($"{supplier.Id}: {supplier.Name}");
                }

                Console.Write("Ange nytt leverantörs-ID (lämna tomt för att behålla): ");
                string? newSupplierIdInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(newSupplierIdInput))
                {
                    int newSupplierId = int.Parse(newSupplierIdInput);
                    if (context.Suppliers.Any(s => s.Id == newSupplierId))
                    {
                        productToUpdate.SupplierId = newSupplierId;
                    }
                }

                context.SaveChanges();
                Console.WriteLine("Produkten har uppdaterats.");
            }
        }

        public void DeleteProduct()
        {
            using (var context = new MyDbContext())
            {
                Console.WriteLine("Befintliga produkter:");
                foreach (var product in context.Products)
                {
                    Console.WriteLine($"{product.Id}: {product.Name} - {product.Price} kr");
                }

                Console.Write("Ange ID för produkten du vill ta bort: ");
                int productId = int.Parse(Console.ReadLine() ?? "0");

                var productToDelete = context.Products.Find(productId);
                if (productToDelete == null)
                {
                    Console.WriteLine("Produkten med det angivna ID:t hittades inte.");
                    return;
                }

                context.Products.Remove(productToDelete);
                context.SaveChanges();
                Console.WriteLine($"Produkten '{productToDelete.Name}' har tagits bort.");
            }
        }
    }
}
