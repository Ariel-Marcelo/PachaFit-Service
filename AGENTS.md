# PachaFit Coding Standards

## C# / .NET
- Use modern C# features (C# 12 / .NET 8).
- Keep Azure Function entry points thin.
- Business logic belongs in Application Services, not in Functions.
- All endpoints must map requests/responses through OpenAPI/NSwag contract first.
- Domain operations must return a `Result<T>` pattern object.

## Testing
- BDD testing via Reqnroll and NUnit.
- Tests belong in `PACHA_FIT.BddTests`.
- Verify behavior, not implementation details.
