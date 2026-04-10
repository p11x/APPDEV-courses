/*
 * TOPIC: CSharp_MasterGuide/03_Advanced_OOP/03_LambdaExpressions
 * SUBTOPIC: Expression Trees - Manipulation and Compilation
 * FILE: ExpressionTrees_Part2.cs
 * PURPOSE: Advanced expression tree manipulation, visiting, transforming, and compiling
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CSharp_MasterGuide._03_Advanced_OOP._03_LambdaExpressions
{
    public class ExpressionTreesPart2
    {
        public static void RunMain(string[] args)
        {
            Console.WriteLine("=== Expression Trees Part 2: Manipulation & Compilation ===\n");

            // ============================================
            // COMPILING EXPRESSION TREES
            // ============================================

            // Example 1: Compile expression to delegate
            Expression<Func<int, int, int>> addExpr = (a, b) => a + b;
            var compiled = addExpr.Compile();
            Console.WriteLine($"Compiled result: {compiled(5, 3)}"); // Output: 8

            // Example 2: Compile and invoke in one step
            var result = addExpr.Compile()(10, 20);
            Console.WriteLine($"Compile and invoke: {result}"); // Output: 30

            // Example 3: Expression<Func<...>> implicitly compiles
            Func<int, int, int> addFunc = addExpr.Compile(); // Explicit conversion
            Func<int, int, int> addFunc2 = addExpr; // Implicit conversion in C# 4.0+
            Console.WriteLine($"Implicit conversion: {addFunc2(7, 8)}"); // Output: 15

            // ============================================
            // EXPRESSION TREE VISITOR PATTERN
            // ============================================

            // Example 4: Custom expression visitor
            Expression<Func<int, int>> expr = x => (x + 3) * 2;
            Console.WriteLine($"\nOriginal expression: {expr}");

            var visitor = new IncrementVisitor();
            var visited = visitor.Visit(expr);
            Console.WriteLine($"After visitor: {visited}");

            // Example 5: Expression that can be optimized
            Expression<Func<int, int>> complexExpr = x => (x + 0) * 1;
            Console.WriteLine($"\nBefore optimization: {complexExpr}");
            var optimized = OptimizeExpression(complexExpr);
            Console.WriteLine($"After optimization: {optimized}");

            // Example 6: Expression substitution
            // Replace parameter with constant
            Expression<Func<int, int>> substitueParam = x => x + 5;
            var substituted = ReplaceParameter(substitueParam, 10);
            Console.WriteLine($"\nSubstituted parameter: {substituted}");

            // ============================================
            // EXPRESSION TREE MANIPULATION
            // ============================================

            // Example 7: Convert expression to different type
            Expression<Func<int, int>> toConvert = x => x * 2;
            var asExpression = (Expression)toConvert;
            Console.WriteLine($"\nAs Expression: {asExpression}");

            // Example 8: Create expression from delegates
            var paramA = Expression.Parameter(typeof(int), "a");
            var paramB = Expression.Parameter(typeof(int), "b");
            var multiplyExpr = Expression.Multiply(paramA, paramB);
            var fromDelegate = Expression.Lambda<Func<int, int, int>>(multiplyExpr, paramA, paramB);
            Console.WriteLine($"From delegate: {fromDelegate}");

            // ============================================
            // REAL-WORLD EXPRESSION MANIPULATION
            // ============================================

            // Example 9: Dynamic predicate builder
            Console.WriteLine("\n=== Dynamic Predicate Builder ===");

            // Build: p => p.Price > 500
            var productParam = Expression.Parameter(typeof(Product), "p");
            var priceProperty = Expression.Property(productParam, "Price");
            var priceConstant = Expression.Constant(500m);
            var priceComparison = Expression.GreaterThan(priceProperty, priceConstant);
            var priceLambda = Expression.Lambda<Func<ETProduct2, bool>>(priceComparison, productParam);

            var expensiveFilter = priceLambda.Compile();
            var products = new List<ETProduct2>
            {
                new ETProduct2 { Id = 1, Name = "Laptop", Price = 999 },
                new ETProduct2 { Id = 2, Name = "Mouse", Price = 29 },
                new ETProduct2 { Id = 3, Name = "Keyboard", Price = 79 }
            };

            var expensive = products.Where(p => expensiveFilter(p)).ToList();
            Console.WriteLine($"Expensive products (>$500): {string.Join(", ", expensive.Select(p => p.Name))}");
            // Output: Laptop

            // Example 10: Dynamic sorting expression
            Console.WriteLine($"\nDynamic sort:");
            var sortedByName = SortBy<ETProduct2>(products, "Name");
            foreach (var p in sortedByName)
            {
                Console.WriteLine($"{p.Name}: ${p.Price}");
            }

            // Example 11: Dynamic property selector
            Console.WriteLine($"\nDynamic selector:");
            var names = Select<ETProduct2, string>(products, "Name");
            foreach (var name in names)
            {
                Console.WriteLine(name);
            }
        }

        // Optimize expression by removing operations like +0, *1
        public static Expression<Func<T, TResult>> OptimizeExpression<T, TResult>(
            Expression<Func<T, TResult>> expr)
        {
            // Simple optimization: just return as-is for demo
            // In real scenario, would walk tree and simplify
            return expr;
        }

        // Replace first parameter with constant
        public static Expression<Func<TResult>> ReplaceParameter<T, TResult>(
            Expression<Func<T, TResult>> expr, T value)
        {
            var constant = Expression.Constant(value, typeof(T));
            var body = expr.Body;
            // Replace parameter with constant using visitor
            var replacer = new ParameterReplacerVisitor(constant);
            var newBody = replacer.Visit(body);
            return Expression.Lambda<Func<TResult>>(newBody);
        }

        // Dynamic sort by property name
        public static List<T> SortBy<T>(List<T> items, string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);
            var orderBy = typeof(Enumerable).GetMethod("OrderBy")
                .MakeGenericMethod(typeof(T), property.PropertyType);
            return orderBy.Invoke(null, new object[] { items, lambda.Compile() }) as List<T>;
        }

        // Dynamic select by property name
        public static List<TResult> Select<T, TResult>(List<T> items, string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);
            var select = typeof(Enumerable).GetMethod("Select")
                .MakeGenericMethod(typeof(T), typeof(TResult));
            return select.Invoke(null, new object[] { items, lambda.Compile() }) as List<TResult>;
        }
    }

    // Custom product for examples
    public class ETProduct2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    // Expression visitor to increment all constants
    public class IncrementVisitor : ExpressionVisitor
    {
        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType == ExpressionType.Add)
            {
                // Could modify + to - for example
            }
            return base.VisitBinary(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.Type == typeof(int))
            {
                int value = (int)node.Value;
                return Expression.Constant(value + 1);
            }
            return node;
        }
    }

    // Visitor to replace parameter with constant
    public class ParameterReplacerVisitor : ExpressionVisitor
    {
        private readonly Expression _replacement;

        public ParameterReplacerVisitor(Expression replacement)
        {
            _replacement = replacement;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return _replacement;
        }
    }
}