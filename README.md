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
This project uses **Local Dotnet Tools**. Before running any command, restore them once:
```powershell
dotnet tool restore
```

#### 💻 Useful Commands

| Action | Command |
| :--- | :--- |
| **Add a new migration** | `dotnet dotnet-ef migrations add NameOfMigration --project PACHA_FIT` |
| **Update Local Database** | `dotnet dotnet-ef database update --project PACHA_FIT` |
| **Update Azure Database** | `dotnet dotnet-ef database update --project PACHA_FIT --connection "YOUR_AZURE_CONNECTION_STRING"` |

---

### 🧪 BDD Testing & Quality

We use **Behavior-Driven Development (BDD)** with **Reqnroll** and **NUnit** to ensure the business logic is correctly implemented and verified.

#### 🚀 Running Tests and Coverage

To execute all BDD tests and generate a professional code coverage report, use the provided helper script:

```powershell
./run-tests.ps1
```

This script will:
1. Build the project.
2. Execute all tests in the `PACHA_FIT.BddTests` project.
3. Collect code coverage data.
4. Generate an interactive HTML report at `./TestResults/CoverageReport/index.html`.

#### 📂 Test Organization
*   **Features**: Located in `PACHA_FIT.BddTests/Features/`. Files are organized by domain (e.g., `User/`).
*   **Steps**: Located in `PACHA_FIT.BddTests/Steps/`. Contains the implementation of the Gherkin steps.

#### ⚠️ Git Guidelines
*   **Do NOT commit** `.feature.cs` files. They are automatically generated during build.
*   The project is configured to ignore these files via `.gitignore`.


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
