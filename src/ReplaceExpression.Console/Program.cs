using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ReplaceExpression.Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            Expression<Func<IDictionary<string, object>, object>> expr = record => (int)record["a"] + 5;
            var fieldExpressionMap = new Dictionary<string, LambdaExpression>
            {
                ["a"] = (Expression<Func<IDictionary<string, object>, object>>)(record => (int)record["b"] + (int)record["c"]),
            };
            var modifiedExpr = new FieldReplacerVisitor(fieldExpressionMap).Visit(expr);
            System.Console.WriteLine(expr.ToString());
            System.Console.WriteLine(modifiedExpr.ToString());
        }
    }
}
