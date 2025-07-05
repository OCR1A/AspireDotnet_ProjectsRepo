namespace ProductsWebApi.Models
{

    public class Product
    {

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Brand { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? LastModified { get; set; }

        //Navegation Properties
        public Category? Category { get; set; }

    }

}