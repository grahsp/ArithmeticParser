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

    public IExpression Parse()
    {
        return ParseExpression();
    }

    private IExpression ParseExpression()
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

    private IExpression ParseTerm()
    {
        var left = ParseFactor();

        while (!IsAtEnd && CurrentToken.Type is TokenType.Multiply or TokenType.Divide)
        {
            var @operator = ConsumeToken(CurrentToken.Type);
            var right = ParseFactor();

            left = new BinaryExpression(@operator, left, right);
        }

        return left;
    }

    private IExpression ParseFactor()
    {
        if (CurrentToken.Type is TokenType.Plus or TokenType.Minus)
        {
            var @operator = ConsumeToken(CurrentToken.Type);
            var operand = ParsePrimary();
            
            return new UnaryExpression(@operator, operand);
        }

        return ParsePrimary();
    }

    private IExpression ParsePrimary()
    {
        if (CurrentToken.Type is TokenType.Number)
            return new LiteralExpression(double.Parse(ConsumeToken(TokenType.Number).Value));

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