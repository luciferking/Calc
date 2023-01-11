using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Calc
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter an expression to evaluate. Press q and enter to quit: ");
            string input = Console.ReadLine();

            if (input == "q")
            {
                Environment.Exit(0);
            }

            try
            {
                var token = Tokenize(input);
                var rpn = ToRPN(token);
                var calculate = EvaluateRPN(rpn);
                Console.WriteLine(calculate);
               

            }
            catch (Exception)
            {

                throw;
            }
        }


        static List<string> Tokenize(string expr)
        {
            // Split the expression into tokens
            var tokens = new List<string>();
            var token = "";
            for (int i = 0; i < expr.Length; i++)
            {
                char c = expr[i];
                if (c == ' ')
                {
                    // Add the current token to the list and reset it
                    if (token != "")
                    {
                        tokens.Add(token);
                        token = "";
                    }
                }
                else if (c == '+' || c == '-' || c == '*' || c == '/' || c == '(' || c == ')')
                {
                    // Add the current token to the list and reset it
                    if (token != "")
                    {
                        tokens.Add(token);
                        token = "";
                    }

                    // Add the operator as a separate token
                    tokens.Add(c.ToString());
                }
                else
                {
                    // Append the character to the current token
                    token += c;
                }
            }

            // Add the final token to the list
            if (token != "")
            {
                tokens.Add(token);
            }

            return tokens;
        }

        static List<string> ToRPN(List<string> tokens)
        {
            // Convert the tokens to reverse polish notation 
            var rpn = new List<string>();
            var stack = new Stack<string>();
            foreach (var token in tokens)
            {
                if (double.TryParse(token, out double _))
                {
                    // If the token is a number, add it to the output
                    rpn.Add(token);
                }
                else if (token == "(")
                {
                    // If the token is a left parenthesis, push it onto the stack
                    stack.Push(token);
                }
                else if (token == ")")
                {
                    // If the token is a right parenthesis, pop all operators from the stack until we find the corresponding left parenthesis
                    while (stack.Count > 0 && stack.Peek() != "(")
                    {
                        rpn.Add(stack.Pop());
                    }
                    if (stack.Count > 0)
                    {
                        stack.Pop();
                    }
                }
                else
                {
                    
                    // If the token is an operator, pop all operators from the stack that have higher or equal precedence
                    while (stack.Count > 0 && GetPrecedence(token) <= GetPrecedence(stack.Peek()))
                    {
                        rpn.Add(stack.Pop());
                    }
                    stack.Push(token);
                }
            }

            // Pop all remaining operators from the stack
            while (stack.Count > 0)
            {
                rpn.Add(stack.Pop());
            }

            return rpn;
        }
        static double EvaluateRPN(List<string> rpn)
        {
            // Evaluate the reverse polish notation using a stack
            var stack = new Stack<double>();
            foreach (var token in rpn)
            {
                if (double.TryParse(token, out double num))
                {
                    // If the token is a number, push it onto the stack
                    stack.Push(num);
                }
                else
                {
                    // If the token is an operator, pop the required number of operands from the stack, perform the operation, and push the result back onto the stack
                    double b = stack.Pop();
                    double a = stack.Pop();

                    switch (token)
                    {
                        case "+":
                            stack.Push(a + b);
                            break;
                        case "-":
                            stack.Push(a - b);
                            break;
                        case "*":
                            stack.Push(a * b);
                            break;
                        case "/":
                            stack.Push(a / b);
                            break;
                        default:
                            throw new InvalidOperationException("Invalid operator");
                    }
                    
                }
            }

            // The result should be the only value left on the stack
            return stack.Pop();
        }

        static int GetPrecedence(string op)
        {
            // Return the precedence of an operator (higher values mean higher precedence)
            if (op == "*" || op == "/")
            {
                return 2;
            }
            else if (op == "+" || op == "-")
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        
    }
}
