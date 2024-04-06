// See https://aka.ms/new-console-template for more information
/*
 * Number can be replaced by int for general, but for programing convenience I prefer using real number for testing
 */
List<string> alphabet = ["2", "5", "10", "+", "*", "(", ")"];
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
            case "q0":
                switch (input)
                {
                    case "(":
                        stack.Push("(");
                        state = "q0";
                        break;
                    case "2":
                    case "5":
                    case "10":
                        // Do nothing but change state to "q1"
                        state = "q1";
                        break;
                }
                break;
            case "q1":
                switch (input)
                {
                    case ")":
                        {
                            var result = stack.Pop();
                            if (result != "(")
                            {
                                state = "q1"; // Go back to q1 if ')' is found but no '(' is there
                            }
                            else
                            {
                                state = "q2";
                            }

                            break;
                        }
                    case "+" or "*":
                        state = "q0";
                        break;
                }
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