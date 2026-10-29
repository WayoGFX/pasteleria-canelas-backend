# Pastelería Canelas - Backend API

> 🌐 **Aplicación en Vivo (Frontend):** [https://pasteleria-canelas-frontend.vercel.app/](https://pasteleria-canelas-frontend.vercel.app/)

Backend RESTful desarrollado en **.NET 8** con arquitectura en capas para la gestión del catálogo, categorías, productos y precios de la pastelería **Canelas**. Diseñado para ofrecer alto rendimiento, desacoplamiento y compatibilidad directa con despliegues en la nube (Docker / Railway / PostgreSQL).

---

## 🌐 Demo y Aplicación en Producción

Puedes ver la aplicación completa en funcionamiento conectada a este backend a través de la interfaz web desplegada en Vercel:

🔗 **Demo en Vivo:** [pasteleria-canelas-frontend.vercel.app](https://pasteleria-canelas-frontend.vercel.app/)

---

## Arquitectura del Proyecto

El proyecto sigue los principios de separación de responsabilidades y arquitectura por capas:

```text
PasteleriaCanelas-backend/
├── PasteleriaCanelas.Api/          # Capa de Presentación: Controladores REST, Swagger, Middlewares y configuración
├── PasteleriaCanelas.Services/     # Capa de Aplicación: Lógica de negocio, DTOs, Interfaces y Utilidades (SlugGenerator)
├── PasteleriaCanelas.Data/         # Capa de Datos: Entity Framework Core, DbContext, configuraciones de mapeo
├── PasteleriaCanelas.Domain/       # Capa de Dominio: Entidades principales (Producto, Categoria, ProductoPrecio)
└── PasteleriaCanelas.Tests/        # Pruebas Unitarias: xUnit, FluentAssertions, EF Core In-Memory
```

---

## Tecnologías y Herramientas

* **Plataforma:** [.NET 8 (C#)](https://dotnet.microsoft.com/)
* **ORM:** [Entity Framework Core 8](https://learn.microsoft.com/ef/core/)
* **Base de Datos:** [PostgreSQL](https://www.postgresql.org/) (usando `Npgsql.EntityFrameworkCore.PostgreSQL`)
* **Documentación:** Swagger / OpenAPI (`Swashbuckle.AspNetCore`)
* **Testing:** [xUnit](https://xunit.net/), [FluentAssertions](https://fluentassertions.com/) y `Microsoft.EntityFrameworkCore.InMemory`
* **Contenedores:** Docker (construcción multi-stage)
* **Despliegue:** Preparado para Railway / Render / Azure

---

## Características Principales

* **Generación automática de Slugs:** Genera slugs amigables para SEO a partir del nombre del producto o categoría (`SlugGenerator`).
* **Optimización y Rendimiento:**
  * Compresión de respuestas HTTP mediante **Brotli** y **Gzip**.
  * Respuestas optimizadas con consultas de solo lectura (`AsNoTracking()`).
  * Caché en memoria integrada (`IMemoryCache`).
* **Validaciones y Respuestas Uniformes:**
  * Configuración personalizada de `ValidationProblemDetails` para respuestas `400 Bad Request`.
  * Mensajes de error y respuestas centralizados en `ApiMensajes`.
* **Soporte Cloud Nativo:**
  * Detección y conversión automática de strings de conexión `DATABASE_URL` (formato Railway/Heroku).
  * Adaptación dinámica del puerto Kestrel en entornos de producción.

---

## Endpoints de la API

### Catálogo (Público / Clientes)
| Método | Endpoint | Descripción |
| :--- | :--- | :--- |
| `GET` | `/api/Catalogo/inicial` | Obtiene el catálogo completo organizado para la vista inicial del cliente. |

### Categorías
| Método | Endpoint | Descripción |
| :--- | :--- | :--- |
| `GET` | `/api/Categorias` | Lista todas las categorías activas. |
| `GET` | `/api/Categorias/{id}` | Obtiene el detalle de una categoría por su ID. |
| `POST` | `/api/Categorias` | Crea una nueva categoría. |
| `PUT` | `/api/Categorias/{id}` | Actualiza los datos de una categoría. |
| `DELETE` | `/api/Categorias/{id}` | Elimina una categoría (valida que no tenga productos asociados). |

### Productos
| Método | Endpoint | Descripción |
| :--- | :--- | :--- |
| `GET` | `/api/Productos` | Lista todos los productos disponibles con sus detalles y precios. |
| `GET` | `/api/Productos/{id}` | Obtiene un producto por su ID. |
| `POST` | `/api/Productos` | Crea un nuevo producto (con opción de asignar precios iniciales). |
| `PUT` | `/api/Productos/{id}` | Actualiza un producto existente. |
| `DELETE` | `/api/Productos/{id}` | Elimina o desactiva un producto. |

### Precios y Porciones
| Método | Endpoint | Descripción |
| :--- | :--- | :--- |
| `POST` | `/api/Precios` | Agrega un nuevo precio/tamaño a un producto. |
| `PUT` | `/api/Precios/{id}` | Modifica un precio existente. |
| `DELETE` | `/api/Precios/{id}` | Elimina una opción de precio. |

---

## Requisitos Previos

* [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) instalado.
* Instancia de [PostgreSQL](https://www.postgresql.org/) (local o remota).
* [Docker](https://www.docker.com/) *(opcional, para ejecución con contenedores)*.

---

## Configuración Local

1. **Clonar el repositorio:**
   ```bash
   git clone <URL_DEL_REPOSITORIO>
   cd PasteleriaCanelas-backend
   ```

2. **Configurar la cadena de conexión:**
   Edita el archivo `PasteleriaCanelas.Api/appsettings.Development.json` o `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=pasteleria_db;Username=postgres;Password=tu_password"
     }
   }
   ```

3. **Restaurar paquetes y compilar:**
   ```bash
   dotnet restore
   dotnet build
   ```

4. **Ejecutar la API:**
   ```bash
   dotnet run --project PasteleriaCanelas.Api
   ```

5. **Acceder a Swagger:**
   Abre tu navegador en:
   ```text
   http://localhost:5000/swagger
   # o la URL asignada en tus launchSettings (ej. https://localhost:7148/swagger)
   ```

---

## Pruebas Unitarias

Para ejecutar la suite de pruebas unitarias automatizadas:

```bash
# Ejecutar todas las pruebas
dotnet test

# Ejecutar con salida detallada
dotnet test --logger "console;verbosity=detailed"
```

Los tests cubren:
* **Funciones de utilidad:** Generación y saneamiento de slugs (`SlugGeneratorTests`).
* **Lógica de negocio en servicios:** Validaciones de eliminación y reglas relacionales (`CategoriaServiceTests`, `ProductoServiceTests`) utilizando una base de datos en memoria para evitar tocar la base de datos real.

---

## Despliegue con Docker

El proyecto incluye un `Dockerfile` multi-stage optimizado para producción.

1. **Construir la imagen:**
   ```bash
   docker build -t pasteleria-canelas-api .
   ```

2. **Ejecutar el contenedor:**
   ```bash
   docker run -d -p 8080:8080 -e DATABASE_URL="postgres://usuario:clave@host:5432/bd" pasteleria-canelas-api
   ```
