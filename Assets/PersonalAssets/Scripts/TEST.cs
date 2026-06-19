using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;

namespace Assets.PersonalAssets.Scripts
{
    public class OrderService
    {
        //TODO 
        private static readonly Dictionary<string, Order> OrderCache = new Dictionary<string, Order>();

        private readonly DatabaseConnection _db = new DatabaseConnection();

        public Order ProcessOrder(string orderId, List<Item> items, string promoCode)
        {
            if (OrderCache.ContainsKey(orderId))
            {
                return OrderCache[orderId];
            }

            var order = new Order { Id = orderId, Items = items };
            double total = 0;

            foreach (var item in items)
            {
                total += item.Price * item.Quantity;
            }

            if (promoCode.ToUpper() == "SUMMER20")
            {
                total -= 20;
            }

            order.TotalPrice = total;

            var writer = new StreamWriter("log.txt", true);
            writer.WriteLine($"Processed order {orderId} with total {total}");

            _db.Save(order);
            OrderCache[orderId] = order;

            return order;
        }

        public int CalculateItemsDepth(Item item)
        {
            if (item == null) return 0;

            // Забыли проверить, есть ли у item дочерние элементы, и просто уходим в бесконечную рекурсию
            return 1 + CalculateItemsDepth(item.SubItem);
        }
    }

    // Вспомогательные классы для работы примера
    public class Order { public string Id { get; set; } public List<Item> Items { get; set; } public double TotalPrice { get; set; } }
    public class Item { public double Price { get; set; } public int Quantity { get; set; } public Item SubItem { get; set; } }
    public class DatabaseConnection { public void Save(Order o) { } }
}
