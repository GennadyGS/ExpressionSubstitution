using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static System.Console;

namespace ReplaceExpression.Console
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Expression<Func<IDictionary<string, object>, object>> expression = 
                record => (int)record["a"] + 5;
            var fieldExpressionMap = new Dictionary<string, LambdaExpression>
            {
                ["a"] = (Expression<Func<IDictionary<string, object>, object>>)
                    (record => (int)record["b"] + (int)record["c"]),
            };
            var modifiedExpr = expression.ReplaceFields(fieldExpressionMap);
            WriteLine($"Source expression: {expression}");
            WriteLine($"Modified expression: {modifiedExpr}");
            var dict = new Dictionary<string, object>
            {
                ["a"] = 1,
                ["b"] = 2,
                ["c"] = 3,
            };
            WriteLine($"Source expression result: {expression.Compile()(dict)}");
            WriteLine($"Modified expression result: {modifiedExpr.Compile()(dict)}");
        }
    }
}
