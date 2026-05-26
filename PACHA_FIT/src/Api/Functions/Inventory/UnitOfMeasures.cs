using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using IResult = PACHA_FIT.Core.Domain.Shared.ResultPattern.IResult;

namespace PACHA_FIT.Api.Functions.Inventory;

public class UnitOfMeasures
{
    [Function("UnitOfMeasures")]
    public async Task<IResult> GetUnitOfMeasures([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route="/units" )] HttpRequest req)
    {
        return null; 
    }


}