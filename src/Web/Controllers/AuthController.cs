using Application.Dtos;
using Application.Interfaces.Services;
using Application.Services;
using Azure;
using Core.Authentications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(IAuthService authService, IJwtTokenService jwtTokenService)
        {
            this._authService = authService;
            this._jwtTokenService = jwtTokenService;
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

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                AuthenticationResult authResult = await _authService.Login(loginDto);

                if (!authResult.Success)
                {
                    return BadRequest("Invalid email or password");
                }

                // Generate JWT
                string token = _jwtTokenService.GenerateToken(authResult.User, authResult.Roles[0]);

                CookieOptions cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddMonths(3),
                    SameSite = SameSiteMode.Strict,
                    Secure = true
                };

                // Set jwt in cookie
                HttpContext.Response.Cookies.Append("JwtToken", token, cookieOptions);

                return Created(
                    "",
                    new
                    {
                        success = true,
                        statusCode = 201,
                        token
                    }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
