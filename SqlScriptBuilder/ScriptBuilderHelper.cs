/*
 * Internal helper class for the script builder.
 * Try to keep the logic simple as unit tests cannot mock this class.
 * I.e. code only belongs in this class if it is shared across instances
 * and are designed in such a way that callers wouldn't make sense without it.
 * An example of this is simple validation that are requred even in a unit test.
*/
using System.Text.RegularExpressions;

namespace SqlScriptBuilder
{
  /// <summary>
  /// Helper class for the <see cref="ScriptBuilder"/>.
  /// Contains common methods shared across instances.
  /// </summary>
  internal static class ScriptBuilderHelper
  {
    /// <summary>
    /// Validates that a given name (Table, Column etc.) are valid.
    /// </summary>
    /// <param name="name">The name to validate.
    /// Must not be empty and only contain letters and numbers.</param>
    public static void ValidateName(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
        throw new ScriptBuilderException($"Name cannot be empty or consist entirely of whitespaces!");

      if (!Regex.IsMatch(name, @"^[a-zA-Z0-9_]+$"))
        throw new ScriptBuilderException($"Name can only contain letters, numbers and underscores!");
    }
  }
}
