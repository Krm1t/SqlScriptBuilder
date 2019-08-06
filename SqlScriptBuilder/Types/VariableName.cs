using System.Text.RegularExpressions;

namespace SqlScriptBuilder
{
  /// <summary>
  /// Defines the name of a script variable and the rules around it.
  /// </summary>
  public struct VariableName
  {
    private string _name;

    /// <summary>
    /// Initializes a new instance of the <see cref="VariableName"/> structure with a specified name.
    /// </summary>
    /// <param name="name">Name to initialize with.</param>
    public VariableName(string name)
    {
      _name = GetFixedName(name);
    }

    /// <summary>
    /// Name of the variable.
    /// </summary>
    public string Name
    {
      get
      {
        return _name;
      }
      set
      {
        _name = GetFixedName(value);
      }
    }

    private static string GetFixedName(string name)
    {
      var fixedName = name.StartsWith("@") ? name : $"@{name}";
      ValidateName(fixedName);
      return fixedName;
    }

    /// <summary>
    /// Validates that a given variable name is valid.
    /// </summary>
    /// <param name="name">The name to validate.
    /// Must not be empty.
    /// Must start with an @ sign
    /// Contain only letters, numbers and, underscores.</param>
    public static void ValidateName(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
        throw new ScriptBuilderException($"Name cannot be empty or consist entirely of white spaces!");

      if (!name.StartsWith("@"))
        throw new ScriptBuilderException($"Variable name must start with an '@' sign!");

      if (name.StartsWith("@@"))
        throw new ScriptBuilderException($"Variable name must not start with two or more '@' signs!");

      if (!Regex.IsMatch(name, @"^@[a-zA-Z0-9_]+$"))
        throw new ScriptBuilderException($"Varable names can only contain letters, numbers and, underscores!");
    }

    public static implicit operator string(VariableName variableName) => variableName.Name;

    public static explicit operator VariableName(string name) => new VariableName(name);

    /// <summary>
    /// Returns the name of the variable.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      return Name;
    }
  }
}
