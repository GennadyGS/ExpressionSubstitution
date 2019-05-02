using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ReplaceExpression.Console
{
    internal static class ExpressionExtensions
    {
        public static LambdaExpression ReplaceFields<TResult>(
            this LambdaExpression expression,
            IReadOnlyDictionary<string, LambdaExpression> fieldExpressionMap) =>
                (LambdaExpression)
                new FieldReplacerVisitor<TResult>(
                    fieldExpressionMap, 
                    expression.Parameters.Single())
                        .Visit(expression);

        private class FieldReplacerVisitor<TResult> : ExpressionVisitor
        {
            private static readonly MethodInfo GetItemMethodInfo = typeof(IReadOnlyDictionary<string, TResult>).GetMethod("get_Item");

            private readonly IReadOnlyDictionary<string, LambdaExpression> _fieldExpressionMap;
            private readonly ParameterExpression _parameter;

            public FieldReplacerVisitor(
                IReadOnlyDictionary<string, LambdaExpression> fieldExpressionMap,
                ParameterExpression parameter)
            {
                _fieldExpressionMap = fieldExpressionMap;
                _parameter = parameter;
            }

            protected override Expression VisitMethodCall(MethodCallExpression node) =>
                node.Method == GetItemMethodInfo
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