using System.Globalization;
using Lexer;

namespace ArithmeticParser.Tests;

[TestClass]
public sealed class ParserTests
{
    [DataTestMethod]
    [DataRow(123)]
    [DataRow(-27)]
    [DataRow(+81)]
    [DataRow(0.5)]
    [DataRow(int.MaxValue)]
    public void Parse_SingleNumber_CorrectEvaluationAndTree(double number)
    {
        // Arrange
        var token = new Token<TokenType>(TokenType.Number, number.ToString(CultureInfo.InvariantCulture));
        var parser = new Parser([token]);

        // Act
        var expression = parser.Parse();
        var result = expression.Evaluate();
        
        // Assert
        Assert.AreEqual(number, result);
        Assert.IsInstanceOfType<LiteralExpression>(expression);
    }
    
    [DataTestMethod]
    [DataRow(1, -2, -1)]
    [DataRow(-2, 5, 3)]
    [DataRow(9, 8, 17)]
    [DataRow(0.2, 0.5, 0.7)]
    public void Parse_Addition_CorrectEvaluationAndTree(double left, double right, double expected)
    {
        // Arrange
        var tokens = new Token<TokenType>[]
        {
            new(TokenType.Number, left.ToString(CultureInfo.InvariantCulture)),
            new(TokenType.Plus, "+"),
            new(TokenType.Number, right.ToString(CultureInfo.InvariantCulture))
        };
        
        var parser = new Parser(tokens);

        // Act
        var expression = parser.Parse();
        var result = expression.Evaluate();
        
        
        // Assert
        Assert.AreEqual(expected, result);
        
        // Root should be Binary with Plus
        Assert.IsInstanceOfType<BinaryExpression>(expression);
        var root = (BinaryExpression)expression;
        Assert.AreEqual(TokenType.Plus, root.Operator.Type);
        
        // Left child of root should be Literal equal to left
        Assert.IsInstanceOfType<LiteralExpression>(root.Left);
        Assert.AreEqual(((LiteralExpression)root.Left).Value, left);
        
        
        // Right child of root should be Literal equal to right
        Assert.IsInstanceOfType<LiteralExpression>(root.Right);
        Assert.AreEqual(((LiteralExpression)root.Right).Value, right);
    }

    [DataTestMethod]
    [DataRow(5, -2, 7)]
    [DataRow(-2, 5, -7)]
    [DataRow(9, 8, 1)]
    [DataRow(9.2, 8.7, 0.5)]
    public void Parse_Subtraction_CorrectEvaluationAndTree(double left, double right, double expected)
    {
        // Arrange
        var tokens = new Token<TokenType>[]
        {
            new(TokenType.Number, left.ToString(CultureInfo.InvariantCulture)),
            new(TokenType.Minus, "-"),
            new(TokenType.Number, right.ToString(CultureInfo.InvariantCulture))
        };
            
        var parser = new Parser(tokens);
    
        // Act
        var expression = parser.Parse();
        var result = expression.Evaluate();
            
            
        // Assert
        Assert.AreEqual(expected, result);
            
        // Root should be Binary with Minus
        Assert.IsInstanceOfType<BinaryExpression>(expression);
        var binary = (BinaryExpression)expression;
        Assert.AreEqual(TokenType.Minus, binary.Operator.Type);
            
        // Left child of root should be Literal equal to left
        Assert.IsInstanceOfType<LiteralExpression>(binary.Left);
        Assert.AreEqual(((LiteralExpression)binary.Left).Value, left);
            
            
        // Right child of root should be Literal equal to right
        Assert.IsInstanceOfType<LiteralExpression>(binary.Right);
        Assert.AreEqual(((LiteralExpression)binary.Right).Value, right);
    }
    
    [DataTestMethod]
    [DataRow(5, -2, -10)]
    [DataRow(-2, 5, -10)]
    [DataRow(4, 3, 12)]
    [DataRow(2.5, 3, 7.5)]
    public void Parse_Multiplication_CorrectEvaluationAndTree(double left, double right, double expected)
    {
        // Arrange
        var tokens = new Token<TokenType>[]
        {
            new(TokenType.Number, left.ToString(CultureInfo.InvariantCulture)),
            new(TokenType.Multiply, "*"),
            new(TokenType.Number, right.ToString(CultureInfo.InvariantCulture))
        };
            
        var parser = new Parser(tokens);
    
        // Act
        var expression = parser.Parse();
        var result = expression.Evaluate();
            
            
        // Assert
        Assert.AreEqual(expected, result);
            
        // Root should be Binary with Multiply
        Assert.IsInstanceOfType<BinaryExpression>(expression);
        var binary = (BinaryExpression)expression;
        Assert.AreEqual(TokenType.Multiply, binary.Operator.Type);
            
        // Left child of root should be Literal equal to left
        Assert.IsInstanceOfType<LiteralExpression>(binary.Left);
        Assert.AreEqual(((LiteralExpression)binary.Left).Value, left);
            
            
        // Right child of root should be Literal equal to right
        Assert.IsInstanceOfType<LiteralExpression>(binary.Right);
        Assert.AreEqual(((LiteralExpression)binary.Right).Value, right);
    }

    [DataTestMethod]
    [DataRow(4, -2, -2)]
    [DataRow(-9, 3, -3)]
    [DataRow(4, 4, 1)]
    [DataRow(5, 2, 2.5)]
    public void Parse_Division_CorrectEvaluationAndTree(double left, double right, double expected)
    {
        // Arrange
        var tokens = new Token<TokenType>[]
        {
            new(TokenType.Number, left.ToString(CultureInfo.InvariantCulture)),
            new(TokenType.Divide, "/"),
            new(TokenType.Number, right.ToString(CultureInfo.InvariantCulture))
        };

        var parser = new Parser(tokens);

        // Act
        var expression = parser.Parse();
        var result = expression.Evaluate();


        // Assert
        Assert.AreEqual(expected, result);

        // Root should be binary with Divide
        Assert.IsInstanceOfType<BinaryExpression>(expression);
        var binary = (BinaryExpression)expression;
        Assert.AreEqual(TokenType.Divide, binary.Operator.Type);

        // Left child of root should be Literal equal to left
        Assert.IsInstanceOfType<LiteralExpression>(binary.Left);
        Assert.AreEqual(((LiteralExpression)binary.Left).Value, left);


        // Right child of root should be Literal equal to right
        Assert.IsInstanceOfType<LiteralExpression>(binary.Right);
        Assert.AreEqual(((LiteralExpression)binary.Right).Value, right);
    }

    [TestMethod]
    public void Parse_Expression_CorrectEvaluationAndTree()
    {
        // Arrange
        var tokens = new Token<TokenType>[]
        {
            new(TokenType.LeftParenthesis, "("),
            new(TokenType.Number, "4"),
            new(TokenType.Multiply, "*"),
            new(TokenType.Number, "-2.5"),
            new(TokenType.RightParenthesis, ")"),
            new(TokenType.Divide, "/"),
            new(TokenType.Number, "2"),
        };
    
        var parser = new Parser(tokens);
    
        // Act
        var expression = parser.Parse();
        var result = expression.Evaluate();
    
        
        // Assert
        Assert.AreEqual(-5, result);
    
        // Root should be Binary with Divide
        Assert.IsInstanceOfType<BinaryExpression>(expression);
        var root = (BinaryExpression)expression;
        Assert.AreEqual(TokenType.Divide, root.Operator.Type);
        
        // Right child of root should be Literal(2)
        Assert.IsInstanceOfType<LiteralExpression>(root.Right);
        Assert.AreEqual(2, ((LiteralExpression)root.Right).Value);
        
        // Left child of root should be Binary with Multiply
        Assert.IsInstanceOfType<BinaryExpression>(root.Left);
        var leftNode = (BinaryExpression)root.Left;
        Assert.AreEqual(TokenType.Multiply, leftNode.Operator.Type);
        
        // Left child of leftNode should be Literal(3)
        Assert.IsInstanceOfType<LiteralExpression>(leftNode.Left);
        Assert.AreEqual(4, ((LiteralExpression)leftNode.Left).Value);
        
        // Right child of leftNode should be Literal(-2)
        Assert.IsInstanceOfType<LiteralExpression>(leftNode.Right);
        Assert.AreEqual(-2.5, ((LiteralExpression)leftNode.Right).Value);
    }
}