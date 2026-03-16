# PachaFit Service - API Documentation

## 🚀 OpenAPI Design-First Workflow

This project follows an **OpenAPI Design-First** approach. All API endpoints, request/response models, and validation rules are defined in the `docs/openapi.yaml` file and automatically translated into C# code.

### 🛠️ Migration & Development Steps

1.  **Define the Contract**: Modify `PACHA_FIT/docs/openapi.yaml` to add or update endpoints, parameters, and schemas. Use prefixes in `operationId` (e.g., `User_GetUsers`, `Auth_Login`) to automatically group operations into separate controllers.
2.  **Generate Code**: Build the project to trigger NSwag code generation.
3.  **Implement the Logic**:
    *   Create or update a Function class in `PACHA_FIT/src/Api/Functions/`.
    *   Inherit from the generated base class (e.g., `UserControllerBase`, `AuthControllerBase`).
    *   Override the generated abstract methods to implement your business logic.
4.  **Middleware Integration**: The generated `ResultDto<T>` classes automatically implement the `IResult` interface (via `src/Core/Domain/Generated/ResultDtoExtensions.cs`) to work seamlessly with the project's custom `ResultMappingMiddleware`.

---

### 💻 Useful Commands

| Command | Description |
| :--- | :--- |
| `dotnet build` | Builds the project and **automatically** runs NSwag generation before compiling. |
| `dotnet build /t:NSwag` | Runs **only** the NSwag generation without a full project build. |

---

### 🗄️ Database Migrations (EF Core)

This project uses Entity Framework Core Migrations to manage the database schema.

#### 🛠️ Prerequisites
Ensure you have the `dotnet-ef` tool installed globally:
```powershell
dotnet tool install --global dotnet-ef
```

#### 💻 Useful Commands

| Action | Command |
| :--- | :--- |
| **Add a new migration** | `dotnet ef migrations add NameOfMigration --project PACHA_FIT` |
| **Update Local Database** | `dotnet ef database update --project PACHA_FIT` |
| **Update Azure Database** | `dotnet ef database update --project PACHA_FIT --connection "YOUR_AZURE_CONNECTION_STRING"` |
| **Remove last migration** | `dotnet ef migrations remove --project PACHA_FIT` |
| **Generate SQL Script** | `dotnet ef migrations script --project PACHA_FIT` |

#### ⚠️ Configuration
*   **Local Development**: Add your connection string to `PACHA_FIT/local.settings.json` under the key `"SqlConnectionString"`.
*   **Azure Deployment**: Add an Application Setting named `SqlConnectionString` in the Azure Portal.
*   **Design-Time**: The `PachaFitContextFactory` class allows generating migrations without a live database connection.

---

### 🌐 Swagger UI

You can visualize and test your endpoints using the built-in Swagger UI:

*   **UI URL**: `http://localhost:7071/api/swagger/ui`
*   **Spec URL**: `http://localhost:7071/api/swagger/spec`

*(Note: The port may change if configured differently in `local.settings.json`)*

---

### 📂 File Structure

*   **Contract**: `PACHA_FIT/docs/openapi.yaml` (The "Source of Truth")
*   **NSwag Config**: `PACHA_FIT/nswag.json`
*   **Generated Code**: `PACHA_FIT/src/Core/Domain/Generated/PachaFitApi.cs` (Do not edit manually!)
*   **Extensions**: `PACHA_FIT/src/Core/Domain/Generated/ResultDtoExtensions.cs` (Used for custom interfaces like `IResult`)

---

### 📖 Example Implementation

```csharp
public class UserFunc : UserControllerBase
{
    // The Azure Function Entry Point
    [Function("GetUsers")]
    public async Task<ResultDtoOfUserResponseDto> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users")] HttpRequest req)
    {
        string? email = req.Query["email"];
        // Delegate to the generated abstract method
        return await this.GetUsers(email);
    }

    // Your actual implementation
    public override async Task<ResultDtoOfUserResponseDto> GetUsers(string email, CancellationToken ct = default)
    {
        // Business logic here...
    }
}
```
