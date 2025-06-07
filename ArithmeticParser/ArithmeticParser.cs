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
        return ParseNumber();
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