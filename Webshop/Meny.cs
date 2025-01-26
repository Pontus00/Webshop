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
        private MyDbContext dbContext;
        private Order currentOrder;

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

                    // Visa alternativ för att se kundvagnen
                    Console.WriteLine("\nTryck 'K' för att se din kundvagn eller 'Q' för att avsluta.");
                    var input = Console.ReadKey();
                    if (input.Key == ConsoleKey.K)
                    {
                        ShowCart();
                    }
                    else if (input.Key == ConsoleKey.Q)
                    {
                        break;
                    }
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

            Console.Write("\nVälj en kategori (ange siffra eller 0 för att gå tillbaka): ");
            if (int.TryParse(Console.ReadLine(), out int categoryChoice) && categoryChoice > 0 && categoryChoice <= categories.Count)
            {
                var selectedCategory = categories[categoryChoice - 1];
                ShowProductsInCategory(selectedCategory.Id);
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

                for (int i = 0; i < cartItems?.Count; i++)
                {
                    var item = cartItems[i];
                    var product = dbContext.Products.FirstOrDefault(p => p.Id == item.ProductId);
                    if (product != null)
                    {
                        Console.WriteLine($"{i + 1}. Produkt: {product.Name}, Antal: {item.Amount}, Pris: {product.Price} kr");
                    }
                }

                Console.WriteLine("\nAnge numret på produkten du vill ta bort, 'A' för att lägga till en produkt, 'O' för att skapa en order eller 0 för att gå tillbaka:");
                var input = Console.ReadLine();
                if (int.TryParse(input, out int choice) && choice > 0 && choice <= cartItems?.Count)
                {
                    RemoveProductFromCart(cartItems[choice - 1].Id);
                    Console.WriteLine("Produkten har tagits bort från kundvagnen.");
                }
                else if (input?.ToLower() == "a")
                {
                    Console.Write("\nAnge produkt-ID att lägga till: ");
                    if (int.TryParse(Console.ReadLine(), out int productId))
                    {
                        Console.Write("\nAnge mängd att lägga till: ");
                        if (int.TryParse(Console.ReadLine(), out int amount))
                        {
                            AddProductToCart(productId, amount);
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

            Console.Write("Ange fraktpris: ");
            decimal shippingPrice = decimal.Parse(Console.ReadLine());

            Console.Write("Ange moms: ");
            decimal vat = decimal.Parse(Console.ReadLine());

            Console.Write("Ange totalpris: ");
            decimal totalPrice = decimal.Parse(Console.ReadLine());

            var order = CreateOrder(customer.Id, paymentMethod, shippingMethod, shippingPrice, vat, totalPrice);
            ShowOrderDetails(order);
        }

        private Customer CreateCustomer()
        {
            Console.Write("Ange kundens namn: ");
            string name = Console.ReadLine();

            Console.Write("Ange kundens ålder: ");
            int age = int.Parse(Console.ReadLine());

            Console.Write("Ange kundens telefonnummer: ");
            string phone = Console.ReadLine();

            Console.Write("Ange kundens e-postadress: ");
            string email = Console.ReadLine();

            var customer = new Customer
            {
                Name = name,
                Age = age,
                Phone = phone,
                Email = email
            };

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

            //foreach (var item in cartItems)
            //{
            //    item.OrderId = order.Id;
            //    order.Items.Add(item);
            //}

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
    }

}
