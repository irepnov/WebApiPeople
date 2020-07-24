using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PeopleWebApi.Interfaces;
using PeopleWebApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace PeopleWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    #pragma warning disable CS1591 // Отсутствует комментарий XML для открытого видимого типа или члена
    public class LoginController : ControllerBase
    {
        private readonly IDBRepository<User, User> _dataRepository;
        public LoginController(IDBRepository<User, User> dataRepository) => _dataRepository = dataRepository;
        // POST: api/login
        [HttpPost()]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Аутентифицирует пользователя и генерирует токен доступа", Tags = new[] { "Аутентификация пользователя" })]
        [Consumes(MediaTypeNames.Application.Json)]//формат запроса
        [Produces(MediaTypeNames.Application.Json)]//фортам ответа
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetToken([FromBody] User user)
        {
            ClaimsIdentity identity = GetIdentity(user);
            if (identity == null)
                return BadRequest(new { errorText = "Invalid username or password." });
            DateTime now = DateTime.UtcNow;
            // создаем JWT-токен
            JwtSecurityToken jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            string encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Ok(new
            {
                access_token = encodedJwt,
                username = identity.Name
            });
        }

        private ClaimsIdentity GetIdentity(User user)
        {
            User person = _dataRepository.GetAsQueryable(predicates: new List<Expression<Func<User, bool>>> { us => us.Login.Equals(user.Login) && us.Password.Equals(user.Password) }, us => us.UserRoles).FirstOrDefault<User>();
            if (person != null)
            {
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login));
                person.UserRoles.ToList().ForEach(role => claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role.RoleId.ToString())));               
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            // если пользователь не найден
            return null;
        }

        // GET: api/login/getlogin
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet()]
        [Route("getlogin")]
        [SwaggerOperation(Summary = "Возвращает информацию о логине пользователя", Description = "Требуется авторизация", Tags = new[] { "Аутентификация пользователя" })]
        [Produces(MediaTypeNames.Application.Json)]//фортам ответа
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetShortInfoUser()
        {
            return Ok($"Ваш логин: {User.Identity.Name}");
        }

        // GET: api/login/getrole
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "2,3")]
        [HttpGet()]
        [Route("getrole")]
        [SwaggerOperation(Summary = "Возвращает детальную информацию о пользователе", Description = "Требуется авторизация и группа доступа 2 или 3", Tags = new[] { "Аутентификация пользователя" })]
        [Produces(MediaTypeNames.Application.Json)]//фортам ответа
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetFullInfoUser()
        {
            return Ok(User.Identity);
        }
    }
    #pragma warning restore CS1591 // Отсутствует комментарий XML для открытого видимого типа или члена
}
