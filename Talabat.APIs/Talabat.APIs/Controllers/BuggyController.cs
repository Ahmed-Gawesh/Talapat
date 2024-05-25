using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlTypes;
using Talabat.APIs.Errors;
using Talabat.Repository.Data;

namespace Talabat.APIs.Controllers
{

    public class BuggyController : ApiBaseController
    {
        private readonly StoreContext context;

        public BuggyController(StoreContext context)
        {
            this.context = context;
        }
        [HttpGet("notfound")] // api/buggy/notfound
        public  ActionResult GetNotFoundRequest()
        {
            var product =  context.Products.Find(100);
            //if(product is null) return NotFound(new ApiErrorResponse(404));

            return Ok(product); 
        }


        [HttpGet("servererror")] // api/buggy/servererror
        public ActionResult GetServerError()
        {
            var product = context.Products.Find(100); //Product= Null
            var productToReturn=product.ToString(); // Through Exception Null =NullReferenceException

            return Ok(productToReturn);
        }


        [HttpGet("badRequest")] // api/buggy/badrequest
        public ActionResult BadRequest()
        {
            return BadRequest(new ApiErrorResponse(400));
        }

        [HttpGet("badRequest/{id}")] // api/buggy/badrequest/Five
        public ActionResult BadRequest(int id)
        {
            return Ok();
        }




    }
}
