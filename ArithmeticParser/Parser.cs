using System.Globalization;
using Lexer;

namespace ArithmeticParser;

public class Parser(IEnumerable<Token<TokenType>> tokens)
{
    private readonly List<Token<TokenType>> _tokens = tokens.ToList();
    private int _position = 0;

    private Token<TokenType> CurrentToken => _tokens[_position];
    private bool IsAtEnd => _position >= _tokens.Count;

    private Token<TokenType> ConsumeToken(TokenType type)
    {
        if (CurrentToken.Type != type)
            throw new Exception($"Expected {type} but got {CurrentToken.Type}");

        return _tokens[_position++];
    }

    public Expression Parse()
    {
        return ParseExpression();
    }

    private Expression ParseExpression()
    {
        var left = ParseTerm();

        while (!IsAtEnd && CurrentToken.Type is TokenType.Plus or TokenType.Minus)
        {
            var operation = ConsumeToken(CurrentToken.Type);
            var right = ParseTerm();
            
            left = new BinaryExpression(operation, left, right);
        }

        return left;
    }

    private Expression ParseTerm()
    {
        var left = ParseFactor();

        while (!IsAtEnd && CurrentToken.Type is TokenType.Star or TokenType.Slash)
        {
            var @operator = ConsumeToken(CurrentToken.Type);
            var right = ParseFactor();

            left = new BinaryExpression(@operator, left, right);
        }

        return left;
    }

    private Expression ParseFactor()
    {
        if (CurrentToken.Type is TokenType.Plus or TokenType.Minus)
        {
            var @operator = ConsumeToken(CurrentToken.Type);
            var operand = ParsePrimary();
            
            return new UnaryExpression(@operator, operand);
        }

        return ParseFunction();
    }

    private Expression ParseFunction()
    {
        if (CurrentToken.Type is TokenType.Sin or TokenType.Cos or TokenType.Tan)
        {
            var function = ConsumeToken(CurrentToken.Type);
            ConsumeToken(TokenType.LeftParenthesis);
            var expression = ParseExpression();
            ConsumeToken(TokenType.RightParenthesis);

            return new FunctionExpression(function, expression);
        }

        return ParsePrimary();
    }

    private Expression ParsePrimary()
    {
        if (CurrentToken.Type is TokenType.Number)
            return new LiteralExpression(double.Parse(ConsumeToken(TokenType.Number).Value, CultureInfo.InvariantCulture));

        if (CurrentToken.Type is TokenType.Pi)
            return new LiteralExpression(Math.PI);
                
        if (CurrentToken.Type is TokenType.LeftParenthesis)
        {
            ConsumeToken(TokenType.LeftParenthesis);
            var expression = ParseExpression();
            ConsumeToken(TokenType.RightParenthesis);

            return expression;
        }
        
        throw new Exception($"Unexpected token '{CurrentToken.Value}' of type {CurrentToken.Type}");
    }
}