using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Do_an_mon_hoc.Models;
using System.Linq;
using AutoMapper;
using Do_an_mon_hoc.Dto.Products;
using Do_an_mon_hoc.Models;
using Newtonsoft.Json.Linq;



namespace Do_an_mon_hoc.Controllers
{

    [Route("api")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        //public UserApiController(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}
        private MiniMarketContext _context { get; }
        private IMapper _mapper { get; }

        private ILogger<UserApiController> _logger;

        public UserApiController(MiniMarketContext context, IMapper mapper, ILogger<UserApiController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }


        //[HttpPost("dangnhap")]
        //public async Task<ActionResult<object>> Login([FromBody] UserDTO_Login userDto)
        //{
        //    try
        //    {
        //        // Validate the user credentials (replace this with your actual authentication logic)
        //        var user = await _context.Users.Include(p => p.Carts).SingleOrDefaultAsync(u => u.Email == userDto.Email);

        //        if (user == null || !VerifyPassword(userDto.password, user.PasswordHash))
        //        {
        //            return Unauthorized(new { error = "Sai tên đăng nhập hoặc mật khẩu" });
        //        }

        //        // Generate and return an authentication token and user information
        //        var token = GenerateAuthToken(user);

        //        var response = new
        //        {
        //            id = user.Id,
        //            cartId = user.Carts.FirstOrDefault()?.Id,
        //            email = user.Email,
        //            fullname = user.Fullname,
        //            phone = user.PhoneNumber,
        //            address = user.Address,
        //            token,
        //            isLoggedIn = true
        //        };

        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new
        //        {
        //            status = "error",
        //            message = "Đăng nhập không thành công",
        //            error = new
        //            {
        //                message = ex.Message,
        //                // You can include additional details about the error if needed
        //            }
        //        });
        //    }
        //}


        //[HttpPost("dangky")]
        //public async Task<ActionResult<object>> SignUp([FromBody] UserDTO_SignUp userDto)
        //{
        //    try
        //    {
        //        // Check if the email already exists
        //        var existingUser = await _context.Users.Include(p => p.Carts).FirstOrDefaultAsync(u => u.Email == userDto.Email);
        //        if (existingUser != null)
        //        {
        //            return BadRequest(new { error = "Email đã tồn tại" });
        //        }

        //        //var verificationLink = $"https://localhost:5020/verify-email?token=aaa";

        //        //var emailService = new EmailService(_configuration); // Inject IConfiguration in your controller

        //        //await emailService.SendEmailAsync(userDto.Email, "Verify Your Email", $"Click on the link to verify your email: {verificationLink}");

        //        // Create a new user
        //        var newUser = new User
        //        {
        //            Email = userDto.Email,
        //            PasswordHash = userDto.password, // Ensure to hash the password
        //            Fullname = userDto.Fullname,
        //            PhoneNumber = "",
        //            // Add other properties as needed
        //        };

        //        // Save the user to the database
        //        _context.Users.Add(newUser);
        //        await _context.SaveChangesAsync();

        //        // Automatically create a cart for the new user
        //        var newCart = new Cart();
        //        newUser.Carts.Add(newCart);

        //        // Save changes to the database
        //        await _context.SaveChangesAsync();

        //        // Generate and return an authentication token and user information
        //        var token = GenerateAuthToken(newUser);

        //        var response = new
        //        {
        //            id = newUser.Id,
        //            cartId = newUser.Carts.FirstOrDefault()?.Id,
        //            email = newUser.Email,
        //            fullname = newUser.Fullname,
        //            phone = "",
        //            address = "",
        //            token,
        //            isLoggedIn = true
        //        };

        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new
        //        {
        //            status = "error",
        //            message = "Failed to log in",
        //            error = new
        //            {
        //                message = ex.Message,
        //                // You can include additional details about the error if needed
        //            }
        //        });
        //    }
        //}

        //[HttpGet("/api/taikhoan/{userId}/thongtin")]
        //public async Task<ActionResult<object>> GetUserInfo(int userId)
        //{
        //    try
        //    {
        //        // Fetch user information based on the user ID
        //        var user = await _context.Users
        //            .Where(u => u.Id == userId)
        //                                .Include(p => p.Carts)
        //            .FirstOrDefaultAsync();

        //        if (user == null)
        //        {
        //            return NotFound("User not found");
        //        }

        //        // Map the user entity to a DTO
        //        var response = new
        //        {
        //            id = user.Id,
        //            cartId = user.Carts.FirstOrDefault()?.Id,
        //            email = user.Email,
        //            fullname = user.Fullname,
        //            phone = user.PhoneNumber,
        //            address = user.Address,
        //            isLoggedIn = true
        //        };

        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new
        //        {
        //            status = "error",
        //            message = "Failed to get user information",
        //            error = new
        //            {
        //                message = ex.Message,
        //                // You can include additional details about the error if needed
        //            }
        //        });
        //    }
        //}

        //[HttpPut("/api/taikhoan/{userId}/capnhat")]
        //public async Task<ActionResult<object>> UpdateUserProfile(int userId, [FromBody] UserDTO_Update updateDTO)
        //{
        //    try
        //    {
        //        // Fetch user based on the user ID
        //        var user = await _context.Users
        //            .Where(u => u.Id == userId)
        //            .Include(p => p.Carts)
        //            .FirstOrDefaultAsync();

        //        if (user == null)
        //        {
        //            return NotFound("User not found");
        //        }

        //        // Update user information based on the DTO
        //        user.Email = updateDTO.Email;
        //        user.Fullname = updateDTO.Fullname;
        //        user.PhoneNumber = updateDTO.phone;
        //        user.Address = updateDTO.Address;

        //        // Save changes to the database
        //        await _context.SaveChangesAsync();

        //        var response = new
        //        {
        //            id = user.Id,
        //            cartId = user.Carts.FirstOrDefault()?.Id,
        //            email = user.Email,
        //            fullname = user.Fullname,
        //            phone = user.PhoneNumber,
        //            address = user.Address,
        //            isLoggedIn = true
        //        };

        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new
        //        {
        //            status = "error",
        //            message = "Failed to update user profile",
        //            error = new
        //            {
        //                message = ex.Message,
        //                // You can include additional details about the error if needed
        //            }
        //        });
        //    }
        //}



        private bool VerifyPassword(string inputPassword, string storedPasswordHash)
        {
            // Implement password verification logic (e.g., using a hashing library)
            // Compare the inputPassword with the storedPasswordHash
            // Return true if they match, false otherwise
            return true;
        }

        private string GenerateAuthToken(User user)
        {
            // Implement token generation logic (e.g., using a JWT library)
            // Return the generated authentication token
            return "YourAuthTokenHere";
        }




    }
}
