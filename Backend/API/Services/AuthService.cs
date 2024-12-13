using System.Linq;
using System.Security.Claims;
using Application.Interfaces;
using Application.Services.Authentication;
using Common.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace API.Services
{
    public class AuthService : IAuthService
    {
        private readonly ClaimsPrincipal _claims;

        public AuthService(IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            var authorizationString =
                contextAccessor.HttpContext.Request.Headers["Authorization"].ElementAtOrDefault(0);
            var jwt = authorizationString?.Split("Bearer ").ElementAtOrDefault(1);
            if (jwt == null) return;
            _claims = AuthenticationService.GetPrincipalFromTokenString(new StandardAuthenticationModel
            {
                JwtToken = jwt,
                SecurityKey = configuration["SecurityKey"],
                Issuer = configuration["Issuer"],
                Audience = configuration["Audience"]
            });
        }

        public int? GetUserId()
        {
            if (int.TryParse(_claims.GetClaimValue(ClaimTypes.PrimarySid), out var result))
                return result;
            return null;
        }

        public bool IsUserIdSelf(int? userId)
        {
            if (userId == null) return false;
            return GetUserId() == userId;
        }

        public bool UserIsManagerOfGroup(Group group)
        {
            if (group == null) return false;
            var userGroupWithId = group.GroupUsers.Where(gu => gu.UserId == GetUserId());
            return userGroupWithId.Any(ug => ug.IsManager == true);
        }

        public bool UserIsManagerOfGroup(User user, Group @group)
        {
            var userGroupWithId = group.GroupUsers.Where(gu => gu.UserId == user.Id);
            return userGroupWithId.Any(ug => ug.IsManager == true);
        }

        public bool UserIsOwnerOfGroup(Group group)
        {
            return group.OwnerId == GetUserId();
        }

        public bool UserIsOwnerOfGroup(User user, Group group)
        {
            return group.OwnerId == user.Id;
        }

        public bool UserIsPartOfGroup(Group group)
        {
            return group.GroupUsers.Any(gu => gu.UserId == GetUserId());
        }
    }
}