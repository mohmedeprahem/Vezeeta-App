using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            this._authService = authService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            // Validate image
            if (registerDto.Image != null)
            {
                long fileSize = registerDto.Image.Length;

                if (
                    registerDto.Image.ContentType != "image/jpeg"
                    && registerDto.Image.ContentType != "image/png"
                )
                {
                    ModelState.AddModelError(
                        "Image",
                        "Invalid image format. Only JPEG and PNG are allowed."
                    );
                }
                else if (fileSize > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError(
                        "Image",
                        "Invalid image size. The maximum allowed size is 5 MB."
                    );
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await _authService.Register(registerDto);

            if (result.Succeeded)
            {
                return Created(
                    "User registered successfully",
                    new { success = true, statusCode = 201 }
                );
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
