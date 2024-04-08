// See https://aka.ms/new-console-template for more information
/*
 * Number can be replaced by int for general, but for programing convenience I prefer using real number for testing
 * Author: Duy Nguyen BUI responsible for coding, developing and testing this algorithm
 */

using System.Globalization;
using System.Text.RegularExpressions;

var numbers = Enumerable.Range(1, 10).Select(i => i.ToString()).ToList();
List<string> alphabet = [..numbers, "+", "*", "(", ")", "N"];
var pda = new PDA(alphabet);
const string input = "(2+(8))*10";
Console.WriteLine(pda.Run(input) ? "Accepted" : "Rejected");
Console.WriteLine(
    $"Result: {input} -> {(input.EvaluateExpression(input) == 0 ? "Invalid expression" : input.EvaluateExpression(input).ToString(CultureInfo.CurrentCulture))}");

/* Class Push Down Automata for the given CFG */
public class PDA
{
    private string state;
    private Stack<string> stack;
    private List<string> alphabet;

    public PDA(List<string> alphabet)
    {
        state = "q0";
        stack = new Stack<string>();
        stack.Push("Z");
        this.alphabet = alphabet;
    }

    private void Transition(string input)
    {
        if (!alphabet.Contains(input)) throw new Exception("Missing alphabet for this input");
        switch (state)
        {
            case "q0" when input == "(":
                stack.Push("(");
                break;
            case "q0" when int.TryParse(input, out _):
                state = "q1";
                break;
            case "q1" when input == ")":
                if (stack.Pop() == "(") state = "q1";
                break;
            case "q1" when input == "N":
                state = "q2";
                break;
            case "q1" when input is "+" or "*":
                state = "q0";
                break;
        }
    }

    public bool Run(string input)
    {
        try
        {
            input += "N";
            const string pattern = @"(\d+|\D)";

            var matches = Regex.Matches(input, pattern);

            foreach (var match in matches.ToList())
            {
                Transition(match.Value);
            }

            return stack.Pop() == "Z" && state == "q2";
        }
        catch (Exception e)
        {
            return false;
        }
    }
}