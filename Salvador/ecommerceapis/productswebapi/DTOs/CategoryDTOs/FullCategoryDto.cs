namespace ProductsWebApi.DTOs.CategoryDTOs
{

    public class FullCategoryDto
    {

        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? FatherCategoryId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? LastModified { get; set; }

    }

}