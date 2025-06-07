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
    
    [DataTestMethod]
    [DataRow("1", "2", 3)]
    [DataRow("0", "5", 5)]
    [DataRow("9", "0", 9)]
    public void Parse_ExpressionContainingAddition(string left, string right, int expected)
    {
        // Arrange
        var tokens = new Token<ArithmeticType>[]
        {
            new(ArithmeticType.Number, left),
            new(ArithmeticType.Plus, "+"),
            new(ArithmeticType.Number, right)
        };
        
        var parser = new ArithmeticParser(tokens);

        // Act
        var expression = parser.Parse();
        var result = expression.Evaluate();
        
        // Assert
        Assert.AreEqual(expected, result);
    }
    
    [DataTestMethod]
    [DataRow("1", "2", -1)]
    [DataRow("8", "5", 3)]
    [DataRow("9", "0", 9)]
    public void Parse_ExpressionContainingSubtraction(string left, string right, int expected)
    {
        // Arrange
        var tokens = new Token<ArithmeticType>[]
        {
            new(ArithmeticType.Number, left),
            new(ArithmeticType.Minus, "-"),
            new(ArithmeticType.Number, right)
        };
        
        var parser = new ArithmeticParser(tokens);

        // Act
        var expression = parser.Parse();
        var result = expression.Evaluate();
        
        // Assert
        Assert.AreEqual(expected, result);
    }
    
    
    [DataTestMethod]
    [DataRow("1", "2", 2)]
    [DataRow("0", "5", 0)]
    [DataRow("2", "10", 20)]
    public void Parse_ExpressionContainingMultiplication(string left, string right, int expected)
    {
        // Arrange
        var tokens = new Token<ArithmeticType>[]
        {
            new(ArithmeticType.Number, left),
            new(ArithmeticType.Multiply, "*"),
            new(ArithmeticType.Number, right)
        };
        
        var parser = new ArithmeticParser(tokens);

        // Act
        var expression = parser.Parse();
        var result = expression.Evaluate();
        
        // Assert
        Assert.AreEqual(expected, result);
    }
    
    [DataTestMethod]
    [DataRow("2", "2", 1)]
    [DataRow("9", "3", 3)]
    [DataRow("33", "3", 11)]
    public void Parse_ExpressionContainingDivision(string left, string right, int expected)
    {
        // Arrange
        var tokens = new Token<ArithmeticType>[]
        {
            new(ArithmeticType.Number, left),
            new(ArithmeticType.Divide, "/"),
            new(ArithmeticType.Number, right)
        };
        
        var parser = new ArithmeticParser(tokens);

        // Act
        var expression = parser.Parse();
        var result = expression.Evaluate();
        
        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Parse_ExpressionContainingParantheses()
    {
         // Arrange
         var tokens = new Token<ArithmeticType>[]
         {
             new(ArithmeticType.Number, "2"),
             new(ArithmeticType.Multiply, "*"),
             new(ArithmeticType.LeftParanthesis, "("),
             new(ArithmeticType.Number, "2"),
             new(ArithmeticType.Plus, "+"),
             new(ArithmeticType.Number, "3"),
             new(ArithmeticType.RightParanthesis, ")"),
         };
         
         var parser = new ArithmeticParser(tokens);
 
         // Act
         var expression = parser.Parse();
         var result = expression.Evaluate();
         
         // Assert
         Assert.AreEqual(10, result);       
    }
}























