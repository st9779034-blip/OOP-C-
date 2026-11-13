using System;
using System.Collections.Generic;

namespace DeliveryApp
{
    public interface IPayment
    {
        void Pay(double amount);
    }

    public class CardPayment : IPayment
    {
        public void Pay(double amount)
        {
            Console.WriteLine($"Оплата карткою успішно проведена. Сума: {amount} грн.");
        }
    }

    public class CashPayment : IPayment
    {
        public void Pay(double amount)
        {
            Console.WriteLine($"Оплата готівкою кур'єру. Сума: {amount} грн.");
        }
    }

    public class Order
    {
        public int Number { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public double Distance { get; set; }
        public Transport AssignedTransport { get; set; }

        public Order(int number, string description, double weight, double distance)
        {
            Number = number;
            Description = description;
            Weight = weight;
            Distance = distance;
        }

        public override string ToString()
        {
            string transportInfo = AssignedTransport != null ? AssignedTransport.Name : "не призначено";
            return $"Замовлення #{Number} ({Description}) | Вага: {Weight} кг | Відстань: {Distance} км | Транспорт: {transportInfo}";
        }
    }

    public abstract class Transport
    {
        public string Name { get; set; }
        public double MaxWeight { get; set; }
        public bool IsBusy { get; set; }

        public Transport(string name, double maxWeight)
        {
            Name = name;
            MaxWeight = maxWeight;
            IsBusy = false;
        }

        public virtual bool CanTake(Order order)
        {
            return order.Weight <= MaxWeight;
        }

        public abstract double CalculateCost(Order order);
    }

    public class Bike : Transport
    {
        public Bike(string name, double maxWeight) : base(name, maxWeight)
        {
        }

        public override bool CanTake(Order order)
        {
            return base.CanTake(order) && order.Distance <= 10;
        }

        public override double CalculateCost(Order order)
        {
            return 20 + 5 * order.Distance;
        }
    }

    public class Car : Transport
    {
        public Car(string name, double maxWeight) : base(name, maxWeight)
        {
        }

        public override double CalculateCost(Order order)
        {
            return 50 + 8 * order.Distance + 1 * order.Weight;
        }
    }

    public class Truck : Transport
    {
        public Truck(string name, double maxWeight) : base(name, maxWeight)
        {
        }

        public override double CalculateCost(Order order)
        {
            return 200 + 15 * order.Distance + 0.5 * order.Weight;
        }
    }

    public class DeliveryService
    {
        private List<Transport> transports = new List<Transport>();
        private List<Order> activeOrders = new List<Order>();
        private List<Order> deliveryHistory = new List<Order>();

        public void AddTransport(Transport transport)
        {
            transports.Add(transport);
        }

        public void CreateOrder(Order order)
        {
            activeOrders.Add(order);
            Console.WriteLine($"Створено: {order}");
        }

        public void ShowDeliveryOptions(Order order)
        {
            Console.WriteLine($"\nДоступний транспорт та вартість для замовлення #{order.Number}:");
            bool found = false;

            foreach (var transport in transports)
            {
                if (transport.CanTake(order))
                {
                    double cost = transport.CalculateCost(order);
                    string status = transport.IsBusy ? "зайнятий" : "вільний";
                    Console.WriteLine($"- {transport.Name} ({status}): {cost} грн");
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("Немає транспорту, який може виконати це замовлення.");
            }
        }

        public void AssignTransport(Order order, Transport transport)
        {
            Console.WriteLine($"\nСпроба призначити {transport.Name} на замовлення #{order.Number}:");

            if (!activeOrders.Contains(order))
            {
                Console.WriteLine("Помилка: замовлення не знайдено серед активних.");
                return;
            }

            if (transport.IsBusy)
            {
                Console.WriteLine($"Помилка: {transport.Name} зараз зайнятий.");
                return;
            }

            if (!transport.CanTake(order))
            {
                Console.WriteLine($"Помилка: {transport.Name} не підходить за вагою або відстанню.");
                return;
            }

            order.AssignedTransport = transport;
            transport.IsBusy = true;
            Console.WriteLine($"Успішно: {transport.Name} призначено на замовлення #{order.Number}.");
        }

        public void DeliverOrder(Order order, IPayment paymentMethod)
        {
            Console.WriteLine($"\nДоставка замовлення #{order.Number}:");

            if (!activeOrders.Contains(order))
            {
                Console.WriteLine("Помилка: замовлення не є активним.");
                return;
            }

            if (order.AssignedTransport == null)
            {
                Console.WriteLine("Помилка: на замовлення ще не призначено транспорт.");
                return;
            }

            double cost = order.AssignedTransport.CalculateCost(order);
            paymentMethod.Pay(cost);

            order.AssignedTransport.IsBusy = false;
            activeOrders.Remove(order);
            deliveryHistory.Add(order);

            Console.WriteLine($"Замовлення #{order.Number} доставлено. Транспорт {order.AssignedTransport.Name} тепер вільний.");
        }

        public void ShowActiveOrders()
        {
            Console.WriteLine("\n--- Активні замовлення ---");
            if (activeOrders.Count == 0)
            {
                Console.WriteLine("Активних замовлень немає.");
                return;
            }

            foreach (var order in activeOrders)
            {
                Console.WriteLine(order);
            }
        }

        public void ShowDeliveryHistory()
        {
            Console.WriteLine("\n--- Історія доставок ---");
            if (deliveryHistory.Count == 0)
            {
                Console.WriteLine("Історія доставок порожня.");
                return;
            }

            foreach (var order in deliveryHistory)
            {
                Console.WriteLine(order);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            DeliveryService service = new DeliveryService();

            Transport bike = new Bike("Велосипед", 15);
            Transport car = new Car("Автомобіль", 300);
            Transport truck = new Truck("Вантажівка", 2000);

            service.AddTransport(bike);
            service.AddTransport(car);
            service.AddTransport(truck);

            Order order1 = new Order(1, "Піца та напої", 5, 4);
            Order order2 = new Order(2, "Ноутбук", 3, 15);
            Order order3 = new Order(3, "Холодильник", 85, 12);

            service.CreateOrder(order1);
            service.CreateOrder(order2);
            service.CreateOrder(order3);

            service.ShowDeliveryOptions(order1);
            service.ShowDeliveryOptions(order2);
            service.ShowDeliveryOptions(order3);

            service.AssignTransport(order1, bike);
            service.AssignTransport(order2, bike);
            service.AssignTransport(order2, car);
            service.AssignTransport(order3, car);
            service.AssignTransport(order3, truck);

            service.ShowActiveOrders();
            service.ShowDeliveryHistory();

            IPayment card = new CardPayment();
            IPayment cash = new CashPayment();

            service.DeliverOrder(order1, card);
            service.DeliverOrder(order2, cash);

            Order order4 = new Order(4, "Документи", 1, 3);
            service.CreateOrder(order4);
            service.AssignTransport(order4, bike);

            service.ShowActiveOrders();
            service.ShowDeliveryHistory();
        }
    }
}