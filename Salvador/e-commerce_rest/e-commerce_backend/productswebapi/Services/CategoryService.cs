using HashIdManager.Services;
using Microsoft.EntityFrameworkCore;
using ProductsWebApi.Data;
using ProductsWebApi.DTOs.CategoryDTOs;
using ProductsWebApi.Models;

namespace ProductsWebApi.Services
{

    public class CategoryService
    {

        //Dependency Injection
        public ProductsWebApiDbContext _dbcontext;
        private readonly HashIdService _hashidservice;
        public CategoryService(ProductsWebApiDbContext dbcontext, HashIdService hashidservice)
        {
            _dbcontext = dbcontext;
            _hashidservice = hashidservice;
        }

        //CRUD Methods for categories
        //1.-C:Create
        public async Task<CategoryResult> CreateCategory(CreateCategoryDto dto)
        {

            //Create new category object
            Category newCategory = new Category()
            {

                Name = dto.Name,
                Description = dto.Description,
                FatherCategoryId = dto.FatherCategoryId,
                DateCreated = DateTime.Today

            };

            //Search for coincidences on the database and return unsuccesfull operation if coincidence is found
            bool sameCategoryFound = await _dbcontext.Categories
                                        .AnyAsync(c => c.Name == dto.Name || c.Description == dto.Description);

            if (sameCategoryFound)
            {
                return new CategoryResult
                {
                    Categories = null,
                    ErrorMessage = "Category with same name or description found",
                    Success = false,
                    OperationDate = DateTime.Today
                };
            }

            //Add changes to databse if category is not yet stored at the Database
            await _dbcontext.AddAsync(newCategory);
            await _dbcontext.SaveChangesAsync();

            FullCategoryDto answerDto = new FullCategoryDto()
            {
                Id = _hashidservice.Encode(newCategory.Id),
                Name = newCategory.Name,
                Description = newCategory.Description,
                FatherCategoryId = _hashidservice.Encode(Convert.ToInt32(newCategory.FatherCategoryId)),
                DateCreated = newCategory.DateCreated,
                LastModified = newCategory.LastModified
            };

            //Return Success
            return new CategoryResult
            {
                Categories = [answerDto],
                ErrorMessage = null,
                Success = true,
                OperationDate = newCategory.DateCreated
            };

        }

        //2.-R:Read
        public async Task<CategoryResult> ReadCategories(ReadCategoryDto dto, int page = 1, int pageSize = 5)
        {

            //Request given DTO attributes
            var dtoAttributes = dto.GetType()
                                    .GetProperties()
                                    .ToDictionary(p => p.Name, p => p.GetValue(dto));

            //Dictionary to store request not null attributes
            Dictionary<string, object> dtoNotNullAttributes = new Dictionary<string, object>();

            //List to store category query result
            List<Category>? desiredCategories = new List<Category>();

            // Decode HASH Id to int
            if (dtoAttributes.ContainsKey("Id"))
            {
                string? rawId = Convert.ToString(dtoAttributes["Id"]);
                int? decodedId = _hashidservice.Decode(rawId!);

                //Stop program if Hash format is incorrect
                if (!string.IsNullOrEmpty(rawId) && decodedId == null)
                {
                    return new CategoryResult
                    {
                        Categories = null,
                        ErrorMessage = $"Invalid hash ID: {rawId}",
                        Success = false,
                        OperationDate = DateTime.Now
                    };
                }

                dtoAttributes["Id"] = decodedId;
                Console.WriteLine($"Decoded ID: {decodedId}");
            }
            if (dtoAttributes.ContainsKey("FatherCategoryId"))
            {
                string? rawFatherId = Convert.ToString(dtoAttributes["FatherCategoryId"]);
                int? decodedFatherId = _hashidservice.Decode(rawFatherId!);

                //Stop program if Hash format is incorrect
                if (!string.IsNullOrEmpty(rawFatherId) && decodedFatherId == null)
                {
                    return new CategoryResult
                    {
                        Categories = null,
                        ErrorMessage = $"Invalid hash FatherCategoryId: {rawFatherId}",
                        Success = false,
                        OperationDate = DateTime.Now
                    };
                }

                dtoAttributes["FatherCategoryId"] = decodedFatherId;
                Console.WriteLine($"Decoded Father Category Id: {decodedFatherId}");
            }


            //Filtering not null values
            foreach (var key in dtoAttributes.Keys)
            {
                if (dtoAttributes[key] != null)
                {
                    dtoNotNullAttributes.Add(key, dtoAttributes[key]!);
                }
            }

            //Build query to find exact coincidences for each not null request attributes
            var query = _dbcontext.Categories.AsQueryable();

            foreach (var attribute in dtoNotNullAttributes)
            {
                var propertyName = attribute.Key;
                var propertyValue = attribute.Value;
                query = query.Where(c => EF.Property<object>(c, propertyName).Equals(propertyValue));
            }

            //Query result pagination
            desiredCategories = await query.Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            //Encode int id to hash
            List<FullCategoryDto> answerDto = new List<FullCategoryDto>();
            int currentFatherCategoryId;
            string? currentFatherCategoryIdHashed;

            for (int i = 0; i < desiredCategories.Count; i++)
            {

                currentFatherCategoryId = desiredCategories[i].FatherCategoryId == null ? -1 : Convert.ToInt32(desiredCategories[i].FatherCategoryId);

                if (currentFatherCategoryId == -1)
                {
                    currentFatherCategoryIdHashed = null;
                }
                else
                {
                    currentFatherCategoryIdHashed = _hashidservice.Encode(currentFatherCategoryId);
                }

                answerDto.Add(new FullCategoryDto()
                {
                    Id = _hashidservice.Encode(desiredCategories[i].Id),
                    Name = desiredCategories[i].Name,
                    Description = desiredCategories[i].Description,
                    FatherCategoryId = currentFatherCategoryIdHashed,
                    DateCreated = desiredCategories[i].DateCreated,
                    LastModified = desiredCategories[i].LastModified
                });
            }

            //If Category(s) is Found return success
            if (desiredCategories.Any())
            {
                return new CategoryResult
                {
                    Categories = answerDto,
                    ErrorMessage = null,
                    Success = true,
                    OperationDate = null
                };
            }

            //Return unsuccessfull operation if the searched category is Not Found
            return new CategoryResult
            {
                Categories = null,
                ErrorMessage = $"Category {dto.Id}, {dto.Name}, {dto.Description}, {dto.FatherCategoryId}, {dto.DateCreated} Not Found",
                Success = false,
                OperationDate = null
            };

        }

        //3.-U:Update
        public async Task<CategoryResult> UpdateCategory(string hashedId, UpdateCategoryDto dto)
        {
            //Decode given Id
            int? Id = _hashidservice.Decode(hashedId);
            if (Id == null)
            {
                return new CategoryResult
                {
                    Categories = null,
                    ErrorMessage = "Hash ID incorrect format.",
                    Success = false,
                    OperationDate = DateTime.Now
                };
            }

            //Search entity
            Category? updatableCategory = await _dbcontext.Categories.FirstOrDefaultAsync(p => p.Id == Id);
            if (updatableCategory == null)
            {
                return new CategoryResult
                {
                    Categories = null,
                    ErrorMessage = "Requested Category Not Found.",
                    Success = false,
                    OperationDate = DateTime.Now
                };
            }

            //Update only not null DTO Fields
            var dtoProperties = dto.GetType().GetProperties();
            foreach (var prop in dtoProperties)
            {
                var value = prop.GetValue(dto);
                if (value != null)
                {
                    var entityProp = updatableCategory.GetType().GetProperty(prop.Name);
                    if (entityProp != null)
                    {
                        entityProp.SetValue(updatableCategory, value);
                    }
                }
            }

            //Update Last Modified value
            updatableCategory.LastModified = DateTime.Now;

            //Save changes at DB
            await _dbcontext.SaveChangesAsync();

            //Answer DTO
            var resultDto = new FullCategoryDto
            {
                Id = hashedId,
                Name = updatableCategory.Name,
                Description = updatableCategory.Description,
                FatherCategoryId = updatableCategory.FatherCategoryId != null
                    ? _hashidservice.Encode((int)updatableCategory.FatherCategoryId)
                    : null,
                DateCreated = updatableCategory.DateCreated,
                LastModified = updatableCategory.LastModified
            };

            return new CategoryResult
            {
                Categories = new List<FullCategoryDto> { resultDto },
                ErrorMessage = null,
                Success = true,
                OperationDate = DateTime.Now
            };
        }


        //4.-D:Delete
        public async Task<CategoryResult> DeleteCategory(string hashedId)
        {
            //Decode hashed ID to int
            int? id = _hashidservice.Decode(hashedId);

            if (id == null)
            {
                return new CategoryResult
                {
                    Success = false,
                    ErrorMessage = "Invalid ID format.",
                    OperationDate = DateTime.Now
                };
            }

            //Search requested category
            var category = await _dbcontext.Categories.FindAsync(id);

            if (category == null)
            {
                return new CategoryResult
                {
                    Success = false,
                    ErrorMessage = "Category not found.",
                    OperationDate = DateTime.Now
                };
            }

            //Delete entity and save changes
            _dbcontext.Categories.Remove(category);
            await _dbcontext.SaveChangesAsync();

            return new CategoryResult
            {
                Success = true,
                ErrorMessage = null,
                OperationDate = DateTime.UtcNow,
                Categories = new List<FullCategoryDto>
                {
                    new FullCategoryDto
                    {
                        Id = hashedId,
                        Name = category.Name,
                        Description = category.Description,
                        FatherCategoryId = category.FatherCategoryId != null
                            ? _hashidservice.Encode((int)category.FatherCategoryId)
                            : null,
                        DateCreated = category.DateCreated,
                        LastModified = category.LastModified
                    }
                }
            };
        }

    }

}

//Result class purpose
public class CategoryResult
{

    public List<FullCategoryDto>? Categories { get; set; }
    public string? ErrorMessage { get; set; }
    public bool Success { get; set; }
    public DateTime? OperationDate { get; set; }

}

public class DeletableCategoryResult
{

    public List<Category>? Categories { get; set; }
    public string? ErrorMessage { get; set; }
    public bool Success { get; set; }
    public DateTime? OperationDate { get; set; }

}