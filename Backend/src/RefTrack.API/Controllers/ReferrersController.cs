using MediatR;
using Microsoft.AspNetCore.Mvc;
using RefTrack.API.DTOs;
using RefTrack.Application.Features.Referrers.Commands;
using RefTrack.Application.Features.Referrers.Queries;

namespace RefTrack.API.Controllers
{
    [Route("api/[controller]")]
    public class ReferrersController : BaseController
    {
        private readonly IMediator _mediator;
        public ReferrersController(IMediator mediator) => _mediator = mediator;
        // GET api/referrers?jobRoleId=xxx
        [HttpGet]
        public async Task<IActionResult> GetByJobRole([FromQuery] Guid jobRoleId,
       CancellationToken ct)
        => Ok(await _mediator.Send(new
       GetReferrersByJobRoleQuery(jobRoleId, CurrentUserId), ct));
        // GET api/referrers/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(new GetAllReferrersQuery(CurrentUserId), ct));

        [HttpPost]
        public async Task<IActionResult> Create(CreateReferrerRequest req, CancellationToken
       ct)
        => Ok(await _mediator.Send(
        new CreateReferrerCommand(req.Name, req.LinkedInUrl, req.Designation, req.JobRoleId, CurrentUserId), ct));

        // Marks Sent + sets LastContactedAt = now
        [HttpPatch("{id:guid}/contact")]
        public async Task<IActionResult> Contact(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new MarkContactedCommand(id, CurrentUserId), ct));
        [HttpPatch("{id:guid}/replied")]
        public async Task<IActionResult> Replied(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new MarkRepliedCommand(id, CurrentUserId), ct));
        [HttpPatch("{id:guid}/referred")]
        public async Task<IActionResult> Referred(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new MarkReferredCommand(id, CurrentUserId), ct));
        [HttpPatch("{id:guid}/ghosted")]
        public async Task<IActionResult> Ghosted(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new MarkGhostedCommand(id, CurrentUserId), ct));
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteReferrerCommand(id, CurrentUserId), ct));
    }
}
