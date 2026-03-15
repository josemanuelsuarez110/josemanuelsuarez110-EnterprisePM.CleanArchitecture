using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementERP.Application.Interfaces.Services;

namespace ProjectManagementERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("token")]
        [AllowAnonymous]
        public ActionResult<dynamic> IssueToken([FromBody] TokenRequest request)
        {
            // NOTE: Replace with real authentication (e.g., Identity) in production
            var token = _tokenService.GenerateToken(request.UserId, request.UserName, request.Roles ?? new List<string>());
            return Ok(new { access_token = token });
        }
    }

    public class TokenRequest
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string UserName { get; set; } = string.Empty;
        public IEnumerable<string>? Roles { get; set; }
    }
}
