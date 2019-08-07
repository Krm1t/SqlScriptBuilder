using System.Collections.Generic;

namespace SqlScriptBuilder
{
  /// <summary>
  /// Base class for building a table variable script section.
  /// </summary>
  public abstract class TableVariableBuilderBase : VariableSectionBuilder
  {
    private readonly Dictionary<string, TableColumn> _columns;

    /// <summary>
    /// Initializes a new instance of the <see cref="TableVariableBuilder"/> class.
    /// </summary>
    /// <param name="owner">The <see cref="ScriptBuilder"/> that owns this <see cref="TableVariableBuilder"/>.</param>
    /// <param name="name">The name that the table veriable will get in the script.</param>
    internal TableVariableBuilderBase(ScriptBuilder owner, VariableName name)
      : base(owner, name)
    {
      _columns = new Dictionary<string, TableColumn>();
    }

    /// <summary>
    /// The columns that have been added to the section.
    /// </summary>
    protected IReadOnlyDictionary<string, TableColumn> Columns
    {
      get
      {
        return _columns;
      }
    }

    /// <summary>
    /// Adds a predefined column to the table variable.
    /// </summary>
    /// <param name="column">The column to add.</param>
    protected void AddColumnInternal(TableColumn column)
    {
      if (IsFinalized) throw new ScriptBuilderException("Cannot add new columns to the section once it has been finalized!");

      if (_columns.ContainsKey(column.Name))
        throw new ScriptBuilderException($"Column name '{column.Name}' already exist!");

      _columns.Add(column.Name, column);
    }

    /// <summary>
    /// Checks if a particular column name have already been added to the table.
    /// </summary>
    /// <param name="name">Name of the column to check for.</param>
    /// <returns>Returns true if the column already exist in the table; Otherwise, returns false.</returns>
    public bool ColumnExists(string name)
    {
      return _columns.ContainsKey(name);
    }
  }
}
