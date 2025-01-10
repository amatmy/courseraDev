using System;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualBasic;

namespace firstModule
{
    // There is an inventory
    // with products in it
    // for every product, there must be a name, a price, and a stock
    // the product belongs to the inventory and cannot exist without them
    // therefore, inventories must have a method to view, add, remove products
    // every product must have a method to update their info

    // Create inventory

    class Program
    {
        // TEST STARTS
        static void Main(string[] args)
        {
            Inventory inventory = new Inventory();
            // TEST 1: Then add a product with a name, price and stock
            Product product = new Product("Shoes", 99, 1);
            string resAddProduct = inventory.AddProduct(product);
            var productInfo = JsonSerializer.Deserialize<Dictionary<string, object>>(resAddProduct);

            if (productInfo != null)
            {
                Console.WriteLine(productInfo["id"]);
            }
            else
            {
                Console.WriteLine("Test 1 Failed");
            }

            // TEST 2: then view the inventory
            string resViewInventory = inventory.ViewInventory();
            if (!resViewInventory.Contains("empty"))
            {
                Console.WriteLine(resViewInventory);
            }
            else
            {
                Console.WriteLine("Test 2 Failed");
            }

            // TEST 3: then update the product information
            if (productInfo != null)
            {
                string resUpdateProduct = inventory.UpdateProduct(productInfo["id"].ToString(), "Boots", 129);
                if (resUpdateProduct.Contains("Boots"))
                {
                    Console.WriteLine(resUpdateProduct);
                }
                else
                {
                    Console.WriteLine("Test 3 Failed");
                }
            }

            // TEST 4: then view the inventory
            resViewInventory = inventory.ViewInventory();
            if (!resViewInventory.Contains("empty"))
            {
                Console.WriteLine(resViewInventory);
            }
            else
            {
                Console.WriteLine("Test 4 Failed");
            }

            // TEST 5: then remove the product

            if (productInfo != null)
            {
                string resRemoveProduct = inventory.RemoveProduct(productInfo["id"].ToString());
                if (resRemoveProduct.Contains("Removed"))
                {
                    Console.WriteLine(resRemoveProduct);
                }
                else
                {
                    Console.WriteLine("Test 5 Failed");
                }
            }

            // TEST 6: then view the inventory
            resViewInventory = inventory.ViewInventory();
            if (resViewInventory.Contains("empty"))
            {
                Console.WriteLine(resViewInventory);
            }
            else
            {
                Console.WriteLine("Test 6 Failed");
            }

        }
        // TEST FINISHES

    }

    class Inventory
    {
        public Dictionary<string, Product> inventory = new Dictionary<string, Product>();

        // Add to inventory and return a string with the info of the product
        public string AddProduct(Product product)
        {
            inventory[product.id] = product;
            Dictionary<string, object> res = new Dictionary<string, object> {
                { "id", product.id },
                { "name", product.name },
                { "price", product.price },
                { "stock", product.stock }
                };
            return JsonSerializer.Serialize(res);
        }

        // Remove from inventory and return a string with the info of product removed
        // if the product doesn't exist, inform this
        public string RemoveProduct(string id)
        {
            if (inventory.ContainsKey(id))
            {
                var removedProduct = inventory[id];
                inventory.Remove(id);
                return $"Removed {removedProduct.name}, {removedProduct.price}, {removedProduct.stock}";
            }
            else
            {
                return "Product not found";
            }
        }

        // Update a specific product from the inventory
        // return string with the new product info
        public string UpdateProduct(string id, string name, double price)
        {
            Product product = inventory[id];
            product.UpdateProduct(name, price);
            return $"{product.name}, {product.price}";
        }

        // view the inventory and return an array of objects
        // if nothing in the inventory, inform this
        public string ViewInventory()
        {
            if (inventory.Count == 0)
            {
                return "Inventory is empty";
            }

            var productList = new List<Dictionary<string, object>>();

            foreach (var product in inventory.Values)
            {
                var productInfo = new Dictionary<string, object>
                {
                    { "id", product.id },
                    { "name", product.name },
                    { "price", product.price },
                    { "stock", product.stock }
                };
                productList.Add(productInfo);
            }

            return JsonSerializer.Serialize(productList);
        }
    }

    class Product
    {
        public string id { get; } = Guid.NewGuid().ToString();
        public string name;
        public double price;
        public int stock;

        // Create a product with name, price and stock
        public Product(string name, double price, int stock)
        {
            this.name = name;
            this.price = price;
            this.stock = stock;
        }
        //  Update the info of the product, excluding the stock
        // Return the new information
        public void UpdateProduct(string name, double price)
        {
            this.name = name;
            this.price = price;
        }

        // Just add the amount that's going to be added or substracted
        // return the stock total
        public void UpdateStock(int stock)
        {
            this.stock += stock;
        }
    }

}