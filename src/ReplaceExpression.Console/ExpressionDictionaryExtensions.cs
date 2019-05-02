using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;

namespace ReplaceExpression.Console
{
    internal static class ExpressionDictionaryExtensions
    {
        public static IImmutableDictionary<string, TExpression> ModifyColumns<TResult, TExpression>(
            this IImmutableDictionary<string, TExpression> sourceExpressionMap,
            IEnumerable<CalculatedColumn<TResult>> calculatedColumns)
            where TExpression : LambdaExpression =>
                calculatedColumns
                    .Aggregate(
                        sourceExpressionMap,
                        (expressionMap, calculatedColumn) => expressionMap.ModifyColumn(calculatedColumn));

        public static IImmutableDictionary<string, TExpression> ModifyColumn<TResult, TExpression>(
            this IImmutableDictionary<string, TExpression> sourceExpressionMap,
            CalculatedColumn<TResult> calculatedColumn)
            where TExpression : LambdaExpression =>
                sourceExpressionMap.ModifyColumn<TResult, TExpression>(calculatedColumn.ColumnName, calculatedColumn.Expression);

        private static IImmutableDictionary<string, TExpression> ModifyColumn<TResult, TExpression>(
            this IImmutableDictionary<string, TExpression> sourceExpressionMap,
            string columnName,
            LambdaExpression columnExpression)
            where TExpression : LambdaExpression =>
                sourceExpressionMap
                    .SetItem(columnName, (TExpression)columnExpression.ReplaceFields<TResult>(
                        sourceExpressionMap.ToDictionary(kvp => kvp.Key, kvp => (LambdaExpression)kvp.Value)));
    }
}