using Lexer;

namespace ArithmeticParser;

public class Parser(IEnumerable<Token<TokenType>> tokens)
{
    private readonly List<Token<TokenType>> _tokens = tokens.ToList();
    private int _position = 0;

    private Token<TokenType> Current => _tokens[_position];

    private Token<TokenType> Consume(TokenType type)
    {
        if (Current.Type != type)
            throw new Exception($"Expected {type} but got {Current.Type}");

        return _tokens[_position++];
    }

    public IExpression Parse()
    {
        return ParseExpression();
    }

    private IExpression ParseExpression()
    {
        var left = ParseTerm();

        while (_position < _tokens.Count && Current.Type is TokenType.Plus or TokenType.Minus)
        {
            var operation = Consume(Current.Type);
            var right = ParseTerm();
            
            left = new BinaryExpression(operation, left, right);
        }

        return left;
    }

    private IExpression ParseTerm()
    {
        var left = ParseFactor();

        while (_position < _tokens.Count && Current.Type is TokenType.Multiply or TokenType.Divide)
        {
            var @operator = Consume(Current.Type);
            var right = ParseFactor();

            left = new BinaryExpression(@operator, left, right);
        }

        return left;
    }

    private IExpression ParseFactor()
    {
        if (Current.Type is TokenType.Plus or TokenType.Minus)
        {
            var @operator = Consume(Current.Type);
            var operand = ParsePrimary();
            
            return new UnaryExpression(@operator, operand);
        }

        return ParsePrimary();
    }

    private IExpression ParsePrimary()
    {
        if (Current.Type is TokenType.Number)
            return new LiteralExpression(int.Parse(Consume(TokenType.Number).Value));

        if (Current.Type is TokenType.LeftParenthesis)
        {
            Consume(TokenType.LeftParenthesis);
            var expression = ParseExpression();
            Consume(TokenType.RightParenthesis);

            return expression;
        }
        
        throw new Exception($"Expected a number or parenthesis but got {Current.Type}");
    }
}