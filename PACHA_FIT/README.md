# PachaFit Service - API de Entrenamiento y Salud

Este proyecto es el backend de PachaFit, desarrollado con **Azure Functions (.NET 8 Isolated Worker)** y **Entity Framework Core**. Proporciona una arquitectura limpia y robusta para gestionar usuarios, productos, ventas e inventario.

## 🚀 Cómo empezar (Docker)

Para facilitar la ejecución en cualquier dispositivo sin necesidad de instalar SQL Server o Azurite localmente, se ha incluido soporte para Docker.

### Requisitos previos
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado y en ejecución.

### Pasos para ejecutar con Docker Compose

1. **Clonar el repositorio** (si aún no lo has hecho).
2. **Abrir una terminal** en la raíz del proyecto (`PACHA_FIT`).
3. **Ejecutar el siguiente comando**:
   ```bash
   docker-compose up --build
   ```
   Esto levantará tres contenedores:
   - `db`: SQL Server 2022 (Puerto 1433)
   - `azurite`: Emulador de Azure Storage (Puertos 10000-10002)
   - `app`: La aplicación Azure Function (Puerto 8080)

4. **Acceder a la aplicación**:
   - La API estará disponible en: `http://localhost:8080/api/v1/users` (ejemplo)
   - Documentación Swagger UI: `http://localhost:8080/api/swagger/ui`

### 🔍 Diagnóstico
Si recibes un error 404, verifica:
1. Que estás usando el prefijo `/api/v1/` en tus rutas (ej: `/api/v1/users`).
2. Los logs del contenedor con `docker logs -f pacha_fit-app-1` (el nombre puede variar según tu carpeta).

### Aplicar Migraciones de Base de Datos

Para que la base de datos esté sincronizada con el código, debes aplicar las migraciones.

#### Requisito: Instalar dotnet-ef (Versión 9 recomendada)
Si no tienes la herramienta instalada, ejecúta:
```bash
dotnet tool install --global dotnet-ef --version 9.*
```
*Nota: Aunque el proyecto use .NET 8, puedes usar la herramienta v9 para gestionar las migraciones.*

#### Opción A: Desde tu máquina local
Ejecuta el siguiente comando en la carpeta `PACHA_FIT`:
```bash
dotnet ef database update --connection "Server=localhost;Database=PACHA_FIT;User ID=sa;Password=YourStrong!Passw0rd123;Encrypt=False;TrustServerCertificate=True;"
```

#### Opción B: Ejecución Automática (Recomendado)
La aplicación está configurada para aplicar las migraciones automáticamente al iniciar el contenedor `app`. Solo asegúrate de que el contenedor `db` esté saludable.

---

## 🛠️ Tecnologías utilizadas

- **Framework:** .NET 8.0 (Isolated Worker Model)
- **Persistencia:** Entity Framework Core + SQL Server
- **Mapeo:** Riok.Mapperly
- **Seguridad:** JWT (JSON Web Tokens) + BCrypt para contraseñas
- **Documentación:** OpenAPI (Swagger) integrada
- **Infraestructura:** Docker & Docker Compose

## 📂 Estructura del Proyecto

- `src/Api`: Capa de entrada, funciones de Azure, middlewares y validaciones.
- `src/Core/Application`: Lógica de negocio y servicios.
- `src/Core/Domain`: Entidades del dominio, puertos e interfaces.
- `src/Infrastructure`: Implementación de persistencia, repositorios y servicios externos.

## 🔑 Variables de Entorno

Si decides ejecutarlo localmente sin Docker, asegúrate de configurar `local.settings.json`:

| Variable | Descripción |
|----------|-------------|
| `SqlConnectionString` | Cadena de conexión principal a SQL Server |
| `AzureWebJobsStorage` | Conexión a Azure Storage o `UseDevelopmentStorage=true` |
| `JwtSecretKey` | Clave secreta para la generación de tokens JWT |

## 📜 Documentación de la API

PachaFit utiliza NSwag para generar automáticamente la especificación OpenAPI a partir de `docs/openapi.yaml`. Puedes consultar los endpoints disponibles navegando a `/api/swagger/ui` una vez que la aplicación esté en ejecución.

---

Desarrollado con ❤️ para PachaFit.
