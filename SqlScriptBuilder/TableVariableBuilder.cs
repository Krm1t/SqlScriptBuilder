using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace SqlScriptBuilder
{
  /// <summary>
  /// Class used to build a table variable script section.
  /// </summary>
  public class TableVariableBuilder : SectionBuilderBase
  {
    private readonly IDictionary<string, TableColumn> _columns;

    /// <summary>
    /// Initializes a new instance of the <see cref="TableVariableBuilder"/> class.
    /// </summary>
    /// <param name="owner">The <see cref="ScriptBuilder"/> that owns this <see cref="TableVariableBuilder"/>.</param>
    /// <param name="name">The name that the table veriable will get in the script.</param>
    internal TableVariableBuilder(ScriptBuilder owner, string name)
      : base(owner)
    {
      ScriptBuilderHelper.ValidateName(name);

      Name = name;
      _columns = new Dictionary<string, TableColumn>();
    }

    /// <summary>
    /// Name of the table variable without @ prefix.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Adds a column to the table variable.
    /// </summary>
    /// <param name="name">Name of the column.</param>
    /// <param name="dataType">Sql data type of the column.</param>
    /// <param name="allowNull">Should the column allow nulls.</param>
    /// <param name="isPrimaryKey">Should the column act as a primary key.</param>
    /// <returns>Returns the <see cref="TableVariableBuilder"/> that the column was added to.</returns>
    public TableVariableBuilder AddColumn(string name, SqlDbType dataType, bool allowNull = false, bool isPrimaryKey = false)
    {
      if (IsFinalized) throw new ScriptBuilderException("Cannot add new columns to the table once it has been finalized!");

      var column = TableColumn.Create(name, dataType, allowNull, isPrimaryKey);
      return AddColumn(column);
    }

    /// <summary>
    /// Adds a predefined column to the table variable.
    /// </summary>
    /// <param name="column">The column to add.</param>
    /// <returns>Returns the <see cref="TableVariableBuilder"/> that the column was added to.</returns>
    public TableVariableBuilder AddColumn(TableColumn column)
    {
      if (IsFinalized) throw new ScriptBuilderException("Cannot add new columns to the table once it has been finalized!");

      if (_columns.ContainsKey(column.Name))
        throw new ScriptBuilderException($"Column name '{column.Name}' already exist!");

      _columns.Add(column.Name, column);
      return this;
    }

    /// <summary>
    /// Finalizes this section of the script and creates an insert data builder with the table name of this instance.
    /// </summary>
    /// <returns>Returns an instance of <see cref="InsertDataBuilder"/> used to insert data into the table variable.</returns>
    public InsertDataBuilder InsertData()
    {
      var owner = EndSection();
      var insertDataSection = owner.InsertData(Name);
      return insertDataSection;
    }
  }

  /// <summary>
  /// Class used to build a table variable script section using a generic type as a template.
  /// </summary>
  /// <typeparam name="TObjectTemplate"></typeparam>
  public class TableVariableBuilder<TObjectTemplate> : TableVariableBuilder
    where TObjectTemplate : class
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="TableVariableBuilder{TSchemaEntity}"/> class.
    /// </summary>
    /// <param name="owner">The <see cref="ScriptBuilder"/> that owns this <see cref="TableVariableBuilder{TSchemaEntity}"/>.</param>
    /// <param name="name">The name that the table veriable will get in the script.</param>
    internal TableVariableBuilder(ScriptBuilder owner, string name)
      : base(owner, name)
    {
    }

    /// <summary>
    /// Adds a column to the table variable using a property on the <typeparamref name="TObjectTemplate"/> type.
    /// </summary>
    /// <typeparam name="TColumnDataType">Type of the property.</typeparam>
    /// <param name="columnSelector">Property in type <typeparamref name="TObjectTemplate"/> to use as a template.</param>
    /// <param name="dataType">The sql data type of the column. If not defined then type will be inferred from <typeparamref name="TColumnDataType"/>.</param>
    /// <param name="allowNull">Should the column allow nulls. If not defined then type will be inferred from <typeparamref name="TColumnDataType"/>.</param>
    /// <param name="isPrimaryKey">Should the column act as a primary key.</param>
    /// <returns>Returns the <see cref="TableVariableBuilder"/> that the column was added to.</returns>
    public TableVariableBuilder AddColumn<TColumnDataType>(Expression<Func<TObjectTemplate, TColumnDataType>> columnSelector, SqlDbType? dataType = null, bool? allowNull = null, bool isPrimaryKey = false)
    {
      var column = TableColumn.Create(columnSelector, dataType, allowNull, isPrimaryKey);
      return AddColumn(column);
    }

    /// <summary>
    /// Finalizes this section of the script and creates an insert data builder with the <typeparamref name="TObjectTemplate"/> as it's template and the table name of this instance.
    /// </summary>
    /// <returns>Returns an instance of <see cref="InsertDataBuilder{TSchemaEntity}"/> used to insert data into the table variable.</returns>
    public new InsertDataBuilder<TObjectTemplate> InsertData()
    {
      var owner = EndSection();
      var insertDataSection = owner.InsertData<TObjectTemplate>(Name);
      return insertDataSection;
    }
  }
}
