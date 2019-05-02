using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using static System.Console;

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
            var modifiedExpr = 
                (Expression<Func<IDictionary<string, object>, object>>)
                    new FieldReplacerVisitor(fieldExpressionMap, expr.Parameters.Single()).Visit(expr);
            WriteLine($"Source expression: {expr}");
            WriteLine($"Modified expression: {modifiedExpr}");
            var dict = new Dictionary<string, object>()
            {
                ["a"] = 1,
                ["b"] = 2,
                ["c"] = 3,
            };
            WriteLine($"Source expression result: {expr.Compile()(dict)}");
            WriteLine($"Modified expression result: {modifiedExpr.Compile()(dict)}");
        }
    }
}
