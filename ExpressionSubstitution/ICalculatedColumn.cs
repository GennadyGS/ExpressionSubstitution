using System.Linq.Expressions;

namespace ExpressionSubstitution
{
    public interface ICalculatedColumn<out TExpression> 
        where TExpression : LambdaExpression
    {
        string ColumnName { get; }

        TExpression CalcExpression { get; }
    }
}