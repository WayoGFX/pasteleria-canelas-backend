using System.Text.RegularExpressions;
using System.Globalization;

namespace PasteleriaCanelas.Services.Utilities;

// funcion para generar Slug
public static class SlugGenerator
{
    public static string GenerateSlug(string phrase)
    {
        string str = phrase.ToLower();

        // Eliminar caracteres que no sean letras, números, espacios o guiones.
        str = Regex.Replace(str, @"[^a-z0-9\s-]", "");

        // Reemplazar espacios por guiones.
        str = Regex.Replace(str, @"\s+", " ").Trim();
        str = str.Replace(" ", "-");

        // Eliminar guiones duplicados.
        str = Regex.Replace(str, @"-+", "-");
        
        return str;
    }
}