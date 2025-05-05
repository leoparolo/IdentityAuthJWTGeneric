# IdentityAuthJWTGeneric
Este módulo permite integrar autenticación basada en JWT utilizando ASP.NET Identity con configuración mínima.

---

## Instalación

1. Bajarte el repositorio

2. Referenciar la biblioteca del módulo desde tu API principal:
   * Tambien se puede Añadir los test a tu proyecto
---

## Configuración del `appsettings.json`

Agregá las siguientes secciones en tu archivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "AuthConnection": "ConnectionString"
  },
  "Jwt": {
    "Secret": "TuClaveDeAlMenos16Caracteres",
    "Issuer": "TuApiEmisora",
    "Audience": "TuApiConsumidora"
  }
}
```

---

## Uso en `Program.cs`

Registrá el módulo de autenticación dentro del `Program.cs` o `Startup.cs`:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthModule(builder.Configuration);
```

---

## Middleware de autenticación

Asegurate de habilitar el middleware en el orden correcto:

```csharp
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
```

---


### Cómo proteger endpoints

Usá el atributo `[Authorize]` en tus controladores o métodos:

```csharp
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PerfilController : ControllerBase
{
    [HttpGet]
    public IActionResult GetPerfil()
    {
        return Ok("Estás autenticado");
    }
}
```

---
### Requisitos de contraseña por defecto

* Mínimo 5 caracteres.
* No requiere números, mayúsculas ni símbolos.
* Puede cambiarse desde el método `ConfigureIdentityOptions` en la biblioteca.

---

### Pruebas unitarias

El módulo ya incluye pruebas automáticas para validar:

* Faltantes en la configuración del JWT.
* Faltantes en la cadena de conexión.
* Configuración válida sin errores.

---

### Personalización

Podés extender el módulo:

* Implementando `IAuthService` con tus propios métodos.
* Reemplazando `IdentityUser` por una clase propia si necesitás más propiedades.
* Cambiando la base de datos por MySQL, PostgreSQL u otro proveedor modificando `AddDbContext`.

---
