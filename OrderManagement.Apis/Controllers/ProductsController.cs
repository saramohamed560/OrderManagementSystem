using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Apis.DTOs;
using OrderManagement.Apis.Errors;
using OrderManagement.Core;
using OrderManagement.Core.Entities;

namespace OrderManagement.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork unitOfWork ,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //Create Product
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(ProductDto dto)
        {
            var product = new Product()
            {
                Name = dto.Name,
                Price = dto.Price,
                stock = dto.stock,
            };
            await _unitOfWork.Repository<Product>().AddAsync(product);
            await _unitOfWork.CompleteAsync();
            return Ok(product);

        }
        //Update Product
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, ProductDto dto)
        {

            var product = await _unitOfWork.Repository<Product>().GetAsync(id);
            if (product is null)
                return NotFound(new ApiResponse(404,$"Product with id {id} Not Found"));
            product.Name = dto.Name;
            product.Price = dto.Price;
            product.stock = dto.stock;
            _unitOfWork.Repository<Product>().Update(product);
            await _unitOfWork.CompleteAsync();
            return Ok(product);
        }

        //Get All Products
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetAlProducts()
        {
            var products = await _unitOfWork.Repository<Product>().GetAllAsync();
            return Ok(products);
        }
        //Get Specific Product
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {

            var product = await _unitOfWork.Repository<Product>().GetAsync(id);
            if (product is null)
                return NotFound(new ApiResponse(404));
           
            return Ok(product);
        }

        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {

            var product = await _unitOfWork.Repository<Product>().GetAsync(id);
            if (product is null)
                return NotFound(new ApiResponse(404));
            _unitOfWork.Repository<Product>().Delete(product);
            await _unitOfWork.CompleteAsync();
            return Ok(product);
        }

    }
}
