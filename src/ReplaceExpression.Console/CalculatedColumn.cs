using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ReplaceExpression.Console
{
    internal class CalculatedColumn<TResult>
    {
        public CalculatedColumn(string columnName, Expression<Func<IReadOnlyDictionary<string, TResult>, TResult>> expression)
        {
            ColumnName = columnName;
            Expression = expression;
        }

        public string ColumnName { get; }

        public Expression<Func<IReadOnlyDictionary<string, TResult>, TResult>> Expression { get; }
    }
}