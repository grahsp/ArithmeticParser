using Lexer;

namespace ArithmeticParser;

public class ArithmeticParser(IEnumerable<Token<ArithmeticType>> tokens)
{
    private readonly List<Token<ArithmeticType>> _tokens = tokens.ToList();
    private int _position = 0;

    private Token<ArithmeticType> Current => _tokens[_position];

    private Token<ArithmeticType> Consume(ArithmeticType type)
    {
        if (Current.Type != type)
            throw new Exception($"Expected {type} but got {Current.Type}");

        return _tokens[_position++];
    }

    public Expression Parse()
    {
        return ParseExpression();
    }

    private Expression ParseExpression()
    {
        var left = ParseNumber();

        while (_position < _tokens.Count && Current.Type is ArithmeticType.Plus or ArithmeticType.Minus)
        {
            var operation = Consume(Current.Type);
            var right = ParseNumber();
            
            left = new Binary(left, operation, right);
        }

        return left;
    }

    private Expression ParseNumber()
    {
        var token = Consume(ArithmeticType.Number);
        return new Number(int.Parse(token.Value));
    }
}

public enum ArithmeticType
{
    Number,
    Plus,
    Minus,
    Whitespace,
}

public abstract class Expression
{
    public abstract int Evaluate();
}

public class Number(int value): Expression
{
    public int Value { get; } = value;
    public override int Evaluate() => Value;
}

public class Binary(Expression left, Token<ArithmeticType> @operator, Expression right): Expression
{
    private Token<ArithmeticType> Operator { get; } = @operator;
    private Expression Left { get; } = left;
    private Expression Right { get; } = right;
    
    public override int Evaluate()
    {
        return Operator.Type switch
        {
            ArithmeticType.Plus => Left.Evaluate() + Right.Evaluate(),
            ArithmeticType.Minus => Left.Evaluate() - Right.Evaluate(),
            _ => throw new ArgumentException("Invalid operator")
        };
    }
}