
namespace TestDynamicOdata
{

        public class Order
        {
            public int Id { get; set; }
            public decimal Amount { get; set; }
        }
   

        public class Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public System.Collections.Generic.List<Order> Orders { get; set; }
        }


}
