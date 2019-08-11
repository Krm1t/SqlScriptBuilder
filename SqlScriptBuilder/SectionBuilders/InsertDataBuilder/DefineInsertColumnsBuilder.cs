using System.Data;

namespace SqlScriptBuilder
{
  public class DefineInsertColumnsBuilder : InsertDataBuilder
  {
    internal DefineInsertColumnsBuilder(
      ScriptBuilder owner,
      VariableName destinationTable)
      : base(owner, destinationTable)
    {
    }

    internal DefineInsertColumnsBuilder(
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
    public DefineInsertColumnsBuilder AddColumn(string name, SqlDbType dataType)
    {
      var column = new Column((ColumnName)name, dataType);
      return AddColumn(column);
    }

    /// <summary>
    /// Adds a predefined column that is going to have a value defined in the insert.
    /// </summary>
    /// <param name="column">The column to add.</param>
    /// <returns>Returns the <see cref="InsertDataBuilder"/> that the column was added to.</returns>
    public DefineInsertColumnsBuilder AddColumn(Column column)
    {
      AddColumnInternal(column);
      return this;
    }
  }

  // *******************************************               **************************************************
  // *******************************************Generic version**************************************************
  // *******************************************               **************************************************

  public class DefineInsertColumnsBuilder<TObjectTemplate> : InsertDataBuilder<TObjectTemplate>
  {
    internal DefineInsertColumnsBuilder(
      ScriptBuilder owner,
      VariableName destinationTable)
      : base(owner, destinationTable)
    {
    }

    internal DefineInsertColumnsBuilder(
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
    public DefineInsertColumnsBuilder<TObjectTemplate> AddColumn(string name, SqlDbType dataType)
    {
      var column = new Column((ColumnName)name, dataType);
      return AddColumn(column);
    }

    /// <summary>
    /// Adds a predefined column that is going to have a value defined in the insert.
    /// </summary>
    /// <param name="column">The column to add.</param>
    /// <returns>Returns the <see cref="InsertDataBuilder"/> that the column was added to.</returns>
    public DefineInsertColumnsBuilder<TObjectTemplate> AddColumn(Column column)
    {
      AddColumnInternal(column);
      return this;
    }
  }
}