using Xunit; // use the xUnit for unity test
using FluentAssertions; // for valitations
using PasteleriaCanelas.Tests;
using PasteleriaCanelas.Services.Utilities;

public class SlugGeneratorTests
{
    [Fact] // indicator for unit test
    public void GenerateSlug_TextoConEspacios_DebeRetornarTextoConGuinesYMinusculas()
    {
        // fase 1 . Arrange (Preparar)
        string input = "Pastel de Chocolate y Fresa";

        // fase 2 . Act (Actuar)
        string result = SlugGenerator.GenerateSlug(input);

        // fase 3 . Assert (Verificar)
        result.Should().Be("pastel-de-chocolate-y-fresa");
    }

    [Theory] // Allow try multiply cases with the same test
    
    [InlineData("Torta Especial!!!", "torta-especial")]
    [InlineData("Panqueques    con miel", "panqueques-con-miel")]
    [InlineData("Café & Canela", "caf-canela")]

    public void GenerateSlug_CasosEspeciales_DebeLimpiarCaracteresCorrectamente(string entrada, string esperado)
    {
        // Act
        var result = SlugGenerator.GenerateSlug(entrada);

        // Assert
        result.Should().Be(esperado);
    }
}