using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq.Expressions;
using static System.Console;

namespace ReplaceExpression.Console
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var calculatedColumns = new[]
            {
                new CalculatedColumn<object>("a", r => (int)r["b"] + (int)r["c"]), 
            };
            var columnExpressionMap = ImmutableDictionary<string, LambdaExpression>.Empty;
            var modifiedExpr = columnExpressionMap.ModifyColumns(calculatedColumns);
            WriteLine($"Modified expression: {modifiedExpr["a"]}");
            var record = new Dictionary<string, object>
            {
                ["a"] = 1,
                ["b"] = 2,
                ["c"] = 3,
            };
            WriteLine($"Modified expression result: {((Expression<Func<IReadOnlyDictionary<string, object>, object>>)modifiedExpr["a"]).Compile()(record)}");
        }
    }
}
