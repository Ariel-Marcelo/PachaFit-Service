using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PACHA_FIT.Core.Application.Inventory;
using IResult = PACHA_FIT.Core.Domain.Shared.ResultPattern.IResult;

namespace PACHA_FIT.Api.Functions.Inventory;

public class UnitOfMeasuresFunc
{
    private readonly ILogger<UnitOfMeasuresFunc> _logger;
    private readonly UnitOfMeasureService _unitOfMeasuresService;
    
    [Function("UnitOfMeasuresFunc")]
    public async Task<IResult> GetUnitOfMeasures([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route="/units" )] HttpRequest req)
    {
        return await _unitOfMeasuresService.GetAllActiveUnitsAsync(); 
    }


}