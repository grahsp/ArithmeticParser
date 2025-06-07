using Lexer;

namespace ArithmeticParser.Tests;

[TestClass]
public sealed class ParserTests
{
    [DataTestMethod]
    [DataRow(123)]
    [DataRow(-27)]
    [DataRow(+81)]
    [DataRow(int.MaxValue)]
    public void Parse_SingleNumber_CorrectEvaluationAndTree(int number)
    {
        // Arrange
        var token = new Token<ArithmeticType>(ArithmeticType.Number, number.ToString());
        var parser = new ArithmeticParser([token]);

        // Act
        var expression = parser.Parse();
        var result = expression.Evaluate();
        
        // Assert
        Assert.AreEqual(number, result);
        Assert.IsInstanceOfType<Number>(expression);
    }
    
    [DataTestMethod]
    [DataRow(1, -2, -1)]
    [DataRow(-2, 5, 3)]
    [DataRow(9, 8, 17)]
    public void Parse_Addition_CorrectEvaluationAndTree(int left, int right, int expected)
    {
        // Arrange
        var tokens = new Token<ArithmeticType>[]
        {
            new(ArithmeticType.Number, left.ToString()),
            new(ArithmeticType.Plus, "+"),
            new(ArithmeticType.Number, right.ToString())
        };
        
        var parser = new ArithmeticParser(tokens);

        // Act
        var expression = parser.Parse();
        var result = expression.Evaluate();
        
        
        // Assert
        Assert.AreEqual(expected, result);
        
        // Root should be Binary with Plus
        Assert.IsInstanceOfType<Binary>(expression);
        var root = (Binary)expression;
        Assert.AreEqual(ArithmeticType.Plus, root.Operator.Type);
        
        // Left child of root should be Number equal to left
        Assert.IsInstanceOfType<Number>(root.Left);
        Assert.AreEqual(((Number)root.Left).Value, left);
        
        
        // Right child of root should be Number equal to right
        Assert.IsInstanceOfType<Number>(root.Right);
        Assert.AreEqual(((Number)root.Right).Value, right);
    }

    [DataTestMethod]
    [DataRow(5, -2, 7)]
    [DataRow(-2, 5, -7)]
    [DataRow(9, 8, 1)]
    public void Parse_Subtraction_CorrectEvaluationAndTree(int left, int right, int expected)
    {
        // Arrange
        var tokens = new Token<ArithmeticType>[]
        {
            new(ArithmeticType.Number, left.ToString()),
            new(ArithmeticType.Minus, "-"),
            new(ArithmeticType.Number, right.ToString())
        };
            
        var parser = new ArithmeticParser(tokens);
    
        // Act
        var expression = parser.Parse();
        var result = expression.Evaluate();
            
            
        // Assert
        Assert.AreEqual(expected, result);
            
        // Root should be Binary with Minus
        Assert.IsInstanceOfType<Binary>(expression);
        var binary = (Binary)expression;
        Assert.AreEqual(ArithmeticType.Minus, binary.Operator.Type);
            
        // Left child of root should be Number equal to left
        Assert.IsInstanceOfType<Number>(binary.Left);
        Assert.AreEqual(((Number)binary.Left).Value, left);
            
            
        // Right child of root should be Number equal to right
        Assert.IsInstanceOfType<Number>(binary.Right);
        Assert.AreEqual(((Number)binary.Right).Value, right);
    }
    
    [DataTestMethod]
    [DataRow(5, -2, -10)]
    [DataRow(-2, 5, -10)]
    [DataRow(4, 3, 12)]
    public void Parse_Multiplication_CorrectEvaluationAndTree(int left, int right, int expected)
    {
        // Arrange
        var tokens = new Token<ArithmeticType>[]
        {
            new(ArithmeticType.Number, left.ToString()),
            new(ArithmeticType.Multiply, "*"),
            new(ArithmeticType.Number, right.ToString())
        };
            
        var parser = new ArithmeticParser(tokens);
    
        // Act
        var expression = parser.Parse();
        var result = expression.Evaluate();
            
            
        // Assert
        Assert.AreEqual(expected, result);
            
        // Root should be Binary with Multiply
        Assert.IsInstanceOfType<Binary>(expression);
        var binary = (Binary)expression;
        Assert.AreEqual(ArithmeticType.Multiply, binary.Operator.Type);
            
        // Left child of root should be Number equal to left
        Assert.IsInstanceOfType<Number>(binary.Left);
        Assert.AreEqual(((Number)binary.Left).Value, left);
            
            
        // Right child of root should be Number equal to right
        Assert.IsInstanceOfType<Number>(binary.Right);
        Assert.AreEqual(((Number)binary.Right).Value, right);
    }

    [DataTestMethod]
    [DataRow(4, -2, -2)]
    [DataRow(-9, 3, -3)]
    [DataRow(4, 4, 1)]
    public void Parse_Division_CorrectEvaluationAndTree(int left, int right, int expected)
    {
        // Arrange
        var tokens = new Token<ArithmeticType>[]
        {
            new(ArithmeticType.Number, left.ToString()),
            new(ArithmeticType.Divide, "/"),
            new(ArithmeticType.Number, right.ToString())
        };

        var parser = new ArithmeticParser(tokens);

        // Act
        var expression = parser.Parse();
        var result = expression.Evaluate();


        // Assert
        Assert.AreEqual(expected, result);

        // Root should be binary with Divide
        Assert.IsInstanceOfType<Binary>(expression);
        var binary = (Binary)expression;
        Assert.AreEqual(ArithmeticType.Divide, binary.Operator.Type);

        // Left child of root should be Number equal to left
        Assert.IsInstanceOfType<Number>(binary.Left);
        Assert.AreEqual(((Number)binary.Left).Value, left);


        // Right child of root should be Number equal to right
        Assert.IsInstanceOfType<Number>(binary.Right);
        Assert.AreEqual(((Number)binary.Right).Value, right);
    }

    [TestMethod]
    public void Parse_Expression_CorrectEvaluationAndTree()
    {
        // Arrange
        var tokens = new Token<ArithmeticType>[]
        {
            new(ArithmeticType.LeftParenthesis, "("),
            new(ArithmeticType.Number, "3"),
            new(ArithmeticType.Multiply, "*"),
            new(ArithmeticType.Number, "-2"),
            new(ArithmeticType.RightParenthesis, ")"),
            new(ArithmeticType.Divide, "/"),
            new(ArithmeticType.Number, "2"),
        };
    
        var parser = new ArithmeticParser(tokens);
    
        // Act
        var expression = parser.Parse();
        var result = expression.Evaluate();
    
        
        // Assert
        Assert.AreEqual(-3, result);
    
        // Root should be Binary with Divide
        Assert.IsInstanceOfType(expression, typeof(Binary));
        var root = (Binary)expression;
        Assert.AreEqual(ArithmeticType.Divide, root.Operator.Type);
        
        // Right child of root should be Number(2)
        Assert.IsInstanceOfType<Number>(root.Right);
        Assert.AreEqual(2, ((Number)root.Right).Value);
        
        // Left child of root should be Binary with Multiply
        Assert.IsInstanceOfType<Binary>(root.Left);
        var leftNode = (Binary)root.Left;
        Assert.AreEqual(ArithmeticType.Multiply, leftNode.Operator.Type);
        
        // Left child of leftNode should be Number(3)
        Assert.IsInstanceOfType<Number>(leftNode.Left);
        Assert.AreEqual(3, ((Number)leftNode.Left).Value);
        
        // Right child of leftNode should be Number(-2)
        Assert.IsInstanceOfType<Number>(leftNode.Right);
        Assert.AreEqual(-2, ((Number)leftNode.Right).Value);
    }
}