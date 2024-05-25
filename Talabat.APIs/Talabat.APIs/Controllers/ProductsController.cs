using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;
using Talabat.Repository.Repositories;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : ApiBaseController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProductsController(IUnitOfWork unitOfWork
            ,IMapper mapper)
        {
         
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        
        [HttpGet]                     //اللي هيرجع عبارة عن pagination
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams productSpec) //fromQuery=> علشان يشوف ال parameters 
        {
            var spec=new ProductSpecifications(productSpec);
            var products=await unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);
            var data = mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDto>>(products);

            var CountSpec = new ProductWithFilterationForCountSpecification(productSpec);
             var Count = await unitOfWork.Repository<Product>().GetCountWithSpec(CountSpec);

            return Ok(new Pagination<ProductToReturnDto>(productSpec.PageSize,productSpec.PageIndex,Count,data));
        }

        [ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse),StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProductById(int id)
        {
            var spec=new ProductSpecifications(id);
            var product=await unitOfWork.Repository<Product>().GetByIdWithSpecAsync(spec);
            if (product is null) return NotFound(new ApiErrorResponse(404));
            var mappedProduct = mapper.Map<Product, ProductToReturnDto>(product);

            return Ok(mappedProduct);
        }

        [HttpGet("brands")]// api/products/brands
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetAllBrands()
        {
            var brands=await unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return Ok(brands);
        }

        [HttpGet("types")]// api/products/types
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetAllTypes()
        {
            var types = await unitOfWork.Repository<ProductType>().GetAllAsync();
            return Ok(types);
        }

    }
}
