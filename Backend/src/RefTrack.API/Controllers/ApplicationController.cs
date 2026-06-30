using MediatR;
using Microsoft.AspNetCore.Mvc;
using RefTrack.API.DTOs;
using RefTrack.Application.Features.Applications.Commands;
using RefTrack.Application.Features.Applications.Queries;

namespace RefTrack.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : BaseController
    {
        private readonly IMediator _mediator;
        public ApplicationController(IMediator mediator) => _mediator = mediator;
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(new GetAllApplicationsQuery(CurrentUserId), ct));
        // GET api/applications/summary — returns {Applied:5, HRScreen:2, Offered:1}
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary(CancellationToken ct)
        => Ok(await _mediator.Send(new GetPipelineSummaryQuery(CurrentUserId), ct));
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateApplicationRequest req, CancellationToken ct)
    => Ok(await _mediator.Send(new CreateApplicationCommand(req.JobRoleId, CurrentUserId), ct)); 
        // PATCH api/applications/move — move to next pipeline stage
        [HttpPatch("move")]
        public async Task<IActionResult> Move(MoveApplicationRequest req, CancellationToken ct)
        => Ok(await _mediator.Send(
        new MoveApplicationCommand(req.ApplicationId, req.Status, CurrentUserId), ct));
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteApplicationCommand(id, CurrentUserId), ct));
    }
}
