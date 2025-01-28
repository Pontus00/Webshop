using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Models;
using System.Data;

namespace Webshop
{
    internal class CustomerManager
    {
        private MyDbContext dbContext;
        private Order currentOrder;
        private string connectionString = "Server=tcp:dbpe.database.windows.net,1433;Initial Catalog=WebshopDb;Persist Security Info=False;User ID=pontus;Password=;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public CustomerManager()
        {
            dbContext = new MyDbContext();
            currentOrder = new Order();
            currentOrder.Items = new List<OrderItem>();
        }

        public void ShowCustomerPage()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\nVälkommen till kundsidan!");

                // Hämta och visa utvalda produkter
                ShowChosenProducts();

                // Visa och hantera kategorier
                ShowCategoriesAndHandleSelection();

                // Visa alternativ för att se kundvagnen eller söka efter produkter
                //Console.WriteLine("\nTryck 'K' för att se din kundvagn, 'S' för att söka efter produkter eller 'Q' för att avsluta.");
                //var input = Console.ReadKey();
                //if (input.Key == ConsoleKey.K)
                //{
                //    ShowCart();
                //}
                //else if (input.Key == ConsoleKey.S)
                //{
                //    SearchProducts();
                //}
                //else if (input.Key == ConsoleKey.Q)
                //{
                //    break;
                //}

            }
        }


        private void ShowChosenProducts()
        {
            Console.WriteLine("\nUtvalda produkter:");
            var chosenProducts = dbContext.Products.Where(p => p.IsChosen).Take(3).ToList();

            if (chosenProducts.Any())
            {
                DisplayChosenProducts(chosenProducts);
            }
            else
            {
                Console.WriteLine("Inga utvalda produkter tillgängliga.");
            }
        }

        private void ShowCategoriesAndHandleSelection()
        {
            Console.WriteLine("\nTillgängliga kategorier:");
            var categories = dbContext.Categories.ToList();

            if (!categories.Any())
            {
                Console.WriteLine("Inga kategorier tillgängliga.");
                return;
            }

            for (int i = 0; i < categories.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {categories[i].Name}");
            }

            Console.Write("\nVälj en kategori (ange siffra eller 0 för att gå tillbaka).");
            Console.Write("\nTryck 'K' för att visa Kundvagnen eller 'S' för att söka på en produkt eller tryck 'B' för att se bäst säljande vara, 'L' för att se minst lager eller 'P' för att se populäraste kategorin: ");
            var input = Console.ReadLine();

            if (int.TryParse(input, out int categoryChoice) && categoryChoice > 0 && categoryChoice <= categories.Count)
            {
                var selectedCategory = categories[categoryChoice - 1];
                ShowProductsInCategory(selectedCategory.Id);
            }
            else if (input.ToLower() == "k")
            {
                ShowCart();
            }
            else if (input.ToLower() == "s")
            {
                SearchProducts();
            }
            else if (input.ToLower() == "b")
            {
                ShowBestSellingProduct();
            }
            else if (input.ToLower() == "l")
            {
                ShowLeastStockedProducts();
            }
            else if (input.ToLower() == "p")
            {
                ShowMostPopularCategory();
            }

            else
            {
                Console.WriteLine("Ogiltigt val. Återgår till huvudmenyn.");
            }

        }

        private void ShowProductsInCategory(int categoryId)
        {
            Console.Clear();
            var products = dbContext.Products.Where(p => p.CategoryId == categoryId).ToList();

            if (!products.Any())
            {
                Console.WriteLine("Inga produkter hittades i denna kategori.");
                return;
            }

            Console.WriteLine("Produkter i kategorin:");
            for (int i = 0; i < products.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {products[i].Name}");
            }

            Console.Write("\nVälj en produkt för mer information (ange siffra eller 0 för att gå tillbaka): ");
            if (int.TryParse(Console.ReadLine(), out int productChoice) && productChoice > 0 && productChoice <= products.Count)
            {
                ShowProductDetails(products[productChoice - 1]);
            }
            else
            {
                Console.WriteLine("Ogiltigt val. Återgår till kategorilistan.");
            }
        }

        private void ShowProductDetails(Product product)
        {
            Console.Clear();

            // Hämta leverantörsnamn baserat på SupplierId
            var supplier = dbContext.Suppliers.FirstOrDefault(s => s.Id == product.SupplierId);
            string supplierName = supplier?.Name ?? "Okänd leverantör";

            // Visa produktdetaljer
            Console.WriteLine("Produktdetaljer:");
            Console.WriteLine($"Namn: {product.Name}");
            Console.WriteLine($"Beskrivning: {product.Description}");
            Console.WriteLine($"Pris: {product.Price} kr");
            Console.WriteLine($"Färg: {product.Color}");
            Console.WriteLine($"Lagerstatus: {product.Stock} i lager");
            Console.WriteLine($"Leverantör: {supplierName}");

            Console.Write("\nVill du lägga till denna produkt i kundvagnen? (j/n): ");
            var input = Console.ReadLine();
            if (input?.ToLower() == "j")
            {
                AddProductToCart(product.Id);
                Console.WriteLine("Produkten har lagts till i kundvagnen.");
            }

            Console.WriteLine("\nTryck på valfri tangent för att gå tillbaka.");
            Console.ReadKey();
        }

        private void AddProductToCart(int productId, int amount = 1)
        {
            var orderItem = currentOrder.Items?.FirstOrDefault(oi => oi.ProductId == productId);
            if (orderItem != null)
            {
                orderItem.Amount += amount;
            }
            else
            {
                orderItem = new OrderItem
                {
                    ProductId = productId,
                    Amount = amount
                };
                currentOrder.Items?.Add(orderItem);
            }
        }

        private void ChangeAmountOfProductInCart(int productId, int amount)
        {
            var orderItem = currentOrder.Items?.FirstOrDefault(oi => oi.ProductId == productId);
            if (orderItem != null)
            {
                orderItem.Amount = amount;
            }
        }
        private void ShowCart()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\nKundvagn:");
                var cartItems = currentOrder.Items?.ToList();

                if (cartItems?.Count == 0)
                {
                    Console.WriteLine("Kundvagnen är tom.");
                    break;
                }

                decimal totalPrice = 0;

                for (int i = 0; i < cartItems?.Count; i++)
                {
                    var item = cartItems[i];
                    var product = dbContext.Products.FirstOrDefault(p => p.Id == item.ProductId);
                    if (product != null)
                    {
                        totalPrice += product.Price * item.Amount;
                        Console.WriteLine($"{i + 1}. Produkt: {product.Name}, Antal: {item.Amount}, Pris: {product.Price} kr");
                    }
                    Console.WriteLine("-----------------------------------------");
                    Console.WriteLine($"Summa: {Math.Round(totalPrice, 2)} kr");
                }

                Console.WriteLine("\nAnge numret på produkten du vill ta bort, 'A' för att lägga till en produkt, 'Ä' för att ändra antal, 'O' för att skapa en order eller 0 för att gå tillbaka:");
                var input = Console.ReadLine();
                if (int.TryParse(input, out int choice) && choice > 0 && choice <= cartItems?.Count)
                {
                    RemoveProductFromCart(cartItems[choice - 1].Id);
                    Console.WriteLine("Produkten har tagits bort från kundvagnen.");
                }
                else if (input?.ToLower() == "a")
                {
                    Console.Write("\nAnge numret på produkten att lägga till: ");
                    input = Console.ReadLine();
                    if (int.TryParse(input, out int addChoice) && addChoice > 0 && addChoice <= cartItems?.Count)
                    {
                        Console.Write("\nAnge antal att lägga till: ");
                        if (int.TryParse(Console.ReadLine(), out int amount))
                        {
                            AddProductToCart(cartItems[addChoice - 1].ProductId, amount);
                            Console.WriteLine("Produkten har lagts till i kundvagnen.");
                        }
                        else
                        {
                            Console.WriteLine("Ogiltig mängd. Försök igen.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ogiltigt produkt-ID. Försök igen.");
                    }
                }
                else if (input?.ToLower() == "ä")
                {
                    Console.Write("\nAnge nummret på produkten för att ändra antal: ");
                    input = Console.ReadLine();
                    if (int.TryParse(input, out int changeChoice) && changeChoice > 0 && changeChoice <= cartItems?.Count)
                    {
                        Console.Write("\nAnge antal: ");
                        if (int.TryParse(Console.ReadLine(), out int amount))
                        {
                            ChangeAmountOfProductInCart(cartItems[changeChoice - 1].ProductId, amount);
                            Console.WriteLine($"Antalet har ändrats till {amount} st.");
                        }
                        else
                        {
                            Console.WriteLine("Ogiltigt antal. Försök igen.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ogiltigt produkt-ID. Försök igen.");
                    }
                }
                else if (input?.ToLower() == "o")
                {
                    Checkout();
                    break;
                }
                else if (choice == 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Ogiltigt val. Försök igen.");
                }
            }

            Console.WriteLine("\nTryck på valfri tangent för att gå tillbaka.");
            Console.ReadKey();
        }

        private void Checkout()
        {
            Console.Clear();
            Console.WriteLine("\nUtcheckning:");

            // Skapa ny kund
            var customer = CreateCustomer();

            Console.Write("Ange betalningsmetod (1: Kreditkort, 2: PayPal, 3: Faktura): ");
            PaymentMethod paymentMethod = (PaymentMethod)int.Parse(Console.ReadLine());

            Console.Write("Ange fraktmetod (1: Standard, 2: Express): ");
            ShippingMethod shippingMethod = (ShippingMethod)int.Parse(Console.ReadLine());

            decimal shippingPrice = shippingMethod == ShippingMethod.Budget ? 0 : 50;

            decimal totalPrice = shippingPrice;
            foreach (var item in currentOrder.Items)
            {
                var product = dbContext.Products.Find(item.ProductId);
                product.Stock -= item.Amount;
                dbContext.SaveChanges();
                totalPrice += product.Price * item.Amount;
            }
            decimal vat = totalPrice * 0.25M;

            totalPrice = Math.Round(totalPrice, 2);

            var order = CreateOrder(customer.Id, paymentMethod, shippingMethod, shippingPrice, vat, totalPrice);
            ShowOrderDetails(order);
        }

        private Customer CreateCustomer()
        {
            var customer = new Customer();
            customer.Addresses = new List<Address>();

            Console.Write("Ange namn: ");
            string name = Console.ReadLine();

            Console.Write("Ange adress: ");
            string street = Console.ReadLine();
            Console.Write("Ange postnummer: ");
            string zipCode = Console.ReadLine();
            Console.Write("Ange stad: ");
            string city = Console.ReadLine();
            Console.Write("Ange land: ");
            string country = Console.ReadLine();

            customer.Addresses.Add(
                new Address()
                {
                    Street = street,
                    ZipCode = zipCode,
                    City = city,
                    Country = country,
                    Type = AddressType.Customer
                }
            );

            Console.WriteLine("Vill du ange en annan leveransadress? (J/N)");
            var answer = Console.ReadKey();
            if (answer.Key == ConsoleKey.J)
            {
                Console.Write("Ange adress: ");
                string deliverystreet = Console.ReadLine();
                Console.Write("Ange postnummer: ");
                string deliveryzipCode = Console.ReadLine();
                Console.Write("Ange stad: ");
                string deliverycity = Console.ReadLine();
                Console.Write("Ange land: ");
                string deliverycountry = Console.ReadLine();
                customer.Addresses.Add(
                new Address()
                {
                    Street = deliverystreet,
                    ZipCode = deliveryzipCode,
                    City = deliverycity,
                    Country = deliverycountry,
                    Type = AddressType.Delivery
                }
            );
            }

            Console.Write("Ange födelseår: ");
            int birthYear = int.Parse(Console.ReadLine());
            int age = DateTime.Now.Year - birthYear;

            Console.Write("Ange kundens telefonnummer: ");
            string phone = Console.ReadLine();

            Console.Write("Ange kundens e-postadress: ");
            string email = Console.ReadLine();


            customer.Name = name;
            customer.Age = age;
            customer.Phone = phone;
            customer.Email = email;

            dbContext.Customers.Add(customer);
            dbContext.SaveChanges();

            return customer;
        }

        private Order CreateOrder(int customerId, PaymentMethod paymentMethod, ShippingMethod shippingMethod, decimal shippingPrice, decimal vat, decimal totalPrice)
        {
            var cartItems = currentOrder.Items?.ToList();
            if (cartItems?.Count == 0)
            {
                Console.WriteLine("Kundvagnen är tom. Kan inte skapa en order.");
                return null;
            }

            var order = new Order
            {
                OrderDate = DateTime.Now,
                CustomerId = customerId,
                PaymentMethod = paymentMethod,
                ShippingMethod = shippingMethod,
                ShippingPrice = shippingPrice,
                VAT = vat,
                TotalPrice = totalPrice,
                Items = cartItems
            };

            dbContext.Orders.Add(order);
            dbContext.SaveChanges();

            // Rensa kundvagnen
            currentOrder.Items?.Clear();

            Console.WriteLine("Ordern har skapats.");
            return order;
        }

        private void ShowOrderDetails(Order order)
        {
            if (order == null)
            {
                Console.WriteLine("Ingen order att visa.");
                return;
            }

            Console.Clear();
            Console.WriteLine("Orderdetaljer:");
            Console.WriteLine($"Order-ID: {order.Id}");
            Console.WriteLine($"Orderdatum: {order.OrderDate}");
            Console.WriteLine($"Kund-ID: {order.CustomerId}");
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

            Console.WriteLine("\nTryck på valfri tangent för att gå tillbaka.");
            Console.ReadKey();
        }

        private void RemoveProductFromCart(int orderItemId)
        {
            var orderItem = currentOrder.Items?.FirstOrDefault(oi => oi.Id == orderItemId);
            if (orderItem != null)
            {
                currentOrder.Items?.Remove(orderItem);
            }
        }
        private void DisplayChosenProducts(List<Product> products)
        {
            var productBoxes = products.Select(p =>
            {
                var name = $"Namn: {p.Name}";
                var price = $"Pris: {p.Price} kr";
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

            for (int i = 0; i < productBoxes[0].Length; i++)
            {
                Console.WriteLine(string.Join("   ", productBoxes.Select(box => box[i])));
            }
        }
        private void SearchProducts()
        {
            Console.Write("\nAnge sökterm: ");
            string searchTerm = Console.ReadLine();

            var searchResults = dbContext.Products
                .Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
                .ToList();

            if (!searchResults.Any())
            {
                Console.WriteLine("Inga produkter matchade din sökning.");
                return;
            }

            Console.WriteLine("\nSökresultat:");
            for (int i = 0; i < searchResults.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {searchResults[i].Name}");
            }

            Console.Write("\nVälj en produkt för mer information (ange siffra eller 0 för att gå tillbaka): ");
            if (int.TryParse(Console.ReadLine(), out int productChoice) && productChoice > 0 && productChoice <= searchResults.Count)
            {
                ShowProductDetails(searchResults[productChoice - 1]);
            }
            else
            {
                Console.WriteLine("Ogiltigt val. Återgår till huvudmenyn.");
            }
        }
        public void ShowBestSellingProduct()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT TOP 5 p.Id, p.Name, SUM(oi.Amount) AS TotalSold
                FROM Products p
                JOIN OrderItems oi ON p.Id = oi.ProductId
                GROUP BY p.Id, p.Name
                ORDER BY TotalSold DESC";

                var bestSellingProduct = connection.QueryFirstOrDefault(query, new { }, commandType: CommandType.Text);

                if (bestSellingProduct != null)
                {
                    Console.WriteLine("Bäst säljande produkt:");
                    Console.WriteLine($"Namn: {bestSellingProduct.Name}");
                    Console.WriteLine($"Totalt sålda enheter: {bestSellingProduct.TotalSold}");
                }
                else
                {
                    Console.WriteLine("Ingen produkt hittades.");
                    

                }
                Console.ReadKey();
            }
        }
        public void ShowMostPopularCategory()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = @"
        SELECT TOP 5 c.Id, c.Name, COUNT(oi.ProductId) AS TotalSold
        FROM Categories c
        JOIN Products p ON c.Id = p.CategoryId
        JOIN OrderItems oi ON p.Id = oi.ProductId
        GROUP BY c.Id, c.Name
        ORDER BY TotalSold DESC";

                var mostPopularCategory = connection.QueryFirstOrDefault(query, new { }, commandType: CommandType.Text);

                if (mostPopularCategory != null)
                {
                    Console.WriteLine("Populäraste kategorin:");
                    Console.WriteLine($"Namn: {mostPopularCategory.Name}");
                    Console.WriteLine($"Totalt sålda produkter: {mostPopularCategory.TotalSold}");
                }
                else
                {
                    Console.WriteLine("Ingen kategori hittades.");
                }
                Console.ReadKey();
            }
        }
        public void ShowLeastStockedProducts()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string query = @"
                SELECT TOP 5 p.Id, p.Name, p.Stock
                FROM Products p
                ORDER BY p.Stock ASC";

                    var leastStockedProducts = connection.Query<Product>(query).ToList();

                    if (leastStockedProducts.Any())
                    {
                        Console.WriteLine("Produkter med minst lager:");
                        foreach (var product in leastStockedProducts)
                        {
                            Console.WriteLine($"Namn: {product.Name}, Lager: {product.Stock}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Inga produkter hittades.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ett fel inträffade när produkter med minst lager skulle hämtas:");
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("\nTryck på valfri tangent för att gå tillbaka.");
            Console.ReadKey();
        }

    }
}
