using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionSubstitution
{
    internal static class ExpressionMapExtensions
    {
        public static IImmutableDictionary<string, TExpression> ModifyColumns<TExpression>(
            this IImmutableDictionary<string, TExpression> sourceExpressionMap,
            IEnumerable<CalculatedColumn<TExpression>> calculatedColumns)
            where TExpression : LambdaExpression =>
                calculatedColumns
                    .Aggregate(
                        sourceExpressionMap,
                        (expressionMap, calculatedColumn) => expressionMap.ModifyColumn(calculatedColumn));

        public static IImmutableDictionary<string, TExpression> ModifyColumn<TExpression>(
            this IImmutableDictionary<string, TExpression> sourceExpressionMap,
            CalculatedColumn<TExpression> calculatedColumn)
            where TExpression : LambdaExpression =>
                sourceExpressionMap.ModifyColumn(calculatedColumn.ColumnName, calculatedColumn.Expression);

        private static IImmutableDictionary<string, TExpression> ModifyColumn<TExpression>(
            this IImmutableDictionary<string, TExpression> sourceExpressionMap,
            string columnName,
            TExpression columnExpression)
            where TExpression : LambdaExpression =>
                sourceExpressionMap
                    .SetItem(columnName, columnExpression.ReplaceFields(sourceExpressionMap));
    }
}