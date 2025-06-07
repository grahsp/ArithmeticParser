using Lexer;

namespace ArithmeticParser.Tests;

[TestClass]
public sealed class ParserTests
{
    [DataTestMethod]
    [DataRow(0)]
    [DataRow(125)]
    [DataRow(int.MaxValue)]
    public void Parse_ParseNumber(int source)
    {
        // Arrange
        var tokens = new Token<ArithmeticType>[]
            { new(ArithmeticType.Number, source.ToString()) };
        
        var parser = new ArithmeticParser(tokens);

        // Act
        var expression = parser.Parse();
        var result = expression.Evaluate();
        
        // Assert
        Assert.AreEqual(source, result);
    }
}























