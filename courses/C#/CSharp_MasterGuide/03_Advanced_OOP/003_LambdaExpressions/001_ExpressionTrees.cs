/*
 * TOPIC: CSharp_MasterGuide/03_Advanced_OOP/03_LambdaExpressions
 * SUBTOPIC: Expression Trees
 * FILE: ExpressionTrees.cs
 * PURPOSE: Introduction to Expression<T> for building dynamic queries and lambda inspection
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CSharp_MasterGuide._03_Advanced_OOP._03_LambdaExpressions
{
    public class ETProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
    }

    public class ExpressionTrees
    {
        public static void RunMain(string[] args)
        {
            Console.WriteLine("=== Expression Trees ===\n");

            // ============================================
            // LAMBDA TO EXPRESSION CONVERSION
            // ============================================

            // Example 1: Converting lambda to Expression tree
            // Use Expression<T> instead of Func<T> to get inspectable tree structure
            Expression<Func<int, int>> squareExpr = x => x * x;
            Console.WriteLine($"Expression type: {squareExpr.Type}"); // Output: Func<Int32, Int32>
            Console.WriteLine($"Expression body: {squareExpr.Body}"); // Output: x * x

            // Example 2: Inspecting expression tree structure
            PrintExpressionTree(squareExpr, "Square");
            // Output shows:
            // Lambda: x => x * x
            // Body: x * x (BinaryExpression)
            // Parameters: x

            // Example 3: Expression with multiple parameters
            Expression<Func<int, int, int>> addExpr = (a, b) => a + b;
            Console.WriteLine($"\nAdd expression: {addExpr}"); // Output: (a, b) => a + b
            PrintExpressionTree(addExpr, "Add");

            // Example 4: Complex expression with method calls
            Expression<Func<string, int>> lengthExpr = s => s.Length;
            Console.WriteLine($"\nLength expression: {lengthExpr}");
            // This is a MethodCallExpression

            // ============================================
            // BUILDING EXPRESSION TREES programmatically
            // ============================================

            // Example 5: Build expression tree programmatically
            // x => x > 5
            var parameter = Expression.Parameter(typeof(int), "x");
            var constant = Expression.Constant(5, typeof(int));
            var comparison = Expression.GreaterThan(parameter, constant);
            var lambda = Expression.Lambda<Func<int, bool>>(comparison, parameter);
            Console.WriteLine($"\nProgrammatic expression: {lambda}");
            // Output: x => (x > 5)

            // Example 6: Build more complex expression
            // x => x * 2 + 10
            var param = Expression.Parameter(typeof(int), "x");
            var multiply = Expression.Multiply(param, Expression.Constant(2));
            var add = Expression.Add(multiply, Expression.Constant(10));
            var complexLambda = Expression.Lambda<Func<int, int>>(add, param);
            Console.WriteLine($"Complex expression: {complexLambda}");
            // Output: x => ((x * 2) + 10)

            // Example 7: Build with multiple parameters
            // (x, y) => x + y * 2
            var paramX = Expression.Parameter(typeof(int), "x");
            var paramY = Expression.Parameter(typeof(int), "y");
            var mult = Expression.Multiply(paramY, Expression.Constant(2));
            var sum = Expression.Add(paramX, mult);
            var twoParamLambda = Expression.Lambda<Func<int, int, int>>(sum, paramX, paramY);
            Console.WriteLine($"Two parameter expression: {twoParamLambda}");
            // Output: (x, y) => (x + (y * 2))

            // ============================================
            // EXPRESSION TREES WITH LINQ
            // ============================================

            // Example 8: Using expressions in LINQ queries
            var products = new List<ETProduct>
            {
                new ETProduct { Id = 1, Name = "Laptop", Price = 999, Category = "Electronics" },
                new ETProduct { Id = 2, Name = "Book", Price = 29, Category = "Books" },
                new ETProduct { Id = 3, Name = "Phone", Price = 699, Category = "Electronics" },
                new ETProduct { Id = 4, Name = "Tablet", Price = 499, Category = "Electronics" }
            };

            // Define expression for filtering
            Expression<Func<ETProduct, bool>> expensiveFilter = p => p.Price > 100;
            Console.WriteLine($"\nExpensive filter: {expensiveFilter}");

            // Compile and use
            var compiled = expensiveFilter.Compile();
            var expensive = products.Where(p => compiled(p)).ToList();
            Console.WriteLine($"Expensive products: {string.Join(", ", expensive.Select(p => p.Name))}");
            // Output: Laptop, Phone, Tablet

            // Example 9: Dynamic query building
            var query = BuildQuery<Product>(products.AsQueryable(), p => p.Category == "Electronics");
            Console.WriteLine($"\nDynamic query results: {string.Join(", ", query.Select(p => p.Name))}");
            // Output: Laptop, Phone, Tablet

            // Example 10: Combining expressions
            Expression<Func<ETProduct, bool>> categoryFilter = p => p.Category == "Electronics";
            Expression<Func<ETProduct, bool>> priceFilter = p => p.Price < 800;
            var combined = CombineExpressions(categoryFilter, priceFilter);
            var affordable = products.Where(p => combined.Compile()(p)).ToList();
            Console.WriteLine($"Combined filter: {string.Join(", ", affordable.Select(p => p.Name))}");
            // Output: Phone, Tablet
        }

        // Print expression tree structure
        public static void PrintExpressionTree<T>(Expression<T> expr, string name)
        {
            Console.WriteLine($"\n{name} Expression Tree:");
            Console.WriteLine($"  Type: {expr.Type}");
            Console.WriteLine($"  Body: {expr.Body}");
            Console.WriteLine($"  Parameters: {string.Join(", ", expr.Parameters.Select(p => p.Name))}");
        }

        // Build dynamic query from expression
        public static IQueryable<T> BuildQuery<T>(IQueryable<T> source, Expression<Func<T, bool>> filter)
        {
            return source.Where(filter);
        }

        // Combine two expressions with AND
        public static Expression<Func<T, bool>> CombineExpressions<T>(
            Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            // Parameter expression (same parameter for both)
            var parameter = Expression.Parameter(typeof(T), "p");

            // Invoke both expressions with the parameter
            var body1 = Expression.Invoke(expr1, parameter);
            var body2 = Expression.Invoke(expr2, parameter);

            // AND together
            var combined = Expression.AndAlso(body1, body2);

            // Create lambda
            return Expression.Lambda<Func<T, bool>>(combined, parameter);
        }
    }
}