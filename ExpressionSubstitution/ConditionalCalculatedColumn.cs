using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionSubstitution
{
    internal class ConditionalCalculatedColumn<TArg, TResult> 
        : ICalculatedColumn<Expression<Func<TArg, TResult>>>
    {
        private readonly Lazy<MethodInfo> _getItemMethodInfo;

        public ConditionalCalculatedColumn(
            string columnName, 
            Expression<Func<TArg, bool>> condition, 
            Expression<Func<TArg, TResult>> ifTrueExpression)
        {
            ColumnName = columnName;
            IfTrueExpression = ifTrueExpression;
            Condition = condition;
            _getItemMethodInfo = new Lazy<MethodInfo>(() => GetItemMethodInfo(typeof(TResult)));
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
                var ifFalse = Expression.Call(parameter, _getItemMethodInfo.Value, Expression.Constant(ColumnName));
                var lambdaExpression = Expression.Lambda(
                    Expression.Condition(Condition.Body, ifTrue, ifFalse),
                    parameter);
                return (Expression<Func<TArg, TResult>>) lambdaExpression;
            }
        }

        private Expression<Func<TArg, bool>> Condition { get; }

        public Expression<Func<TArg, TResult>> IfTrueExpression { get; }

        private MethodInfo GetItemMethodInfo(Type returnType) =>
            typeof(IReadOnlyDictionary<,>)
                .MakeGenericType(typeof(string), returnType)
                .GetMethod("get_Item");
    }
}