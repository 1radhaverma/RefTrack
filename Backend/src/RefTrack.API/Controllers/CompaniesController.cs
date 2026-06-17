using MediatR;
using Microsoft.AspNetCore.Mvc;
using RefTrack.API.DTOs;
using RefTrack.Application.Features.Companies.Commands;
using RefTrack.Application.Features.Companies.Queries;

namespace RefTrack.API.Controllers
{
    [Route("api/[controller]")]
    public class CompaniesController : BaseController
    {
        private readonly IMediator _mediator;
        public CompaniesController(IMediator mediator) => _mediator = mediator;
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(new GetAllCompaniesQuery(CurrentUserId), ct));
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetCompanyByIdQuery(id, CurrentUserId), ct));
        [HttpPost]
        public async Task<IActionResult> Create(CreateCompanyRequest req, CancellationToken ct)
        => Ok(await _mediator.Send(
        new CreateCompanyCommand(req.Name, req.Domain, req.Tier, CurrentUserId), ct));
        [HttpPatch("{id:guid}/blacklist")]
        public async Task<IActionResult> Blacklist(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new BlacklistCompanyCommand(id, CurrentUserId), ct));
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteCompanyCommand(id, CurrentUserId), ct));
    }
}
