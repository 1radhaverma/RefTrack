using MediatR;
using Microsoft.AspNetCore.Mvc;
using RefTrack.API.DTOs;
using RefTrack.Application.Features.JobRoles.Commands;
using RefTrack.Application.Features.JobRoles.Queries;

namespace RefTrack.API.Controllers
{
    [Route("api/[controller]")]
    public class JobRolesController : BaseController
    {
        private readonly IMediator _mediator;
        public JobRolesController(IMediator mediator) => _mediator = mediator;
        // GET api/jobroles?companyId=xxx
        
        [HttpGet]
        public async Task<IActionResult> GetByCompany([FromQuery] Guid companyId,
       CancellationToken ct)
        => Ok(await _mediator.Send(new GetJobsByCompanyQuery(companyId, CurrentUserId), ct));
        // GET api/jobroles/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(new GetAllJobRolesQuery(CurrentUserId), ct));
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateJobRoleRequest req, CancellationToken ct)
         => Ok(await _mediator.Send(
        new CreateJobRoleCommand(req.Title, req.JobUrl, req.JobDescription,
        req.Tier, req.CompanyId, CurrentUserId), ct));
      
        [HttpPatch("{id:guid}/apply")]
        public async Task<IActionResult> Apply(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new ApplyToJobCommand(id, CurrentUserId), ct));
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteJobRoleCommand(id, CurrentUserId), ct));
    }
}
