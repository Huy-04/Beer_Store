using Api.Core;
using BeerStore.Application.DTOs.Shop.Store.Requests;
using BeerStore.Application.DTOs.Shop.Store.Responses;
using BeerStore.Application.Modules.Shop.Stores.Commands.ApproveStore;
using BeerStore.Application.Modules.Shop.Stores.Commands.BanStore;
using BeerStore.Application.Modules.Shop.Stores.Commands.ReactivateStore;
using BeerStore.Application.Modules.Shop.Stores.Commands.RegisterStore;
using BeerStore.Application.Modules.Shop.Stores.Commands.RejectStore;
using BeerStore.Application.Modules.Shop.Stores.Commands.ResubmitStore;
using BeerStore.Application.Modules.Shop.Stores.Commands.SuspendStore;
using BeerStore.Application.Modules.Shop.Stores.Commands.UpdateStore;
using BeerStore.Application.Modules.Shop.Stores.Queries.GetAllStores;
using BeerStore.Application.Modules.Shop.Stores.Queries.GetMyStore;
using BeerStore.Application.Modules.Shop.Stores.Queries.GetStoreById;
using BeerStore.Application.Modules.Shop.Stores.Queries.GetStoreBySlug;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeerStore.Api.Controllers.Shop
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StoresController : BaseApiController
    {
        private readonly IMediator _mediator;

        public StoresController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Queries - Public

        // GET: api/Store/slug/{slug}
        [HttpGet("slug/{slug}")]
        [AllowAnonymous]
        public async Task<ActionResult<StorePublicResponse>> GetBySlug([FromRoute] string slug, CancellationToken token)
        {
            var result = await _mediator.Send(new GetStoreBySlugQuery(slug), token);
            if (result == null) return NotFound();
            return Ok(result);
        }

        #endregion Queries - Public

        #region Queries - Protected

        // GET: api/Store
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StoreResponse>>> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? status = null,
            CancellationToken token = default)
        {
            var result = await _mediator.Send(new GetAllStoresQuery(pageNumber, pageSize, searchTerm, status), token);
            return Ok(result);
        }

        // GET: api/Store/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<StoreResponse>> GetById([FromRoute] Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetStoreByIdQuery(id), token);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // GET: api/Store/my
        [HttpGet("my")]
        public async Task<ActionResult<StoreResponse>> GetMyStore(CancellationToken token)
        {
            var result = await _mediator.Send(new GetMyStoreQuery(CurrentUserId), token);
            if (result == null) return NotFound();
            return Ok(result);
        }

        #endregion Queries - Protected

        #region Commands - Owner

        // POST: api/Stores
        [HttpPost]
        public async Task<ActionResult<StoreResponse>> Register(
            [FromBody] RegisterStoreRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new RegisterStoreCommand(CurrentUserId, CurrentUserId, request), token);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: api/Store/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<StoreResponse>> Update(
            [FromRoute] Guid id, [FromBody] UpdateStoreRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new UpdateStoreCommand(id, CurrentUserId, request), token);
            return Ok(result);
        }

        // PUT: api/Store/{id}/resubmit
        [HttpPut("{id:guid}/resubmit")]
        public async Task<ActionResult<StoreResponse>> Resubmit(
            [FromRoute] Guid id, [FromBody] ResubmitStoreRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(new ResubmitStoreCommand(id, CurrentUserId, request), token);
            return Ok(result);
        }

        #endregion Commands - Owner

        #region Commands - Admin

        // PUT: api/Store/{id}/approve
        [HttpPut("{id:guid}/approve")]
        public async Task<ActionResult<StoreResponse>> Approve([FromRoute] Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new ApproveStoreCommand(id, CurrentUserId), token);
            return Ok(result);
        }

        // PUT: api/Store/{id}/reject
        [HttpPut("{id:guid}/reject")]
        public async Task<ActionResult<StoreResponse>> Reject(
            [FromRoute] Guid id, [FromQuery] string? reason, CancellationToken token)
        {
            var result = await _mediator.Send(new RejectStoreCommand(id, CurrentUserId, reason), token);
            return Ok(result);
        }

        // PUT: api/Store/{id}/suspend
        [HttpPut("{id:guid}/suspend")]
        public async Task<ActionResult<StoreResponse>> Suspend(
            [FromRoute] Guid id, [FromQuery] string reason, CancellationToken token)
        {
            var result = await _mediator.Send(new SuspendStoreCommand(id, CurrentUserId, reason), token);
            return Ok(result);
        }

        // PUT: api/Store/{id}/ban
        [HttpPut("{id:guid}/ban")]
        public async Task<ActionResult<StoreResponse>> Ban(
            [FromRoute] Guid id, [FromQuery] string reason, CancellationToken token)
        {
            var result = await _mediator.Send(new BanStoreCommand(id, CurrentUserId, reason), token);
            return Ok(result);
        }

        // PUT: api/Store/{id}/reactivate
        [HttpPut("{id:guid}/reactivate")]
        public async Task<ActionResult<StoreResponse>> Reactivate([FromRoute] Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new ReactivateStoreCommand(id, CurrentUserId), token);
            return Ok(result);
        }

        #endregion Commands - Admin
    }
}
