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
        var left = ParseTerm();

        while (_position < _tokens.Count && Current.Type is ArithmeticType.Plus or ArithmeticType.Minus)
        {
            var operation = Consume(Current.Type);
            var right = ParseTerm();
            
            left = new Binary(left, operation, right);
        }

        return left;
    }

    private Expression ParseTerm()
    {
        var left = ParseFactor();

        while (_position < _tokens.Count && Current.Type is ArithmeticType.Multiply or ArithmeticType.Divide)
        {
            var @operator = Consume(Current.Type);
            var right = ParseFactor();

            left = new Binary(left, @operator, right);
        }

        return left;
    }

    private Expression ParseFactor()
    {
        if (Current.Type is ArithmeticType.Plus or ArithmeticType.Minus)
        {
            var @operator = Consume(Current.Type);
            var right = ParsePrimary();
            
            return new Unary(@operator, right);
        }

        return ParsePrimary();
    }

    private Expression ParsePrimary()
    {
        if (Current.Type is ArithmeticType.Number)
            return new Number(int.Parse(Consume(ArithmeticType.Number).Value));

        if (Current.Type is ArithmeticType.LeftParenthesis)
        {
            Consume(ArithmeticType.LeftParenthesis);
            var expression = ParseExpression();
            Consume(ArithmeticType.RightParenthesis);

            return expression;
        }
        
        throw new Exception($"Expected a number or parenthesis but got {Current.Type}");
    }
}

public enum ArithmeticType
{
    Number,
    Plus,
    Minus,
    Multiply,
    Divide,
    LeftParenthesis,
    RightParenthesis,
    Whitespace,
}

public abstract class Expression
{
    public abstract int Evaluate();
}


public class Binary(Expression left, Token<ArithmeticType> @operator, Expression right): Expression
{
    public Token<ArithmeticType> Operator { get; } = @operator;
    public Expression Left { get; } = left;
    public Expression Right { get; } = right;
    
    public override int Evaluate()
    {
        return Operator.Type switch
        {
            ArithmeticType.Plus => Left.Evaluate() + Right.Evaluate(),
            ArithmeticType.Minus => Left.Evaluate() - Right.Evaluate(),
            ArithmeticType.Multiply => Left.Evaluate() * Right.Evaluate(),
            ArithmeticType.Divide => Left.Evaluate() / Right.Evaluate(),
            _ => throw new ArgumentException("Invalid operator")
        };
    }
}

public class Unary(Token<ArithmeticType> @operator, Expression right) : Expression
{
    public Token<ArithmeticType> Operator { get; } = @operator;
    public Expression Right { get; } = right;

    public override int Evaluate()
    {
        return Operator.Type switch
        {
            ArithmeticType.Plus => Right.Evaluate(),
            ArithmeticType.Minus => -Right.Evaluate(),
            _ => throw new ArgumentException("Invalid operator")
        };
    }
}

public class Number(int value): Expression
{
    public int Value { get; } = value;
    public override int Evaluate() => Value;
}