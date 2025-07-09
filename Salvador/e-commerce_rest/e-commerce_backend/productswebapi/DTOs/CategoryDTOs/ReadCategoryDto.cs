namespace ProductsWebApi.DTOs.CategoryDTOs
{

    public class ReadCategoryDto
    {

        public string? Id { get; set; } //ORIGINAL int?
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? FatherCategoryId { get; set; }
        public DateTime? DateCreated { get; set; }

    }

}