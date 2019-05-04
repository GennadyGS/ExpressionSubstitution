using System;
using System.Linq.Expressions;

namespace ExpressionSubstitution
{
    internal class ConditionalCalculatedColumn<TArg, TResult> 
        : ICalculatedColumn<Expression<Func<TArg, TResult>>>
    {
        public ConditionalCalculatedColumn(string columnName, Expression<Func<TArg, TResult>> expression, Expression<Func<TArg, bool>> condition)
        {
            ColumnName = columnName;
            Expression = expression;
            Condition = condition;
        }

        public string ColumnName { get; }

        private Expression<Func<TArg, bool>> Condition { get; }

        public Expression<Func<TArg, TResult>> Expression { get; }
    }
}