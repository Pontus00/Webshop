using Microsoft.EntityFrameworkCore;
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
        private MyDbContext dbContext;

        public ProductManager()
        {
            dbContext = new MyDbContext();
        }

        public void AddCategory()
        {
            Console.Write("Ange kategorins namn: ");
            string? name = Console.ReadLine();

            var category = new Category { Name = name };

            dbContext.Categories.Add(category);
            dbContext.SaveChanges();

            Console.WriteLine($"Kategorin '{name}' har lagts till.");
        }

        public void AddSupplier()
        {
            Console.Write("Ange leverantörens namn: ");
            string? name = Console.ReadLine();

            var supplier = new Supplier { Name = name };

            dbContext.Suppliers.Add(supplier);
            dbContext.SaveChanges();

            Console.WriteLine($"Leverantören '{name}' har lagts till.");
        }

        public void AddProduct()
        {
            Console.WriteLine("Tillgängliga kategorier:");
            foreach (var category in dbContext.Categories)
            {
                Console.WriteLine($"{category.Id}: {category.Name}");
            }

            Console.Write("Ange kategori-ID: ");
            int categoryId = int.Parse(Console.ReadLine() ?? "0");

            Console.WriteLine("Tillgängliga leverantörer:");
            foreach (var supplier in dbContext.Suppliers)
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

            dbContext.Products.Add(product);
            dbContext.SaveChanges();

            Console.WriteLine("Produkten har lagts till i databasen.");
        }

        public void UpdateProduct()
        {
            Console.WriteLine("Befintliga produkter:");
            foreach (var product in dbContext.Products)
            {
                Console.WriteLine($"{product.Id}: {product.Name} - {product.Price} kr");
            }

            Console.Write("Ange ID för produkten du vill ändra: ");
            int productId = int.Parse(Console.ReadLine() ?? "0");

            var productToUpdate = dbContext.Products.Find(productId);
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
            foreach (var category in dbContext.Categories)
            {
                Console.WriteLine($"{category.Id}: {category.Name}");
            }

            Console.Write("Ange nytt kategori-ID (lämna tomt för att behålla): ");
            string? newCategoryIdInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(newCategoryIdInput))
            {
                int newCategoryId = int.Parse(newCategoryIdInput);
                if (dbContext.Categories.Any(c => c.Id == newCategoryId))
                {
                    productToUpdate.CategoryId = newCategoryId;
                }
            }

            Console.WriteLine("Tillgängliga leverantörer:");
            foreach (var supplier in dbContext.Suppliers)
            {
                Console.WriteLine($"{supplier.Id}: {supplier.Name}");
            }

            Console.Write("Ange nytt leverantörs-ID (lämna tomt för att behålla): ");
            string? newSupplierIdInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(newSupplierIdInput))
            {
                int newSupplierId = int.Parse(newSupplierIdInput);
                if (dbContext.Suppliers.Any(s => s.Id == newSupplierId))
                {
                    productToUpdate.SupplierId = newSupplierId;
                }
            }

            dbContext.SaveChanges();
            Console.WriteLine("Produkten har uppdaterats.");
        }

        public void DeleteProduct()
        {
            Console.WriteLine("Befintliga produkter:");
            foreach (var product in dbContext.Products)
            {
                Console.WriteLine($"{product.Id}: {product.Name} - {product.Price} kr");
            }

            Console.Write("Ange ID för produkten du vill ta bort: ");
            int productId = int.Parse(Console.ReadLine() ?? "0");

            var productToDelete = dbContext.Products.Find(productId);
            if (productToDelete == null)
            {
                Console.WriteLine("Produkten med det angivna ID:t hittades inte.");
                return;
            }

            dbContext.Products.Remove(productToDelete);
            dbContext.SaveChanges();
            Console.WriteLine($"Produkten '{productToDelete.Name}' har tagits bort.");
        }
    }
    internal class Admin
    {
        private MyDbContext dbContext;

        public Admin()
        {
            dbContext = new MyDbContext();
        }
        public void ShowAdminPage()
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
                Console.WriteLine("8. Updatera kund");
                Console.WriteLine("0. Avsluta");

                var input = Console.ReadLine();
                if (input == "1")
                {
                    ShowAllCustomers();
                }
                else if (input == "2")
                {
                    ShowAllCustomers();
                    Console.Write("Ange kund-ID: ");
                    if (int.TryParse(Console.ReadLine(), out int customerId))
                    {
                        ShowCustomerOrders(customerId);
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
                else if (input == "8")
                {
                    UpdateCustomerDetails();
                }
                else if (input == "0")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Ogiltigt val. Försök igen.");
                }

                Console.WriteLine("Tryck på valfri tangent för att fortsätta.");
                Console.ReadKey();
            }
        }

        private void ShowAllCustomers()
        {
            var customers = dbContext.Customers.ToList();
            if (!customers.Any())
            {
                Console.WriteLine("Inga kunder hittades.");
                return;
            }

            Console.WriteLine("Kunder:");
            foreach (var customer in customers)
            {
                Console.WriteLine($"Kund-ID: {customer.Id}, Namn: {customer.Name}, E-post: {customer.Email}");
            }
        }

        private void ShowCustomerOrders(int customerId)
        {
            var customer = dbContext.Customers.FirstOrDefault(c => c.Id == customerId);
            if (customer == null)
            {
                Console.WriteLine("Kunden hittades inte.");
                return;
            }

            var orders = dbContext.Orders
                .Include(o => o.Items)
                .Where(o => o.CustomerId == customerId)
                .ToList();
            if (!orders.Any())
            {
                Console.WriteLine("Inga orders hittades för denna kund.");
                return;
            }

            Console.WriteLine($"Orders för kund: {customer.Name}");
            foreach (var order in orders)
            {
                ShowOrderDetails(order);
            }
        }

        private void ShowOrderDetails(Order order)
        {
            Console.WriteLine("Orderdetaljer:");
            Console.WriteLine($"Order-ID: {order.Id}");
            Console.WriteLine($"Orderdatum: {order.OrderDate}");
            Console.WriteLine($"Betalningsmetod: {order.PaymentMethod}");
            Console.WriteLine($"Fraktmetod: {order.ShippingMethod}");
            Console.WriteLine($"Fraktpris: {order.ShippingPrice} kr");
            Console.WriteLine($"Moms: {order.VAT} kr");
            Console.WriteLine($"Totalpris: {order.TotalPrice} kr");

            Console.WriteLine("Orderrader:");
            foreach (var item in order.Items)
            {
                var product = dbContext.Products.Find(item.ProductId);
                Console.WriteLine($"Produkt-ID: {item.ProductId}, Namn: {product?.Name}, Antal: {item.Amount}, Pris: {product?.Price} kr");
            }
        }

        public void AddCategory()
        {
            Console.Write("Ange kategorins namn: ");
            string? name = Console.ReadLine();

            var category = new Category { Name = name };

            dbContext.Categories.Add(category);
            dbContext.SaveChanges();

            Console.WriteLine($"Kategorin '{name}' har lagts till.");
        }

        public void AddSupplier()
        {
            Console.Write("Ange leverantörens namn: ");
            string? name = Console.ReadLine();

            var supplier = new Supplier { Name = name };

            dbContext.Suppliers.Add(supplier);
            dbContext.SaveChanges();

            Console.WriteLine($"Leverantören '{name}' har lagts till.");
        }

        public void AddProduct()
        {
            Console.WriteLine("Tillgängliga kategorier:");
            foreach (var category in dbContext.Categories)
            {
                Console.WriteLine($"{category.Id}: {category.Name}");
            }

            Console.Write("Ange kategori-ID: ");
            int categoryId = int.Parse(Console.ReadLine() ?? "0");

            Console.WriteLine("Tillgängliga leverantörer:");
            foreach (var supplier in dbContext.Suppliers)
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

            dbContext.Products.Add(product);
            dbContext.SaveChanges();

            Console.WriteLine("Produkten har lagts till i databasen.");
        }

        public void UpdateProduct()
        {
            Console.WriteLine("Befintliga produkter:");
            foreach (var product in dbContext.Products)
            {
                Console.WriteLine($"{product.Id}: {product.Name} - {product.Price} kr");
            }

            Console.Write("Ange ID för produkten du vill ändra: ");
            int productId = int.Parse(Console.ReadLine() ?? "0");

            var productToUpdate = dbContext.Products.Find(productId);
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
            foreach (var category in dbContext.Categories)
            {
                Console.WriteLine($"{category.Id}: {category.Name}");
            }

            Console.Write("Ange nytt kategori-ID (lämna tomt för att behålla): ");
            string? newCategoryIdInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(newCategoryIdInput))
            {
                int newCategoryId = int.Parse(newCategoryIdInput);
                if (dbContext.Categories.Any(c => c.Id == newCategoryId))
                {
                    productToUpdate.CategoryId = newCategoryId;
                }
            }

            Console.WriteLine("Tillgängliga leverantörer:");
            foreach (var supplier in dbContext.Suppliers)
            {
                Console.WriteLine($"{supplier.Id}: {supplier.Name}");
            }

            Console.Write("Ange nytt leverantörs-ID (lämna tomt för att behålla): ");
            string? newSupplierIdInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(newSupplierIdInput))
            {
                int newSupplierId = int.Parse(newSupplierIdInput);
                if (dbContext.Suppliers.Any(s => s.Id == newSupplierId))
                {
                    productToUpdate.SupplierId = newSupplierId;
                }
            }

            dbContext.SaveChanges();
            Console.WriteLine("Produkten har uppdaterats.");
        }

        public void DeleteProduct()
        {
            Console.WriteLine("Befintliga produkter:");
            foreach (var product in dbContext.Products)
            {
                Console.WriteLine($"{product.Id}: {product.Name} - {product.Price} kr");
            }

            Console.Write("Ange ID för produkten du vill ta bort: ");
            int productId = int.Parse(Console.ReadLine() ?? "0");

            var productToDelete = dbContext.Products.Find(productId);
            if (productToDelete == null)
            {
                Console.WriteLine("Produkten med det angivna ID:t hittades inte.");
                return;
            }

            dbContext.Products.Remove(productToDelete);
            dbContext.SaveChanges();
            Console.WriteLine($"Produkten '{productToDelete.Name}' har tagits bort.");
        }
        public void UpdateCustomerDetails()
        {
            ShowAllCustomers();

            Console.Write("Ange kundens ID: ");
            if (int.TryParse(Console.ReadLine(), out int customerId))
            {
                var customer = dbContext.Customers
                    .Include(c => c.Addresses)
                    .FirstOrDefault(c => c.Id == customerId);
                if (customer != null)
                {
                    Console.WriteLine($"Nuvarande namn: {customer.Name}");
                    Console.Write("Ange nytt namn (eller tryck Enter för att behålla nuvarande): ");
                    string newName = Console.ReadLine();
                    if (!string.IsNullOrEmpty(newName))
                    {
                        customer.Name = newName;
                    }

                    Console.WriteLine($"Nuvarande ålder: {customer.Age}");
                    Console.Write("Ange ny ålder (eller tryck Enter för att behålla nuvarande): ");
                    if (int.TryParse(Console.ReadLine(), out int newAge))
                    {
                        customer.Age = newAge;
                    }

                    Console.WriteLine($"Nuvarande telefonnummer: {customer.Phone}");
                    Console.Write("Ange nytt telefonnummer (eller tryck Enter för att behålla nuvarande): ");
                    string newPhone = Console.ReadLine();
                    if (!string.IsNullOrEmpty(newPhone))
                    {
                        customer.Phone = newPhone;
                    }

                    Console.WriteLine($"Nuvarande e-postadress: {customer.Email}");
                    Console.Write("Ange ny e-postadress (eller tryck Enter för att behålla nuvarande): ");
                    string newEmail = Console.ReadLine();
                    if (!string.IsNullOrEmpty(newEmail))
                    {
                        customer.Email = newEmail;
                    }

                    Console.WriteLine("\nNuvarande adresser:");
                    foreach (var address in customer.Addresses)
                    {
                        Console.WriteLine($"Adress-ID: {address.Id}, Gata: {address.Street}, Postnummer: {address.ZipCode}, Stad: {address.City}, Land: {address.Country}");
                    }

                    Console.Write("Vill du uppdatera en adress? (j/n): ");
                    if (Console.ReadLine()?.ToLower() == "j")
                    {
                        Console.Write("Ange adressens ID: ");
                        if (int.TryParse(Console.ReadLine(), out int addressId))
                        {
                            var address = customer.Addresses.FirstOrDefault(a => a.Id == addressId);
                            if (address != null)
                            {
                                Console.WriteLine($"Nuvarande gata: {address.Street}");
                                Console.Write("Ange ny gata (eller tryck Enter för att behålla nuvarande): ");
                                string newStreet = Console.ReadLine();
                                if (!string.IsNullOrEmpty(newStreet))
                                {
                                    address.Street = newStreet;
                                }

                                Console.WriteLine($"Nuvarande postnummer: {address.ZipCode}");
                                Console.Write("Ange nytt postnummer (eller tryck Enter för att behålla nuvarande): ");
                                string newZipCode = Console.ReadLine();
                                if (!string.IsNullOrEmpty(newZipCode))
                                {
                                    address.ZipCode = newZipCode;
                                }

                                Console.WriteLine($"Nuvarande stad: {address.City}");
                                Console.Write("Ange ny stad (eller tryck Enter för att behålla nuvarande): ");
                                string newCity = Console.ReadLine();
                                if (!string.IsNullOrEmpty(newCity))
                                {
                                    address.City = newCity;
                                }

                                Console.WriteLine($"Nuvarande land: {address.Country}");
                                Console.Write("Ange nytt land (eller tryck Enter för att behålla nuvarande): ");
                                string newCountry = Console.ReadLine();
                                if (!string.IsNullOrEmpty(newCountry))
                                {
                                    address.Country = newCountry;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Adress med angivet ID hittades inte.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ogiltigt adress-ID.");
                        }
                    }

                    dbContext.SaveChanges();
                    Console.WriteLine("Kunduppgifterna har uppdaterats.");
                }
                else
                {
                    Console.WriteLine("Kund med angivet ID hittades inte.");
                }
            }
            else
            {
                Console.WriteLine("Ogiltigt kund-ID.");
            }

            Console.WriteLine("Tryck på valfri tangent för att gå tillbaka.");
            Console.ReadKey();
        }
    }
}
