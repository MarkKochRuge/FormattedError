using System.Text;
using FluentResults;

namespace miniproject;

public class FormattedError : IError
{
  public required string UnformattedMessage { get; set; }
  public Dictionary<string, object> Metadata { get; set; } = [];
  public List<IError>? Reasons { get; set; } = new();
  public string Message => _cachedMessage ??= GetFormattedMessage();

  private string? _cachedMessage;

  private string GetFormattedMessage()
  {
    var messageBuilder = new StringBuilder();
    var messageEnumerator = UnformattedMessage.GetEnumerator();

    while (messageEnumerator.MoveNext())
    {
      var currentChar = messageEnumerator.Current;

      if (currentChar is '{')
      {
        var key = GetKey(messageEnumerator);
        messageBuilder.Append(key);
      }
      else
        messageBuilder.Append(currentChar);
    }

    var message = messageBuilder.ToString();
    return message;
  }

  private string GetKey(IEnumerator<Char> messageEnumerator)
  {
    var keyBuilder = new StringBuilder();

    while (messageEnumerator.MoveNext())
    {
      var currentChar = messageEnumerator.Current;

      if (currentChar is '}')
      {
        var key = keyBuilder.ToString();

        if (Metadata.TryGetValue(key, out var value))
          return value.ToString() ?? string.Empty;
        else
          return '{' + key + '}';
      }
      else
        keyBuilder.Append(currentChar);
    }

    return '{' + keyBuilder.ToString();
  }
}
