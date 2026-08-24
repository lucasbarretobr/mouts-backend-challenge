using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ambev.DeveloperEvaluation.Application.Sales;
using Ambev.DeveloperEvaluation.WebApi.Common;
namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class SalesController : BaseController
{
    readonly IMediator mediator;
    readonly IMapper mapper;
    public SalesController(IMediator mediator, IMapper mapper) { this.mediator = mediator; this.mapper = mapper; }

    [HttpGet]
    public async Task<IActionResult> List(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var r = await mediator.Send(new ListSalesQuery(page, pageSize), cancellationToken);
        return Ok(new PaginatedResponse<SaleResult> { Success = true, Message = "Sales retrieved successfully", Data = r.Items, CurrentPage = r.CurrentPage, TotalPages = r.TotalPages, TotalCount = r.TotalCount });
    }
    [HttpPost]
    public async Task<IActionResult> Create(SaleRequest request, CancellationToken cancellationToken)
    {
        var v = await new SaleRequestValidator().ValidateAsync(request, cancellationToken);

        if (!v.IsValid) return BadRequest(v.Errors);

        var r = await mediator.Send(mapper.Map<CreateSaleCommand>(request), cancellationToken);

        if (!r.IsSuccess) return FromResult(r);

        return StatusCode(201, new ApiResponseWithData<SaleResult> { Success = true, Message = "Sale created successfully", Data = r.Value });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var r = await mediator.Send(new GetSaleQuery(id), cancellationToken);

        if (!r.IsSuccess) return FromResult(r);

        return Ok(new ApiResponseWithData<SaleResult>
        { Success = true, Message = "Sale retrieved successfully", Data = r.Value });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, SaleRequest request, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty) return BadRequest("Sale ID is required");

        var v = await new SaleRequestValidator().ValidateAsync(request, cancellationToken);

        if (!v.IsValid) return BadRequest(v.Errors);

        var command = mapper.Map(request, new UpdateSaleCommand { Id = id });

        var r = await mediator.Send(command, cancellationToken);

        if (!r.IsSuccess) return FromResult(r);
        return Ok(new ApiResponseWithData<SaleResult> { Success = true, Message = "Sale updated successfully", Data = r.Value });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var r = await mediator.Send(new DeleteSaleCommand(id), cancellationToken);

        if (!r.IsSuccess)
            return StatusCode(404, new ApiResponse { Success = false, Message = r.Errors.FirstOrDefault()?.Message ?? "Sale not found." });
        return Ok(new ApiResponse { Success = true, Message = "Sale deleted successfully" });
    }


    [HttpPatch("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
    {
        var r = await mediator.Send(new CancelSaleCommand(id), cancellationToken);
        if (!r.IsSuccess) return FromResult(r);
        return Ok(new ApiResponseWithData<SaleResult> { Success = true, Message = "Sale cancelled successfully", Data = r.Value });
    }

    [HttpPatch("{saleId:guid}/items/{itemId:guid}/cancel")]
    public async Task<IActionResult> CancelItem(Guid saleId, Guid itemId, CancellationToken cancellationToken)
    {
        var r = await mediator.Send(new CancelSaleItemCommand(saleId, itemId), cancellationToken);
        if (!r.IsSuccess) return FromResult(r);
        return Ok(new ApiResponseWithData<SaleResult> { Success = true, Message = "Sale item cancelled successfully", Data = r.Value });
    }
}
