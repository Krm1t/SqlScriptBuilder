using System.Data;

namespace SqlScriptBuilder
{
  public class DefineInsertDataBuilder : InsertDataBuilder
  {
    internal DefineInsertDataBuilder(
      ScriptBuilder owner,
      VariableName destinationTable)
      : base(owner, destinationTable)
    {
    }

    internal DefineInsertDataBuilder(
      ScriptBuilder owner,
      TableName destinationTable)
     : base(owner, destinationTable)
    {
    }

    /// <summary>
    /// Adds a predefined column that is going to have a value defined in the insert.
    /// </summary>
    /// <param name="column">The column to add.</param>
    /// <returns>Returns the <see cref="InsertDataBuilder"/> that the column was added to.</returns>
    public DefineInsertDataBuilder AddColumn(string name, SqlDbType dataType)
    {
      var column = new Column((ColumnName)name, dataType);
      return AddColumn(column);
    }

    /// <summary>
    /// Adds a predefined column that is going to have a value defined in the insert.
    /// </summary>
    /// <param name="column">The column to add.</param>
    /// <returns>Returns the <see cref="InsertDataBuilder"/> that the column was added to.</returns>
    public DefineInsertDataBuilder AddColumn(Column column)
    {
      if (IsFinalized)
        throw new ScriptBuilderException("Cannot add new columns to the section once it has been finalized!");
      if (IsDataBuilderCreated())
        throw new ScriptBuilderException("Cannot add new columns to the section once data population has begun!");
      //if (_columns.ContainsKey(column.Name))
      //throw new ScriptBuilderException($"Column name '{column.Name}' already exist!");

      //_columns.Add(column.Name, column);
      return this;
    }
  }
}