using PACHA_FIT.Core.Domain.Shared.ResultPattern;
namespace PACHA_FIT.Infrastructure.Nswag;

public partial class ResultDtoOfString : IResult
{
    public object? GetValue() => this.Value;
}

public partial class ResultDtoOfLoginResponse : IResult
{
    public object? GetValue() => this.Value;
}

public partial class ResultDtoOfUserResponseDto : IResult
{
    public object? GetValue() => this.Value;
}

public partial class ResultDtoOfUnitOfMeasureGroupedDto : IResult
{
    public object? GetValue() => this.Value;
}

public partial class ResultDtoOfDecimal : IResult
{
    public object? GetValue() => this.Value;
}

public partial class ResultDtoOfProductResponse : IResult
{
    public object? GetValue() => this.Value;
}

public partial class ResultDtoOfBoolean : IResult
{
    public object? GetValue() => this.Value;
}
