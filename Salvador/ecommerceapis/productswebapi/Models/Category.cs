namespace ProductsWebApi.Models
{

    public class Category
    {

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? FatherCategoryId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? LastModified { get; set; }

    }

}