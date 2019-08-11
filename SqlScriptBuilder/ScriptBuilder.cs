using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlScriptBuilder
{
  /// <summary>
  /// Builder class used to create sql scripts in a fluent way.
  /// </summary>
  public sealed class ScriptBuilder : IOwner
  {
    private readonly IScriptGenerator _scriptGenerator;
    private readonly Queue<SectionBuilder> _sections;

    /// <summary>
    /// Contains already added variable names.
    /// Variables must be unique across the entire script.
    /// </summary>
    private readonly HashSet<VariableName> _registeredVariableNames;

    /// <summary>
    /// Initializes a new instance of the <see cref="ScriptBuilder"/> class.
    /// </summary>
    public ScriptBuilder()
    {
      _scriptGenerator = new MSSqlScriptGenerator();
      _sections = new Queue<SectionBuilder>();
      _registeredVariableNames = new HashSet<VariableName>();
    }

    /// <summary>
    /// Ensures that a variable with the specified name have not already been added to the script.
    /// </summary>
    /// <param name="variableName">The name to check.</param>
    private void EnsureVariableName(VariableName variableName)
    {
      if (_registeredVariableNames.Contains(variableName))
        throw new ScriptBuilderException($"Variable name '{variableName}' have already been defined!");
    }

    /// <summary>
    /// Adds a section to the section queue.
    /// </summary>
    /// <param name="section">Section to add.</param>
    void IOwner.AddSection(SectionBuilder section)
    {
      if (section is VariableSectionBuilder variableSectionBuilder)
        EnsureVariableName(variableSectionBuilder.Name);

      _sections.Enqueue(section);
    }

    /// <summary>
    /// Creates a table variable with columns and optional initial data.
    /// Does not actually add the section to the builder. Call <see cref="SectionBuilder.EndSection"/> to add it to the <see cref="ScriptBuilder"/>.
    /// </summary>
    /// <param name="name">Name of the table variable.
    /// Must be only letters and numbers.</param>
    /// <returns>Returns a <see cref="TableVariableBuilder"/> instance used to setup the variable.</returns>
    public TableVariableBuilder DeclareTableVariable(string name)
    {
      var section = new TableVariableBuilder(this, (VariableName)name);
      return section;
    }

    /// <summary>
    /// Creates a table variable with columns and optional initial data.
    /// Does not actually add the section to the builder. Call <see cref="SectionBuilder.EndSection"/> to add it to the <see cref="ScriptBuilder"/>.
    /// </summary>
    /// <typeparam name="TObjectTemplate">An object type used to define the table variable with.</typeparam>
    /// <param name="name">Name of the table variable. If not defined the name of the type in <typeparamref name="TObjectTemplate"/> is used by default.</param>
    /// <returns>Returns a <see cref="TableVariableBuilder{TObjectTemplate}"/> instance used to setup the variable.</returns>
    public TableVariableBuilder<TObjectTemplate> DeclareTableVariable<TObjectTemplate>(string name = "")
      where TObjectTemplate : class
    {
      var tableName = name;
      if (string.IsNullOrWhiteSpace(tableName))
        tableName = typeof(TObjectTemplate).Name;

      var section = new TableVariableBuilder<TObjectTemplate>(this, (VariableName)tableName);
      return section;
    }

    /// <summary>
    /// Creates one or more insert statements against a table or table variable.
    /// Does not actually add the section to the builder. Call <see cref="SectionBuilder.EndSection"/> to add it to the <see cref="ScriptBuilder"/>.
    /// </summary>
    /// <param name="destinationTable">The name of the table or table variable to insert data into.</param>
    /// <returns>Returns a <see cref="InsertDataBuilder"/> instance used to setup the statement.</returns>
    public DefineInsertColumnsBuilder InsertData(string destinationTable)
    {
      DefineInsertColumnsBuilder section;
      if (destinationTable.StartsWith("@"))
        section = new DefineInsertColumnsBuilder(this, (VariableName)destinationTable);
      else
        section = new DefineInsertColumnsBuilder(this, (TableName)destinationTable);

      return section;
    }

    /// <summary>
    /// Creates an insert statement with static values or the result of a select statement.
    /// Does not actually add the section to the builder. Call <see cref="SectionBuilder.EndSection"/> to add it to the <see cref="ScriptBuilder"/>.
    /// </summary>
    /// <param name="destinationTable">The name of the table to insert data into. If not defined the name of the type in <typeparamref name="TObjectTemplate"/> is used by default.</param>
    /// <returns>Returns a <see cref="InsertDataBuilder{TObjectTemplate}"/> instance used to setup the statement.</returns>
    public DefineInsertColumnsBuilder<TObjectTemplate> InsertData<TObjectTemplate>(string destinationTable = "")
    {
      var tableName = destinationTable;
      if (string.IsNullOrWhiteSpace(tableName))
        tableName = typeof(TObjectTemplate).Name;

      DefineInsertColumnsBuilder<TObjectTemplate> section;
      if (tableName.StartsWith("@"))
        section = new DefineInsertColumnsBuilder<TObjectTemplate>(this, (VariableName)tableName);
      else
        section = new DefineInsertColumnsBuilder<TObjectTemplate>(this, (TableName)tableName);

      return section;
    }

    public override string ToString()
    {
      var tempQueue = new Queue<SectionBuilder>(_sections);
      var stringBuilder = new StringBuilder();
      while (tempQueue.Any())
      {
        var nextSection = tempQueue.Dequeue();
        stringBuilder.AppendLine(_scriptGenerator.GenerateSection(nextSection));
      }

      return stringBuilder.ToString();
    }
  }
}