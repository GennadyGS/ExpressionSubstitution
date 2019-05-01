using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ReplaceExpression.Console
{
    internal class FieldReplacerVisitor : ExpressionVisitor
    {
        private readonly IReadOnlyDictionary<string, LambdaExpression> _fieldExpressionMap;
        private static readonly MethodInfo GetItemMethodInfo = typeof(IDictionary<string, object>).GetMethod("get_Item");

        public FieldReplacerVisitor(IReadOnlyDictionary<string, LambdaExpression> fieldExpressionMap) =>
            _fieldExpressionMap = fieldExpressionMap;

        protected override Expression VisitMethodCall(MethodCallExpression methodCall) =>
            methodCall.Method == GetItemMethodInfo
            && methodCall.Object.NodeType == ExpressionType.Parameter
            && methodCall.Arguments.Single() is ConstantExpression constant
            && _fieldExpressionMap.TryGetValue((string)constant.Value, out var fieldExpression)
                ? fieldExpression.Body
                : base.VisitMethodCall(methodCall);
    }
}