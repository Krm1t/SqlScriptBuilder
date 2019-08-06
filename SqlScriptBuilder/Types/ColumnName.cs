using System.Text.RegularExpressions;

namespace SqlScriptBuilder
{
  /// <summary>
  /// Defines the name of a script variable and the rules around it.
  /// </summary>
  public struct ColumnName
  {
    private string _name;

    /// <summary>
    /// Initializes a new instance of the <see cref="ColumnName"/> structure with a specified name.
    /// </summary>
    /// <param name="name">Name to initialize with.</param>
    public ColumnName(string name)
    {
      ValidateName(name);
      _name = name;
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
        ValidateName(value);
        _name = value;
      }
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

      if (!Regex.IsMatch(name, @"^[a-zA-Z0-9_]+$"))
        throw new ScriptBuilderException($"Column names can only contain letters, numbers and, underscores!");
    }

    public static implicit operator string(ColumnName variableName) => variableName.Name;

    public static explicit operator ColumnName(string name) => new ColumnName(name);

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
