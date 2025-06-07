using Lexer;

namespace ArithmeticParser;

public interface IExpression
{
    int Evaluate();
}

public class BinaryExpression(Token<TokenType> @operator, IExpression left, IExpression right) : IExpression
{
    public Token<TokenType> Operator { get; } = @operator;
    public IExpression Left { get; } = left;
    public IExpression Right { get; } = right;

    public int Evaluate()
    {
        return Operator.Type switch
        {
            TokenType.Plus => Left.Evaluate() + Right.Evaluate(),
            TokenType.Minus => Left.Evaluate() - Right.Evaluate(),
            TokenType.Multiply => Left.Evaluate() * Right.Evaluate(),
            TokenType.Divide => Left.Evaluate() / Right.Evaluate(),
            _ => throw new InvalidOperationException($"Unsupported binary operator: {Operator.Type}")
        };
    }
}

public class UnaryExpression(Token<TokenType> @operator, IExpression operand) : IExpression
{
    public Token<TokenType> Operator { get; } = @operator;
    public IExpression Operand { get; } = operand;

    public int Evaluate()
    {
        return Operator.Type switch
        {
            TokenType.Plus => Operand.Evaluate(),
            TokenType.Minus => -Operand.Evaluate(),
            _ => throw new InvalidOperationException($"Unsupported unary operator {Operator.Type}")
        };
    }
}

public class LiteralExpression(int value) : IExpression
{
    public int Value { get; } = value;
    
    public int Evaluate() => Value;
}