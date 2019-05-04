using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionSubstitution.Experiments
{
    internal static class CalculatedColumnFactory
    {
        public static IReadOnlyCollection<CalculatedColumn<TExpression>> Create<TExpression>(
            params (string columnName, TExpression expression)[] items)
            where TExpression : LambdaExpression =>
                items
                    .Select(item => new CalculatedColumn<TExpression>(item.columnName, item.expression))
                    .ToList();
    }
}