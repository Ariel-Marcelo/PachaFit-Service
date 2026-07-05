using NSubstitute;
using PACHA_FIT.Core.Application.Inventory;
using PACHA_FIT.Core.Application.User;
using PACHA_FIT.Core.Domain.Inventory.Ports;
using PACHA_FIT.Core.Domain.User.Ports;

namespace PACHA_FIT.BddTests.Steps;

public class ScenarioDependencies
{
    // Inventory
    public IProductRepository ProductRepository { get; } = Substitute.For<IProductRepository>();
    public IStockMovementRepository StockMovementRepository { get; } = Substitute.For<IStockMovementRepository>();
    public IUnitOfMeasureRepository UnitOfMeasureRepository { get; } = Substitute.For<IUnitOfMeasureRepository>();
    public ProductsService ProductsService { get; }
    public InventoryService InventoryService { get; }
    public UnitOfMeasureService UnitOfMeasureService { get; }

    // User
    public IUserRepository UserRepository { get; } = Substitute.For<IUserRepository>();
    public IPasswordService PasswordService { get; } = Substitute.For<IPasswordService>();
    public ICredentialService CredentialService { get; } = Substitute.For<ICredentialService>();
    public UserService UserService { get; }
    public AuthService AuthService { get; }

    public ScenarioDependencies()
    {
        ProductsService = new ProductsService(ProductRepository, StockMovementRepository);
        InventoryService = new InventoryService(StockMovementRepository);
        UnitOfMeasureService = new UnitOfMeasureService(UnitOfMeasureRepository);

        UserService = new UserService(UserRepository, PasswordService);
        AuthService = new AuthService(CredentialService, UserRepository, PasswordService);
    }
}
