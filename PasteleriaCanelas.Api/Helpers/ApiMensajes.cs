namespace PasteleriaCanelas.Api.Helpers;


public static class ApiMensajes
{
    // Mensajes de NotFound (404)
    public const string CategoriaNoEncontrada = "La categoría no fue encontrada.";
    public const string ProductoNoEncontrado = "El producto no fue encontrado.";
    public const string PrecioNoEncontrado = "El precio no fue encontrado.";
    public const string RecursoNoEncontrado = "El recurso solicitado no fue encontrado.";
    public const string ProductoParaPrecioNoEncontrado = "El producto al que intenta agregar el precio no fue encontrado.";

    // Mensajes de BadRequest (400)
    public const string IdInvalido = "El ID en la URL no coincide con el ID del cuerpo de la solicitud.";
    public const string CreacionCategoriaFallida = "No se pudo crear la categoría.";
    public const string CategoriaParaProductoNoExiste = "La categoría especificada no existe.";
    public const string CategoriaConProductos = "No se puede eliminar la categoría porque tiene productos asociados.";
}
