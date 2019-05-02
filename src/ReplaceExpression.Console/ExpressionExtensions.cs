using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ReplaceExpression.Console
{
    internal static class ExpressionExtensions
    {
        public static TExpression ReplaceFields<TExpression>(
            this TExpression expression,
            IReadOnlyDictionary<string, TExpression> fieldExpressionMap)
            where TExpression : LambdaExpression =>
                (TExpression)
                new FieldReplacerVisitor<TExpression>(
                    fieldExpressionMap, 
                    expression.Parameters.Single(),
                    expression.ReturnType)
                        .Visit(expression);

        private class FieldReplacerVisitor<TExpression> : ExpressionVisitor
            where TExpression : LambdaExpression
        {
            private readonly IReadOnlyDictionary<string, TExpression> _fieldExpressionMap;
            private readonly ParameterExpression _parameter;
            private readonly Lazy<MethodInfo> _getItemMethodInfo;

            public FieldReplacerVisitor(
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
                    ? new ReplaceParameterVisitor(
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

        private class ReplaceParameterVisitor : ExpressionVisitor
        {
            private readonly Expression _replacement;
            private readonly ParameterExpression _parameter;

            public ReplaceParameterVisitor(ParameterExpression parameter, Expression replacement)
            {
                _replacement = replacement;
                _parameter = parameter;
            }

            protected override Expression VisitParameter(ParameterExpression node) =>
                node == _parameter 
                    ? _replacement 
                    : node;
        }
    }
}