﻿using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionSubstitution
{
    public static class CalculatedColumnFactory
    {
        public static IReadOnlyCollection<ICalculatedColumn<TExpression>> Create<TExpression>(
            params (string columnName, TExpression expression)[] items)
            where TExpression : LambdaExpression =>
                items
                    .Select(item => new CalculatedColumn<TExpression>(item.columnName, item.expression))
                    .ToList();
    }
}