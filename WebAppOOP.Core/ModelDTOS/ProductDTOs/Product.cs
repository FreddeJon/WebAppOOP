namespace WebAppOOP.Core.ModelDTOS.ProductDTOs
{
    public class Product
    {
        public int Id { get; set; }
        public ProductType ProductType { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string IMGUrl { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal CurrentPrice { get; set; }

        public decimal GetCost()
        {
            if (Quantity > 0)
            {
                return (Quantity * Price);
            }
            return Price;
        }
        public string GetTotalCostToString()
        {
            if (Quantity > 0)
            {
                return $"{(Price * Quantity):C}";
            }
            return $"{Price:C}";
        }
    }
    public enum ProductType
    {
        UNDEFINED,
        DIGITAL,
        PHYSICAL
    }
}
