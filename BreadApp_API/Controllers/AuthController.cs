using BreadApp_API.DTOs.Auth;
using BreadApp_BL;
using BreadApp_Struct.AuthModel;
using BreadApp_Struct.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static BreadApp_DL.UserModel;


namespace StudentApi.Controllers
{


    // This controller is responsible for authentication-related actions,
    // such as logging in and issuing JWT tokens.
    [ApiController]
    [Route("api/Auth")]
    public class AuthController : ControllerBase
    {
        private static string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        // This endpoint handles user login.
        // It verifies credentials and returns a JWT token if login succeeds.
        [HttpPost("login")]
        [EnableRateLimiting("AuthLimiter")]
        public IActionResult Login([FromBody] LoginRequest request, [FromServices] SessionContextInfo sessionInfo)
        {
            
            Access.Insert(sessionInfo);

            var UserObjDTO = Users.Authenticate(request.NaionalNumber, request.Password);

            // return 401 Unauthorized.
            if (UserObjDTO == null) { 

                Auth.InsertAuthAction(new
                AuthModel.AuthDTO{
                    UsernameAttempted = request.NaionalNumber ,
                    Action = false
                }, sessionInfo);
                return Unauthorized("Invalid credentials");
            }
            
            Users User = new Users(UserObjDTO);

            // Step 3: Create claims that represent the authenticated user's identity.
            // These claims will be embedded inside the JWT.
            var claims = new[]
            {
                // Unique identifier for the User
                new Claim(ClaimTypes.NameIdentifier, UserObjDTO.UserID.ToString()!),


                // User National number 
                new Claim(ClaimTypes.Name, UserObjDTO.NationalNumber!),


                // Role (admin , user , breadPoint) used later for authorization
                new Claim(ClaimTypes.Role, UserObjDTO.Role!)
            };


            // Step 4: Create the symmetric security key used to sign the JWT.
            // This key must match the key used in JWT validation middleware.
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("THIS_IS_A_VERY_SECRET_KEY_123456"));


            // Step 5: Define the signing credentials.
            // This specifies the algorithm used to sign the token.
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);



            // Step 6: Create the JWT token.
            // The token includes issuer, audience, claims, expiration, and signature.
            var token = new JwtSecurityToken(
                issuer: "BreadApp_API",
                audience: "BreadAppApiUsers",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );


            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            // Create refresh token (random)
            var refreshToken = GenerateRefreshToken();

            // Store refresh token securely (hash + expiry + not revoked)
            User.RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken);
            User.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);
            User.RefreshTokenRevokedAt = null;

    
            if(User.Save(sessionInfo))
            {
                Auth.InsertAuthAction(new
                 AuthModel.AuthDTO
                {
                    UsernameAttempted = request.NaionalNumber,
                    Action = true
                }, sessionInfo);
            }

            

            return Ok(new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
            
        }

        [HttpPost("refresh")]
        [EnableRateLimiting("AuthLimiter")]
        public IActionResult Refresh([FromBody] RefreshRequest request , [FromServices] SessionContextInfo sessionInfo)
        {
            var userDto = Users.GetUserBy(NationalNumber: request.NationalNum);

            if (userDto == null)
                return Unauthorized("Invalid refresh request");

            Users user = new Users(userDto);

            if (user.RefreshTokenRevokedAt != null)
                return Unauthorized("Refresh token is revoked");

            if (user.RefreshTokenExpiresAt == null || user.RefreshTokenExpiresAt <= DateTime.UtcNow)
                return Unauthorized("Refresh token expired");

            bool refreshValid = BCrypt.Net.BCrypt.Verify(request.RefreshToken, user.RefreshTokenHash);

            if (!refreshValid)
                return Unauthorized("Invalid refresh token");

            // Issue NEW access token (same claims & signing settings as login)
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()!),
                new Claim(ClaimTypes.Email, user.NationalNumber!),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("THIS_IS_A_VERY_SECRET_KEY_123456"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: "BreadApp_API",
                audience: "BreadAppApiUsers",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            var newAccessToken = new JwtSecurityTokenHandler().WriteToken(jwt);

            // Rotation: replace refresh token
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(newRefreshToken);
            user.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);
            user.RefreshTokenRevokedAt = null;

            user.Save(sessionInfo);

            return Ok(new TokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }



        [HttpPost("logout")]
        public IActionResult Logout([FromBody] LogoutRequest request, [FromServices] SessionContextInfo sessionInfo)
        {
            var userDto = Users.GetUserBy(NationalNumber: request.NationalNum);


            if (userDto == null)
                return Ok(); // Do not reveal if user exists

            Users user = new Users(userDto);

            bool refreshValid = BCrypt.Net.BCrypt.Verify(request.RefreshToken, user.RefreshTokenHash);
            if (!refreshValid)
                return Ok();

            user.RefreshTokenRevokedAt = DateTime.UtcNow;
            user.Save(sessionInfo);
            return Ok("Logged out successfully");
        }
    }
}