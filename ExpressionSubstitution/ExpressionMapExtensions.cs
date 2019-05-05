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
            IEnumerable<ICalculatedColumn<TExpression>> calculatedColumns)
            where TExpression : LambdaExpression =>
                calculatedColumns
                    .Aggregate(
                        sourceExpressionMap,
                        (expressionMap, calculatedColumn) => expressionMap.ModifyColumn(calculatedColumn));

        public static IImmutableDictionary<string, TExpression> ModifyColumn<TExpression>(
            this IImmutableDictionary<string, TExpression> sourceExpressionMap,
            ICalculatedColumn<TExpression> calculatedColumn)
            where TExpression : LambdaExpression =>
                sourceExpressionMap.ModifyColumn(calculatedColumn.ColumnName, calculatedColumn.CalcExpression);

        private static IImmutableDictionary<string, TExpression> ModifyColumn<TExpression>(
            this IImmutableDictionary<string, TExpression> sourceExpressionMap,
            string columnName,
            TExpression columnExpression)
            where TExpression : LambdaExpression =>
                sourceExpressionMap
                    .SetItem(columnName, columnExpression.SubstituteFields(sourceExpressionMap));
    }
}