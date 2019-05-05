using System.Linq.Expressions;

namespace ExpressionSubstitution
{
    internal class CalculatedColumn<TExpression> : ICalculatedColumn<TExpression> 
        where TExpression : LambdaExpression
    {
        public CalculatedColumn(string columnName, TExpression expression)
        {
            ColumnName = columnName;
            CalcExpression = expression;
        }

        public string ColumnName { get; }

        public TExpression CalcExpression { get; }
    }
}