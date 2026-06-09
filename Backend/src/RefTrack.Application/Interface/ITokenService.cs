using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using RefTrack.Domain.Entities;

namespace RefTrack.Application.Interface
{
    public interface ITokenService
    {
        string GenerateToken(AppUser user);
        ClaimsPrincipal? ValidateToken(string token);
    }
}
