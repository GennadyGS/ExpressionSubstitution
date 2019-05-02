using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace ReplaceExpression.Console
{
    internal static class CalculatedColumnsExtensions
    {
        public static IReadOnlyDictionary<string, TExpression> ToExpressionMap<TExpression>(
            this IEnumerable<CalculatedColumn<TExpression>> calculatedColumns)
            where TExpression : LambdaExpression =>
                ImmutableDictionary<string, TExpression>.Empty.ModifyColumns(calculatedColumns);
    }
}