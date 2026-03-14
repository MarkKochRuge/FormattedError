using System.Text;

namespace miniproject;

public class NotificationHelper<T>
{
  // private readonly NotificationService _notificationService;
  // private readonly ILocalizer<T> _localizer;
  //
  // public NotificationHelper(ILocalizer<T> localizer, NotificationService notificationService)
  // {
  //   _localizer = localizer;
  //   _notificationService = notificationService;
  // }

  public async Task DisplayFormattedErrorAsync(
    FormattedError error,
    string? title = null,
    int duration = 10_000
  )
  {
    var (message, keys) = GetFormattedMessageAndKeys(error.UnformattedMessage);

    var value = keys
      .OrderBy(kw => kw.Value)
      .Select(kw =>
      {
        var insertObject = kw.Value as object;
        if (error.Metadata.TryGetValue(kw.Key, out var obj))
          insertObject = obj;
        return insertObject;
      })
      .ToArray();

    // message = await _localizer.GetAsync(message);
    // message = string.Format(CultureInfo.InvariantCulture, message, values);
  }

  private static (string message, Dictionary<string, int> keys) GetFormattedMessageAndKeys(string formatMessage)
  {
    var keyOrder = 0;

    var keys = new Dictionary<string, int>();
    var messagebuilder = new StringBuilder();

    var messageEnumerator = formatMessage.GetEnumerator();

    while (messageEnumerator.MoveNext())
    {
      var currentChar = messageEnumerator.Current;

      if (currentChar is '{')
      {
        var (validKey, key) = GetKey(messageEnumerator);

        if (validKey) 
        {
          var order = keyOrder;

          if (!keys.TryGetValue(key, out order))
          {
            order = keyOrder;
            keys.Add(key, keyOrder);
            keyOrder++;
          }

          messagebuilder.Append((string)$"{{{order}}}");
        }
        else
          messagebuilder.Append('{' + key);
      }
      else
        messagebuilder.Append(currentChar);
    }

    return (messagebuilder.ToString(), keys);
  }

  private static (bool ValidKey, string Key) GetKey(IEnumerator<char> messageEnumerator)
  {
    var keyBuilder = new StringBuilder();

    while (messageEnumerator.MoveNext())
    {
      var currentChar = messageEnumerator.Current;

      if (currentChar is '}')
        return (true, keyBuilder.ToString());
      else
        keyBuilder.Append(currentChar);
    }

    return (false, keyBuilder.ToString());
  }
}
