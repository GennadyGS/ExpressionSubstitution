using System.Linq.Expressions;

namespace ReplaceExpression.Console
{
    internal class CalculatedColumn<TExpression> where TExpression : LambdaExpression
    {
        public CalculatedColumn(string columnName, TExpression expression)
        {
            ColumnName = columnName;
            Expression = expression;
        }

        public string ColumnName { get; }

        public TExpression Expression { get; }
    }
}