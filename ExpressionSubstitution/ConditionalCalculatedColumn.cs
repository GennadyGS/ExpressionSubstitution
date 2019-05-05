using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionSubstitution
{
    internal class ConditionalCalculatedColumn<TArg, TResult> 
        : ICalculatedColumn<Expression<Func<TArg, TResult>>>
    {
        public ConditionalCalculatedColumn(string columnName, Expression<Func<TArg, TResult>> ifTrueExpression, Expression<Func<TArg, bool>> condition)
        {
            ColumnName = columnName;
            IfTrueExpression = ifTrueExpression;
            Condition = condition;
        }

        public string ColumnName { get; }

        public Expression<Func<TArg, TResult>> CalcExpression
        {
            get
            {
                var parameter = Condition.Parameters.Single();
                var ifTrue = IfTrueExpression.Body.SubstituteParameter(
                    IfTrueExpression.Parameters.Single(),
                    parameter);
                // TODO: Get method info
                MethodInfo methodInfo = null;
                var ifFalse = Expression.Call(parameter, methodInfo, Expression.Constant(ColumnName));
                return (Expression<Func<TArg, TResult>>) 
                    Expression.Lambda(Expression.IfThenElse(Condition.Body, ifTrue, ifFalse));
            }
        }

        private Expression<Func<TArg, bool>> Condition { get; }

        public Expression<Func<TArg, TResult>> IfTrueExpression { get; }
    }
}