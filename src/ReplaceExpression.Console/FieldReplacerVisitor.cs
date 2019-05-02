using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ReplaceExpression.Console
{
    internal class FieldReplacerVisitor : ExpressionVisitor
    {
        private readonly IReadOnlyDictionary<string, LambdaExpression> fieldExpressionMap;
        private readonly MethodInfo getItemMethodInfo = typeof(IDictionary<string, object>).GetMethod("get_Item");
        private readonly ParameterExpression parameter;

        public FieldReplacerVisitor(
            IReadOnlyDictionary<string, LambdaExpression> fieldExpressionMap,
            ParameterExpression parameter)
        {
            this.fieldExpressionMap = fieldExpressionMap;
            this.parameter = parameter;
        }

        protected override Expression VisitMethodCall(MethodCallExpression methodCall) =>
            methodCall.Method == getItemMethodInfo
            && methodCall.Object.NodeType == ExpressionType.Parameter
            && methodCall.Arguments.Single() is ConstantExpression constant
            && fieldExpressionMap.TryGetValue((string)constant.Value, out var fieldExpression)
                ? new ReplaceParameterVisitor(
                        fieldExpression.Parameters.Single(),
                        parameter)
                    .Visit(fieldExpression.Body)
                : base.VisitMethodCall(methodCall);
    }
}