using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using HashIdManager.Services;
using Microsoft.AspNetCore.Mvc;
using ProductsWebApi.DTOs.CategoryDTOs;
using ProductsWebApi.Services;

namespace ProductsWebApi.Controllers
{

    [ApiController]
    [Route("/api/[controller]")]
    public class CategoryController : ControllerBase
    {

        //Dependency injection
        private readonly CategoryService _service;
        private readonly HashIdService _hashidservice;

        public CategoryController(CategoryService service, HashIdService hashidservice)
        {
            _service = service;
            _hashidservice = hashidservice;
        }

        //Crud operations for categories
        //Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
        {

            CategoryResult result = await _service.CreateCategory(dto);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);

        }

        //Read
        [HttpGet]
        public async Task<IActionResult> Read
        (
            [FromQuery] ReadCategoryDto dto,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5
        )
        {

            //Pagination inputs validation
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 5;

            var result = await _service.ReadCategories(dto, page, pageSize);

            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result);

        }

        //Update
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateCategoryDto dto)
        {

            CategoryResult result = await _service.UpdateCategory(id, dto);
            Console.WriteLine($"[DEBUG] {result.Success}");

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);

        }

        //Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {

            CategoryResult result = await _service.DeleteCategory(id);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);

        }

    }

}