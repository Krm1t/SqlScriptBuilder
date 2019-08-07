using System.Text.RegularExpressions;

namespace SqlScriptBuilder
{
  /// <summary>
  /// Defines the name of a table and the rules around it.
  /// </summary>
  public struct TableName
  {
    private string _name;

    /// <summary>
    /// Initializes a new instance of the <see cref="TableName"/> structure with a specified name.
    /// </summary>
    /// <param name="name">Name to initialize with.</param>
    public TableName(string name)
    {
      ValidateName(name);
      _name = name;
    }

    /// <summary>
    /// Name of the table.
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
    /// Validates that a given table name is valid.
    /// </summary>
    /// <param name="name">The name to validate.
    /// Must not be empty.
    /// Contain only letters, numbers and, underscores.</param>
    public static void ValidateName(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
        throw new ScriptBuilderException($"Name cannot be empty or consist entirely of white spaces!");

      if (!Regex.IsMatch(name, @"^[a-zA-Z0-9_]+$"))
        throw new ScriptBuilderException($"Column names can only contain letters, numbers and, underscores!");
    }

    public static implicit operator string(TableName variableName) => variableName.Name;

    public static explicit operator TableName(string name) => new TableName(name);

    /// <summary>
    /// Returns the name of the table.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      return Name;
    }
  }
}
