using System.Collections;
using System.Collections.Generic;

namespace SqlScriptBuilder
{
  /// <summary>
  /// Builder class used to create sql scripts in a fluent way.
  /// </summary>
  public sealed class ScriptBuilder
  {
    private readonly Queue _sectionBuilders;
    /// <summary>
    /// Contains already added variable names.
    /// Variables must be unique across the entire script.
    /// </summary>
    private readonly HashSet<string> _registeredVariableNames;

    /// <summary>
    /// Initializes a new instance of the <see cref="ScriptBuilder"/> class.
    /// </summary>
    public ScriptBuilder()
    {
      _sectionBuilders = new Queue();
      _registeredVariableNames = new HashSet<string>();
    }

    private void EnsureVariableName(string variableName)
    {
      if (_registeredVariableNames.Contains(variableName))
        throw new ScriptBuilderException($"Variable name '{variableName}' have already been defined!");
    }

    /// <summary>
    /// Adds a section builder to the section builder queue.
    /// </summary>
    /// <param name="section">Section builder to add.</param>
    private void AddSectionBuilder(TableVariableBuilder section)
    {
      EnsureVariableName(section.Name);
      _sectionBuilders.Enqueue(section);
    }

    /// <summary>
    /// Creates a table variable with columns and optional initial data.
    /// </summary>
    /// <param name="name">Name of the table variable.
    /// Must be only letters and numbers.</param>
    /// <returns>Returns a <see cref="TableVariableBuilder"/> instance used to setup the variable.</returns>
    public TableVariableBuilder CreateTableVariable(string name)
    {
      var section = new TableVariableBuilder(this, name);
      AddSectionBuilder(section);
      return section;
    }

    /// <summary>
    /// Creates a table variable with columns and optional initial data.
    /// </summary>
    /// <typeparam name="TObjectTemplate">An object type used to define the table variable with.</typeparam>
    /// <param name="name">Name of the table variable. If not defined the name of the type in <typeparamref name="TObjectTemplate"/> is used by default.</param>
    /// <returns>Returns a <see cref="TableVariableBuilder{TObjectTemplate}"/> instance used to setup the variable.</returns>
    public TableVariableBuilder<TObjectTemplate> CreateTableVariable<TObjectTemplate>(string name = "")
      where TObjectTemplate : class
    {
      var tableName = name;
      if (string.IsNullOrWhiteSpace(tableName))
        tableName = typeof(TObjectTemplate).Name;

      var section = new TableVariableBuilder<TObjectTemplate>(this, tableName);
      AddSectionBuilder(section);
      return section;
    }

    /// <summary>
    /// Creates an insert statement with static values or the result of a select statement.
    /// </summary>
    /// <param name="destinationTable">The name of the table to insert data into.</param>
    /// <returns>Returns a <see cref="InsertDataBuilder"/> instance used to setup the statement.</returns>
    public InsertDataBuilder InsertData(string destinationTable)
    {
      var section = new InsertDataBuilder(this, destinationTable);
      _sectionBuilders.Enqueue(section);
      return section;
    }

    /// <summary>
    /// Creates an insert statement with static values or the result of a select statement.
    /// </summary>
    /// <param name="destinationTable">The name of the table to insert data into. If not defined the name of the type in <typeparamref name="TObjectTemplate"/> is used by default.</param>
    /// <returns>Returns a <see cref="InsertDataBuilder{TObjectTemplate}"/> instance used to setup the statement.</returns>
    public InsertDataBuilder<TObjectTemplate> InsertData<TObjectTemplate>(string destinationTable = "")
    {
      var tableName = destinationTable;
      if (string.IsNullOrWhiteSpace(tableName))
        tableName = typeof(TObjectTemplate).Name;

      var section = new InsertDataBuilder<TObjectTemplate>(this, tableName);
      _sectionBuilders.Enqueue(section);
      return section;
    }
  }
}