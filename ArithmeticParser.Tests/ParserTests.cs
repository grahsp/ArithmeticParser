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
        Assert.AreEqual(((LiteralExpression)root.Left).Literal, left);
        
        
        // Right child of root should be Literal equal to right
        Assert.IsInstanceOfType<LiteralExpression>(root.Right);
        Assert.AreEqual(((LiteralExpression)root.Right).Literal, right);
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
        Assert.AreEqual(((LiteralExpression)binary.Left).Literal, left);
            
            
        // Right child of root should be Literal equal to right
        Assert.IsInstanceOfType<LiteralExpression>(binary.Right);
        Assert.AreEqual(((LiteralExpression)binary.Right).Literal, right);
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
            new(TokenType.Star, "*"),
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
        Assert.AreEqual(TokenType.Star, binary.Operator.Type);
            
        // Left child of root should be Literal equal to left
        Assert.IsInstanceOfType<LiteralExpression>(binary.Left);
        Assert.AreEqual(((LiteralExpression)binary.Left).Literal, left);
            
            
        // Right child of root should be Literal equal to right
        Assert.IsInstanceOfType<LiteralExpression>(binary.Right);
        Assert.AreEqual(((LiteralExpression)binary.Right).Literal, right);
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
            new(TokenType.Slash, "/"),
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
        Assert.AreEqual(TokenType.Slash, binary.Operator.Type);

        // Left child of root should be Literal equal to left
        Assert.IsInstanceOfType<LiteralExpression>(binary.Left);
        Assert.AreEqual(((LiteralExpression)binary.Left).Literal, left);


        // Right child of root should be Literal equal to right
        Assert.IsInstanceOfType<LiteralExpression>(binary.Right);
        Assert.AreEqual(((LiteralExpression)binary.Right).Literal, right);
    }
    
    [DataTestMethod]
    [DataRow(90, 1)]
    [DataRow(0, 0)]
    public void Parse_Sin_CorrectEvaluationAndTree(double value, double expected)
    {
        // Arrange
        var tokens = new Token<TokenType>[]
        {
            new(TokenType.Sin, "sin"),
            new(TokenType.LeftParenthesis, "("),
            new(TokenType.Number, value.ToString(CultureInfo.InvariantCulture)),
            new(TokenType.RightParenthesis, ")"),
        };

        var parser = new Parser(tokens);

        // Act
        var expression = parser.Parse();
        var result = expression.Evaluate();


        // Assert
        Assert.AreEqual(expected, result);
        
        // Root should be Function of Sin
        Assert.IsInstanceOfType<FunctionExpression>(expression);
        var root = (FunctionExpression)expression;
        Assert.AreEqual(TokenType.Sin, root.Function.Type);
    }
    
    [DataTestMethod]
    [DataRow(90, 0)]
    [DataRow(0, 1)]
    public void Parse_Cos_CorrectEvaluationAndTree(double value, double expected)
    {
        // Arrange
        var tokens = new Token<TokenType>[]
        {
            new(TokenType.Cos, "cos"),
            new(TokenType.LeftParenthesis, "("),
            new(TokenType.Number, value.ToString(CultureInfo.InvariantCulture)),
            new(TokenType.RightParenthesis, ")"),
        };

        var parser = new Parser(tokens);

        // Act
        var expression = parser.Parse();
        var result = expression.Evaluate();


        // Assert
        Assert.AreEqual(expected, result);
        
        // Root should be Function of Cos
        Assert.IsInstanceOfType<FunctionExpression>(expression);
        var root = (FunctionExpression)expression;
        Assert.AreEqual(TokenType.Cos, root.Function.Type);
    }
    
    
    [DataTestMethod]
    [DataRow(45, 1)]
    [DataRow(0, 0)]
    public void Parse_Tan_CorrectEvaluationAndTree(double value, double expected)
    {
        // Arrange
        var tokens = new Token<TokenType>[]
        {
            new(TokenType.Tan, "tan"),
            new(TokenType.LeftParenthesis, "("),
            new(TokenType.Number, value.ToString(CultureInfo.InvariantCulture)),
            new(TokenType.RightParenthesis, ")"),
        };

        var parser = new Parser(tokens);

        // Act
        var expression = parser.Parse();
        var result = expression.Evaluate();


        // Assert
        Assert.AreEqual(expected, result);
        
        // Root should be Function of Tan
        Assert.IsInstanceOfType<FunctionExpression>(expression);
        var root = (FunctionExpression)expression;
        Assert.AreEqual(TokenType.Tan, root.Function.Type);
    }

    [TestMethod]
    public void Parse_Pi_CorrectEvaluation()
    {
        var token = new Token<TokenType>(TokenType.Pi, "pi");
        var parser = new Parser([token]);
        
        // Act
        var expression = parser.Parse();
        var result = expression.Evaluate();
        
        // Assert
        Assert.AreEqual(Math.PI, result);
        
        // Root should be Literal
        Assert.IsInstanceOfType<LiteralExpression>(expression);
    }

    [TestMethod]
    public void Parse_Expression_CorrectEvaluationAndTree()
    {
        // Arrange
        var tokens = new Token<TokenType>[]
        {
            new(TokenType.LeftParenthesis, "("),
            new(TokenType.Number, "4"),
            new(TokenType.Star, "*"),
            new(TokenType.Number, "-2.5"),
            new(TokenType.RightParenthesis, ")"),
            new(TokenType.Slash, "/"),
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
        Assert.AreEqual(TokenType.Slash, root.Operator.Type);
        
        // Right child of root should be Literal(2)
        Assert.IsInstanceOfType<LiteralExpression>(root.Right);
        Assert.AreEqual(2, ((LiteralExpression)root.Right).Literal);
        
        // Left child of root should be Binary with Multiply
        Assert.IsInstanceOfType<BinaryExpression>(root.Left);
        var leftNode = (BinaryExpression)root.Left;
        Assert.AreEqual(TokenType.Star, leftNode.Operator.Type);
        
        // Left child of leftNode should be Literal(3)
        Assert.IsInstanceOfType<LiteralExpression>(leftNode.Left);
        Assert.AreEqual(4, ((LiteralExpression)leftNode.Left).Literal);
        
        // Right child of leftNode should be Literal(-2)
        Assert.IsInstanceOfType<LiteralExpression>(leftNode.Right);
        Assert.AreEqual(-2.5, ((LiteralExpression)leftNode.Right).Literal);
    }
}