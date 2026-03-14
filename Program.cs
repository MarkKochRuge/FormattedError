using miniproject;

var errorCode = "System Crash";

var error = new FormattedError
{
    UnformattedMessage = $"an error occured: {{{nameof(errorCode)}}}",
    Metadata =
    {
        {nameof(errorCode), errorCode}
    }
};

var user = "USR_01";
var errorMessage = "not authenticated";

var error2 = new FormattedError
{
    UnformattedMessage = $"user: \"{{{nameof(user)}}}\" could not login due to: {{{nameof(errorMessage)}}}",
    Metadata =
    {
        {nameof(user), user},
        {nameof(errorMessage), errorMessage}
    }
};

Console.WriteLine(error.Message);
Console.WriteLine(error2.Message);

var newTest = new Test
{
    id = 1
};

var error3 = new FormattedError
{
    UnformattedMessage = $"User : \"{{{nameof(user)}}}\" could not login",
    Metadata =
    {
        {nameof(user), newTest}
    }
};

Console.WriteLine(error3.Message);

public class Test()
{
   public int? id { get; set; }
}
