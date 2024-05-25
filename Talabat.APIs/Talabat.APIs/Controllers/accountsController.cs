using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
  
    public class accountsController : ApiBaseController
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public accountsController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,ITokenService tokenService,IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user is null) return Unauthorized(new ApiErrorResponse(401));
            var result = await signInManager.CheckPasswordSignInAsync(user,model.Password,false);
            if(!result.Succeeded) return Unauthorized(new ApiErrorResponse(401));

            return Ok(new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await tokenService.CreateTokenAsync(user,userManager)
            });

        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if(CheckEmailExist(model.Email).Result.Value)
                   return BadRequest(new ApiValidationErrorResponse()   // استخدمتو علشان اعرف اكتب الرسالة
                   { Errors = new string[] { "This Email Is Already Exist" } });

            var user = new AppUser()
            {
                
                DisplayName = model.DisplayName,
                Email = model.Email,//ahmed.gawesh@gmail.com
                UserName = model.Email.Split('@')[0],//ahmed.gawesh
                PhoneNumber = model.PhoneNumber,
                
            };
            var result= await  userManager.CreateAsync(user,model.Password);
            if (!result.Succeeded) return BadRequest(new ApiErrorResponse(400));
            return Ok(new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await tokenService.CreateTokenAsync(user, userManager)
            });
        }

        [Authorize]
        [HttpGet("currentuser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email=User.FindFirstValue(ClaimTypes.Email);
            var user=await userManager.FindByEmailAsync(email);

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await tokenService.CreateTokenAsync(user, userManager)
            });
        }
        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await userManager.FindUserWithAddressByEmailAsync(User);
            var address = mapper.Map<Address, AddressDto>(user.Address);

            return Ok(address);
        }

        [Authorize] // يعني هباصي Token 
        [HttpPut("updatedAddress")] 
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto updatedAddress)
        {
            var address=mapper.Map<AddressDto,Address>(updatedAddress);
            var user = await userManager.FindUserWithAddressByEmailAsync(User);

            // باخد ال id بتاعو واحطو في التحديث الجديد 
            address.Id = user.Address.Id; // علشان ال id مش يتغير
           
            user.Address = address; // بحط الaddress الجديد في القديم 

            var result=await userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiErrorResponse(400));

            return Ok(updatedAddress);  // هيرجعلي الحاجة اللي انا غيرتها 

        }

        [HttpGet("emailexist")]
        public async Task<ActionResult<bool>> CheckEmailExist(string email)
        {
            return await userManager.FindByEmailAsync(email) is not null;
               // يعني لو فيه ايميل عندي متسجل هيرجع true 
               // غير كدا هيرجع false
        }


    }
}
