# PachaFit Service - API Documentation

## 🚀 OpenAPI Design-First Workflow

This project follows an **OpenAPI Design-First** approach. All API endpoints, request/response models, and validation rules are defined in the `docs/openapi.yaml` file and automatically translated into C# code.

### 🛠️ Migration & Development Steps

1.  **Define the Contract**: Modify `PACHA_FIT/docs/openapi.yaml` to add or update endpoints, parameters, and schemas.
2.  **Generate Code**: Build the project to trigger NSwag code generation.
3.  **Implement the Logic**:
    *   Create or update a Function class in `PACHA_FIT/src/Api/Functions/`.
    *   Inherit from the generated `PachaFitControllerBase` class.
    *   Override the generated abstract methods to implement your business logic.
4.  **Middleware Integration**: The generated `ResultDto<T>` classes automatically implement the `IResult` interface (via `src/Core/Domain/Generated/ResultDtoExtensions.cs`) to work seamlessly with the project's custom `ResultMappingMiddleware`.

---

### 💻 Useful Commands

| Command | Description |
| :--- | :--- |
| `dotnet build` | Builds the project and **automatically** runs NSwag generation before compiling. |
| `dotnet build /t:NSwag` | Runs **only** the NSwag generation without a full project build. |

---

### 📂 File Structure

*   **Contract**: `PACHA_FIT/docs/openapi.yaml` (The "Source of Truth")
*   **NSwag Config**: `PACHA_FIT/nswag.json`
*   **Generated Code**: `PACHA_FIT/src/Core/Domain/Generated/PachaFitApi.cs` (Do not edit manually!)
*   **Extensions**: `PACHA_FIT/src/Core/Domain/Generated/ResultDtoExtensions.cs` (Used for custom interfaces like `IResult`)

---

### 📖 Example Implementation

```csharp
public class UserFunc : PachaFitControllerBase
{
    // The Azure Function Entry Point
    [Function("GetUsers")]
    public async Task<ResultDtoOfUserResponseDto> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
    {
        // Simply delegate to the implemented method
        return await this.GetUsers();
    }

    // Your actual implementation
    public override async Task<ResultDtoOfUserResponseDto> GetUsers(CancellationToken ct = default)
    {
        // Business logic here...
    }
}
```
