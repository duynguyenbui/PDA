// See https://aka.ms/new-console-template for more information
/*
 * Number can be replaced by int for general, but for programing convenience I prefer using real number for testing
 */

var numbers = Enumerable.Range(1, 10).Select(i => i.ToString()).ToList();
List<string> alphabet = [..numbers, "+", "*", "(", ")"];
var pda = new PDA(alphabet);
const string input = "(2+5)*10";
Console.WriteLine(pda.Run(input) ? "Accepted" : "Rejected");

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
        if (!alphabet.Contains(input)) return;
        switch (state)
        {
            case "q0" when input == "(":
                stack.Push("(");
                break;
            case "q0" when int.TryParse(input, out _):
                state = "q1";
                break;
            case "q1" when input == ")":
                if (stack.Pop() == "(") state = "q2";
                else stack.Push("("); // Push back if ')' is found but no '(' is there
                break;
            case "q1" when input is "+" or "*":
                state = "q0";
                break;
        }
    }

    public bool Run(string input)
    {
        foreach (var symbol in input)
        {
            Transition(symbol.ToString());
        }

        return stack.Pop() == "Z" && state == "q2";
    }
}