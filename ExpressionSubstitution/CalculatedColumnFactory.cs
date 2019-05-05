using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionSubstitution
{
    public static class CalculatedColumnFactory
    {
        public static IReadOnlyCollection<ICalculatedColumn<TExpression>> CreateCalculatedColumns<TExpression>(
            params (string columnName, TExpression calcExpression)[] items)
            where TExpression : LambdaExpression =>
                items
                    .Select(item => new CalculatedColumn<TExpression>(item.columnName, item.calcExpression))
                    .ToList();

        public static IReadOnlyCollection<ICalculatedColumn<Expression<Func<TArg, TResult>>>> CreateConditionalCalculatedColumns
            <TArg, TResult>(
            params (
                string columnName, 
                Expression<Func<TArg, bool>> condition,
                Expression<Func<TArg, TResult>> ifTruexpression)[] items) =>
                    items
                        .Select(item => new ConditionalCalculatedColumn<TArg, TResult>(item.columnName, item.condition, item.ifTruexpression))
                        .ToList();
    }
}