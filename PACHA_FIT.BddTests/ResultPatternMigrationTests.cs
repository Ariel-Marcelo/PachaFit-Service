using NUnit.Framework;
using System.Net;
using PACHA_FIT.Core.Domain.Shared;
using PACHA_FIT.Api.Middlewares;
using PACHA_FIT.Core.Domain.Inventory.Dtos;
using PACHA_FIT.Core.Domain.Inventory.Ports;
using PACHA_FIT.Core.Application.Inventory;
using PACHA_FIT.Core.Domain.User.Ports;
using PACHA_FIT.Core.Domain.User.Dtos;
using PACHA_FIT.Core.Domain.User;
using PACHA_FIT.Core.Application.User;
using NSubstitute;


namespace PACHA_FIT.BddTests;

[TestFixture]
public class ResultPatternMigrationTests
{
    [Test]
    public void Task1_1_SystemError_ShouldHaveNewSecurityValues()
    {
        // Compile-time RED state test: references non-existent enum members
        // UserNotFound = 2000, UserAlreadyExists = 2001, InvalidCredentials = 2002, Unauthorized = 2003
        Assert.That((int)SystemError.UserNotFound, Is.EqualTo(2000));
        Assert.That((int)SystemError.UserAlreadyExists, Is.EqualTo(2001));
        Assert.That((int)SystemError.InvalidCredentials, Is.EqualTo(2002));
        Assert.That((int)SystemError.Unauthorized, Is.EqualTo(2003));
    }

    [Test]
    public void Task1_2_IResult_ShouldNotHaveStatusCodeProperty()
    {
        var property = typeof(PACHA_FIT.Core.Domain.Shared.ResultPattern.IResult).GetProperty("StatusCode");
        Assert.That(property, Is.Null, "IResult should not have StatusCode property anymore.");
    }

    [Test]
    public void Task1_3_Result_ShouldNotHaveStatusCodeProperty()
    {
        var property = typeof(PACHA_FIT.Core.Domain.Shared.ResultPattern.Result<int>).GetProperty("StatusCode");
        Assert.That(property, Is.Null, "Result<T> should not have StatusCode property anymore.");
    }

    [Test]
    public void Task1_3_Result_ShouldNotHaveLegacyFailureOverloads()
    {
        // Get all Failure methods on Result<int>
        var methods = typeof(PACHA_FIT.Core.Domain.Shared.ResultPattern.Result<int>)
            .GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
            .Where(m => m.Name == "Failure");

        // Verify that there is no Failure method that takes (string, ErrorType)
        var hasLegacyOverload = methods.Any(m =>
        {
            var parameters = m.GetParameters();
            return parameters.Length == 2 &&
                   parameters[0].ParameterType == typeof(string) &&
                   parameters[1].ParameterType.Name == "ErrorType";
        });

        Assert.That(hasLegacyOverload, Is.False, "Result<T> should not have legacy Failure(string, ErrorType) overload anymore.");
    }

    [Test]
    public void Task1_4_ErrorTypes_ShouldBeDeleted()
    {
        var type = typeof(PACHA_FIT.Core.Domain.Shared.SystemError).Assembly.GetType("PACHA_FIT.Core.Domain.Shared.ErrorType");
        Assert.That(type, Is.Null, "ErrorType enum should be deleted entirely.");
    }

    [Test]
    public void Task2_2_ToHttpStatusCode_ShouldMapSecurityErrorCodesCorrectly()
    {
        Assert.That(SystemError.UserNotFound.ToHttpStatusCode(), Is.EqualTo(HttpStatusCode.NotFound));
        Assert.That(SystemError.UserAlreadyExists.ToHttpStatusCode(), Is.EqualTo(HttpStatusCode.Conflict));
        Assert.That(SystemError.InvalidCredentials.ToHttpStatusCode(), Is.EqualTo(HttpStatusCode.Unauthorized));
        Assert.That(SystemError.Unauthorized.ToHttpStatusCode(), Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public void Task1_5_SuccessCode_Enum_ShouldExistWithCorrectValues()
    {
        // Compile-time RED state check for SuccessCode values
        Assert.That((int)SuccessCode.Ok, Is.EqualTo(200));
        Assert.That((int)SuccessCode.Created, Is.EqualTo(201));
        Assert.That((int)SuccessCode.NoContent, Is.EqualTo(204));
    }

    [Test]
    public void Task1_6_IResult_And_Result_ShouldHaveSuccessStatus_And_Helpers()
    {
        // Compile-time RED state check for SuccessStatus and Created/NoContent factory helpers
        var successResult = PACHA_FIT.Core.Domain.Shared.ResultPattern.Result<string>.Success("test");
        Assert.That(successResult.SuccessStatus, Is.EqualTo(SuccessCode.Ok));

        var createdResult = PACHA_FIT.Core.Domain.Shared.ResultPattern.Result<string>.Created("created_test");
        Assert.That(createdResult.SuccessStatus, Is.EqualTo(SuccessCode.Created));
        Assert.That(createdResult.Value, Is.EqualTo("created_test"));

        var noContentResult = PACHA_FIT.Core.Domain.Shared.ResultPattern.Result<string>.NoContent();
        Assert.That(noContentResult.SuccessStatus, Is.EqualTo(SuccessCode.NoContent));
    }

    [Test]
    public void Task4_2_ResultMappingMiddleware_ShouldMapSuccessStatusCorrectly()
    {
        // Compile-time RED state check for ResultMappingMiddleware.ResolveStatusCode mapping
        var okResult = PACHA_FIT.Core.Domain.Shared.ResultPattern.Result<string>.Success("ok");
        var createdResult = PACHA_FIT.Core.Domain.Shared.ResultPattern.Result<string>.Created("created");
        var noContentResult = PACHA_FIT.Core.Domain.Shared.ResultPattern.Result<string>.NoContent();

        Assert.That(ResultMappingMiddleware.ResolveStatusCode(okResult), Is.EqualTo(HttpStatusCode.OK));
        Assert.That(ResultMappingMiddleware.ResolveStatusCode(createdResult), Is.EqualTo(HttpStatusCode.Created));
        Assert.That(ResultMappingMiddleware.ResolveStatusCode(noContentResult), Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task Task3_9_ProductsService_CreateProduct_ShouldReturnCreatedResult()
    {
        var productRepo = Substitute.For<IProductRepository>();
        var stockMovementRepo = Substitute.For<IStockMovementRepository>();
        var productsService = new ProductsService(productRepo, stockMovementRepo);

        var request = new ProductCreateRequest(
            Name: "Test Product",
            SKU: "TEST-SKU",
            CategoryId: 1,
            CostPrice: 10.0m,
            SalePrice: 15.0m,
            InitialStock: 5,
            InitialUnitAbbreviation: "g"
        );

        productRepo.ExistsSku(request.SKU).Returns(false);
        var expectedResponse = new ProductResponse(1, "Test Product", "TEST-SKU", 5, 0, false, null, null);
        productRepo.SaveProduct(request).Returns(expectedResponse);

        var result = await productsService.CreateProduct(request);

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.SuccessStatus, Is.EqualTo(SuccessCode.Created));
    }

    [Test]
    public async Task Task3_10_AuthService_SignUp_ShouldReturnCreatedResult()
    {
        var credentialService = Substitute.For<ICredentialService>();
        var userRepo = Substitute.For<IUserRepository>();
        var passwordService = Substitute.For<IPasswordService>();
        var authService = new AuthService(credentialService, userRepo, passwordService);

        var registration = new NewUserRegistration(
            Email: "new@example.com",
            Password: "Password123",
            FullName: "New User"
        );

        userRepo.GetInternalUserAsync(registration.Email).Returns(Task.FromResult<InternalUserResponse?>(null));
        passwordService.HashPassword(registration.Password).Returns("hashed_Password123");

        var result = await authService.SignUp(registration);

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.SuccessStatus, Is.EqualTo(SuccessCode.Created));
    }
}


