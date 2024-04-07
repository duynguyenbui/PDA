namespace System;

public static class StringExtension
{
    public static double EvaluateExpression(this string _, string expression)
    {
        expression = expression.Replace(" ", "");
        var numbers = new Stack<double>();
        var operators = new Stack<char>();
        for (var i = 0; i < expression.Length; i++)
        {
            var ch = expression[i];
            if (char.IsDigit(ch))
            {
                var num = 0;
                while (i < expression.Length && char.IsDigit(expression[i]))
                { num = num * 10 + (expression[i] - '0'); i++; }
                i--; numbers.Push(num);
            }
            else
                switch (ch)
                {
                    case '(':
                        operators.Push(ch); break;
                    case ')':
                    {
                        while (operators.Peek() != '(')
                        {
                            var val2 = numbers.Pop(); var val1 = numbers.Pop(); 
                            var op = operators.Pop(); var result = ApplyOperator(val1, val2, op);
                            numbers.Push(result);
                        } operators.Pop(); break;
                    }
                    default:
                    {
                        if (IsOperator(ch))
                        {
                            while (operators.Count > 0 && Precedence(operators.Peek()) >= Precedence(ch))
                            {
                                var val2 = numbers.Pop(); var val1 = numbers.Pop();
                                var op = operators.Pop(); var result = ApplyOperator(val1, val2, op);
                                numbers.Push(result);
                            } operators.Push(ch);
                        } break;
                    }
                }
        }

        while (operators.Count > 0)
        {
            var val2 = numbers.Pop(); var val1 = numbers.Pop();
            var op = operators.Pop(); var result = ApplyOperator(val1, val2, op);
            numbers.Push(result);
        }
        return numbers.Pop();
    }

    private static bool IsOperator(char ch) => ch is '+' or '-' or '*' or '/';

    private static int Precedence(char op) => op is '+' or '-' ? 1 : op is '*' or '/' ? 2 : 0;

    private static double ApplyOperator(double val1, double val2, char op)
    {
        return op switch
        {
            '+' => val1 + val2,
            '*' => val1 * val2,
            _ => throw new ArgumentException { HelpLink = null, HResult = 0, Source = "https://github.com/duynguyenbui" }
        };
    }
}