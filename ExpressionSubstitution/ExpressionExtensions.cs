using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionSubstitution
{
    internal static class ExpressionExtensions
    {
        public static TExpression SubstituteFields<TExpression>(
            this TExpression expression,
            IReadOnlyDictionary<string, TExpression> fieldExpressionMap)
            where TExpression : LambdaExpression =>
                (TExpression)
                new FieldSubstituteVisitor<TExpression>(
                    fieldExpressionMap, 
                    expression.Parameters.Single(),
                    expression.ReturnType)
                        .Visit(expression);

        private class FieldSubstituteVisitor<TExpression> : ExpressionVisitor
            where TExpression : LambdaExpression
        {
            private readonly IReadOnlyDictionary<string, TExpression> _fieldExpressionMap;
            private readonly ParameterExpression _parameter;
            private readonly Lazy<MethodInfo> _getItemMethodInfo;

            public FieldSubstituteVisitor(
                IReadOnlyDictionary<string, TExpression> fieldExpressionMap,
                ParameterExpression parameter,
                Type returnType)
            {
                _fieldExpressionMap = fieldExpressionMap;
                _parameter = parameter;
                _getItemMethodInfo = new Lazy<MethodInfo>(() => GetItemMethodInfo(returnType));
            }

            protected override Expression VisitMethodCall(MethodCallExpression node) =>
                node.Method == _getItemMethodInfo.Value
                && node.Object != null
                && node.Object.NodeType == ExpressionType.Parameter
                && node.Arguments.Single() is ConstantExpression constant
                && _fieldExpressionMap.TryGetValue((string)constant.Value, out var fieldExpression)
                    ? new SubstituteParameterVisitor(
                            fieldExpression.Parameters.Single(),
                            _parameter)
                        .Visit(fieldExpression.Body)
                        ?? throw new InvalidOperationException("Expression cannot be null")
                    : base.VisitMethodCall(node);

            private MethodInfo GetItemMethodInfo(Type returnType) =>
                typeof(IReadOnlyDictionary<,>)
                    .MakeGenericType(typeof(string), returnType)
                    .GetMethod("get_Item");
        }

        private class SubstituteParameterVisitor : ExpressionVisitor
        {
            private readonly Expression _substitution;
            private readonly ParameterExpression _parameter;

            public SubstituteParameterVisitor(ParameterExpression parameter, Expression substitution)
            {
                _substitution = substitution;
                _parameter = parameter;
            }

            protected override Expression VisitParameter(ParameterExpression node) =>
                node == _parameter 
                    ? _substitution 
                    : node;
        }
    }
}