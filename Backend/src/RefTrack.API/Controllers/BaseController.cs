using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RefTrack.API.Controllers
{
    [ApiController]
    [Authorize]
    public class BaseController : ControllerBase
    {
        protected Guid CurrentUserId =>
  Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
// Interview: SRP — one job only: extract userId from JWT claim.
// Every child controller gets CurrentUserId for free.
