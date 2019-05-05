using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionSubstitution.Experiments
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var calculatedColumns = CalculatedColumnFactory.CreateConditionalCalculatedColumns<IReadOnlyDictionary<string, object>, object>(
                ("a", r => (bool) r["b"], r => (int)r["c"] + (int)r["d"])
            );
            var expressionMap = calculatedColumns.ToExpressionMap();
            Console.WriteLine($"Modified expression: {expressionMap["a"]}");
            var record = new Dictionary<string, object>
            {
                ["a"] = 1,
                ["b"] = false,
                ["c"] = 3,
                ["d"] = 4,
            };
            Console.WriteLine($"Modified expression result: {expressionMap["a"].Compile()(record)}");
        }
    }
}
