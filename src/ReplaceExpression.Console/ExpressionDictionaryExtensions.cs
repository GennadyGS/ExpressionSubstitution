using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;

namespace ReplaceExpression.Console
{
    internal static class ExpressionDictionaryExtensions
    {
        public static IImmutableDictionary<string, LambdaExpression> ModifyColumns<TResult>(
            this IImmutableDictionary<string, LambdaExpression> sourceExpressionMap,
            IEnumerable<CalculatedColumn<TResult>> calculatedColumns) =>
                calculatedColumns
                    .Aggregate(
                        sourceExpressionMap,
                        (expressionMap, calculatedColumn) => expressionMap.ModifyColumn(calculatedColumn));

        public static IImmutableDictionary<string, LambdaExpression> ModifyColumn<TResult>(
            this IImmutableDictionary<string, LambdaExpression> sourceExpressionMap,
            CalculatedColumn<TResult> calculatedColumn) =>
                sourceExpressionMap.ModifyColumn(calculatedColumn.ColumnName, calculatedColumn.Expression);

        private static IImmutableDictionary<string, LambdaExpression> ModifyColumn<TResult>(
            this IImmutableDictionary<string, LambdaExpression> sourceExpressionMap,
            string columnName,
            Expression<Func<IReadOnlyDictionary<string, TResult>, TResult>> columnExpression) =>
                sourceExpressionMap
                    .SetItem(columnName, columnExpression.ReplaceFields(sourceExpressionMap));
    }
}