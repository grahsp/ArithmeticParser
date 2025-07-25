using System.Globalization;
using Lexer;

namespace ArithmeticParser;

public abstract class Expression
{
    public string Name => ToString();
    public double Value => Evaluate();
    public abstract Expression[] Children { get; }
    
    public double Evaluate()
    {
        var rawValue = EvaluateInternal();
        return CleanValue(rawValue);
    }

    protected abstract double EvaluateInternal();
    public abstract override string ToString();

    private static double CleanValue(double value, double epsilon = 1e-10)
    {
        if (Math.Abs(value) < epsilon)
            return 0;

        double rounded = Math.Round(value);
        if (Math.Abs(value - rounded) < epsilon)
            return rounded;

        return value;
    }
}

public class BinaryExpression(Token<TokenType> @operator, Expression left, Expression right) : Expression
{
    public Token<TokenType> Operator { get; } = @operator;
    public Expression Left { get; } = left;
    public Expression Right { get; } = right;
    
    public override Expression[] Children => [Left, Right];

    protected override double EvaluateInternal()
    {
        return Operator.Type switch
        {
            TokenType.Plus => Left.Evaluate() + Right.Evaluate(),
            TokenType.Minus => Left.Evaluate() - Right.Evaluate(),
            TokenType.Star => Left.Evaluate() * Right.Evaluate(),
            TokenType.Slash => Left.Evaluate() / Right.Evaluate(),
            _ => throw new InvalidOperationException($"Unsupported binary operator: {Operator.Type}")
        };
    }

    public override string ToString() => Operator.Value;
}

public class UnaryExpression(Token<TokenType> @operator, Expression operand) : Expression
{
    public Token<TokenType> Operator { get; } = @operator;
    public Expression Operand { get; } = operand;
    
    public override Expression[] Children => [Operand];

    protected override double EvaluateInternal()
    {
        return Operator.Type switch
        {
            TokenType.Plus => Operand.Evaluate(),
            TokenType.Minus => -Operand.Evaluate(),
            _ => throw new InvalidOperationException($"Unsupported unary operator {Operator.Type}")
        };
    }
    
    public override string ToString() => Operator.Value;
}

public class FunctionExpression(Token<TokenType> function, Expression operand) : Expression
{
    public Token<TokenType> Function { get; } = function;
    public Expression Operand { get; } = operand;
    
    public override Expression[] Children => [Operand];

    protected override double EvaluateInternal()
    {
        var degrees = Operand.Evaluate();
        var radians = degrees * Math.PI / 180;
        
        return Function.Type switch
        {
            TokenType.Sin => Math.Sin(radians),
            TokenType.Cos => Math.Cos(radians),
            TokenType.Tan => Math.Tan(radians),
            _ => throw new InvalidOperationException($"Unsupported function: {Function.Type}")
        };
    }
    
    public override string ToString() => Function.Value;
}

public class LiteralExpression(double literal) : Expression
{
    public double Literal { get; } = literal;
    
    public override Expression[] Children => [];
    
    protected override double EvaluateInternal() => Literal;
    
    public override string ToString() => Literal.ToString(CultureInfo.InvariantCulture);
}