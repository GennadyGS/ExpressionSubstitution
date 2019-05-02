using System.Linq.Expressions;

namespace ReplaceExpression.Console
{
    internal class ReplaceParameterVisitor : ExpressionVisitor
    {
        private readonly Expression _replacement;
        private readonly ParameterExpression _parameter;

        public ReplaceParameterVisitor(ParameterExpression parameter, Expression replacement)
        {
            _replacement = replacement;
            _parameter = parameter;
        }

        protected override Expression VisitParameter(ParameterExpression node) =>
            node == _parameter ? _replacement : node;
    }
}