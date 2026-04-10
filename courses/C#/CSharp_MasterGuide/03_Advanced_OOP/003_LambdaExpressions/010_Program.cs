/*
 * TOPIC: CSharp_MasterGuide/03_Advanced_OOP/03_LambdaExpressions
 * FILE: Program.cs
 * PURPOSE: Entry point to run all lambda expression examples
 */
using System;

namespace CSharp_MasterGuide._03_Advanced_OOP._03_LambdaExpressions
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Choose a demo to run:");
            Console.WriteLine("1. LambdaBasics");
            Console.WriteLine("2. LambdaBasics_Part2");
            Console.WriteLine("3. LambdaWithGenerics");
            Console.WriteLine("4. FuncAction_Predicate");
            Console.WriteLine("5. FuncAction_Predicate_Part2");
            Console.WriteLine("6. ExpressionTrees");
            Console.WriteLine("7. ExpressionTrees_Part2");
            Console.WriteLine("8. Lambda_RealWorld");
            Console.WriteLine("9. Lambda_RealWorld_Part2");
            Console.WriteLine("A. All Demos");
            
            var choice = Console.ReadLine() ?? "A";
            
            switch (choice.ToUpper())
            {
                case "1": LambdaBasics.RunMain(args); break;
                case "2": LambdaBasicsPart2.RunMain(args); break;
                case "3": LambdaWithGenerics.RunMain(args); break;
                case "4": FuncActionPredicate.RunMain(args); break;
                case "5": FuncActionPredicatePart2.RunMain(args); break;
                case "6": ExpressionTrees.RunMain(args); break;
                case "7": ExpressionTreesPart2.RunMain(args); break;
                case "8": LambdaRealWorld.RunMain(args); break;
                case "9": LambdaRealWorldPart2.RunMain(args); break;
                case "A":
                    Console.WriteLine("\n=== RUNNING ALL DEMOS ===\n");
                    Console.WriteLine("1.\n"); LambdaBasics.RunMain(args);
                    Console.WriteLine("\n2.\n"); LambdaBasicsPart2.RunMain(args);
                    Console.WriteLine("\n3.\n"); LambdaWithGenerics.RunMain(args);
                    Console.WriteLine("\n4.\n"); FuncActionPredicate.RunMain(args);
                    Console.WriteLine("\n5.\n"); FuncActionPredicatePart2.RunMain(args);
                    Console.WriteLine("\n6.\n"); ExpressionTrees.RunMain(args);
                    Console.WriteLine("\n7.\n"); ExpressionTreesPart2.RunMain(args);
                    Console.WriteLine("\n8.\n"); LambdaRealWorld.RunMain(args);
                    Console.WriteLine("\n9.\n"); LambdaRealWorldPart2.RunMain(args);
                    Console.WriteLine("\n=== ALL DEMOS COMPLETE ===");
                    break;
                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
        }
    }
}