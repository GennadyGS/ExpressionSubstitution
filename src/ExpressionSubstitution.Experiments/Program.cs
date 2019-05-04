using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionSubstitution.Experiments
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var calculatedColumns = CalculatedColumnFactory.Create<Expression<Func<IReadOnlyDictionary<string, object>, object>>>(
                ("a", r => (int)r["b"] + (int)r["c"])
            );
            var expressionMap = calculatedColumns.ToExpressionMap();
            Console.WriteLine($"Modified expression: {expressionMap["a"]}");
            var record = new Dictionary<string, object>
            {
                ["a"] = 1,
                ["b"] = 2,
                ["c"] = 3,
            };
            Console.WriteLine($"Modified expression result: {expressionMap["a"].Compile()(record)}");
        }
    }
}
