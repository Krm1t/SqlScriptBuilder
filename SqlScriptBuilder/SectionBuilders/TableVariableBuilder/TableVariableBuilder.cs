using System;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace SqlScriptBuilder
{
  /// <summary>
  /// Class used to build a table variable script section.
  /// </summary>
  public class TableVariableBuilder : TableVariableBuilderBase
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="TableVariableBuilder"/> class.
    /// </summary>
    /// <param name="owner">The <see cref="ScriptBuilder"/> that owns this <see cref="TableVariableBuilder"/>.</param>
    /// <param name="name">The name that the table veriable will get in the script.</param>
    internal TableVariableBuilder(ScriptBuilder owner, VariableName name)
      : base(owner, name)
    {
    }

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
      var column = TableColumn.Create((ColumnName)name, dataType, allowNull, isPrimaryKey);
      return AddColumn(column);
    }

    /// <summary>
    /// Adds a predefined column to the table variable.
    /// </summary>
    /// <param name="column">The column to add.</param>
    /// <returns>Returns the <see cref="TableVariableBuilder"/> that the column was added to.</returns>
    public TableVariableBuilder AddColumn(TableColumn column)
    {
      AddColumnInternal(column);
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

  // *******************************************               **************************************************
  // *******************************************Generic version**************************************************
  // *******************************************               **************************************************

  /// <summary>
  /// Class used to build a table variable script section using a generic type as a template.
  /// </summary>
  /// <typeparam name="TObjectTemplate"></typeparam>
  public class TableVariableBuilder<TObjectTemplate> : TableVariableBuilderBase
    where TObjectTemplate : class
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="TableVariableBuilder{TObjectTemplate}"/> class.
    /// </summary>
    /// <param name="owner">The <see cref="ScriptBuilder"/> that owns this <see cref="TableVariableBuilder{TObjectTemplate}"/>.</param>
    /// <param name="name">The name that the table veriable will get in the script.</param>
    internal TableVariableBuilder(ScriptBuilder owner, VariableName name)
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
    /// <returns>Returns the <see cref="TableVariableBuilder{TObjectTemplate}"/> that the column was added to.</returns>
    public TableVariableBuilder<TObjectTemplate> AddColumn<TColumnDataType>(
      Expression<Func<TObjectTemplate, TColumnDataType>> columnSelector,
      SqlDbType? dataType = null,
      bool? allowNull = null,
      bool isPrimaryKey = false)
    {
      var column = TableColumn.Create(columnSelector, dataType, allowNull, isPrimaryKey);
      AddColumnInternal(column);
      return this;
    }

    /// <summary>
    /// Reflects the public properties on <typeparamref name="TObjectTemplate"/> and creates columns for each property.
    /// If a column already exist with a particular name then it is skipped.
    /// </summary>
    /// <param name="columnFactory">A factory used to create a <see cref="TableColumn"/> instance based on a <see cref="PropertyInfo"/> instance.</param>
    /// <returns>Returns the <see cref="TableVariableBuilder{TObjectTemplate}"/> that the column was added to.</returns>
    public TableVariableBuilder<TObjectTemplate> GenerateColumns(
      Func<PropertyInfo, TableColumn> columnFactory = null)
    {
      var tableColumnFactory = columnFactory ?? new Func<PropertyInfo, TableColumn>(p =>
        TableColumn.Create((ColumnName)p.Name, ClrTypeToSqlDbTypeMapper.GetSqlDbTypeFromClrType(p.PropertyType), p.PropertyType.IsTypeNullable()));
      var properties = typeof(TObjectTemplate).GetProperties();
      foreach (var property in properties)
      {
        var column = tableColumnFactory(property);
        if (!ColumnExists(column.Name))
          AddColumnInternal(column);
      }

      return this;
    }

    /// <summary>
    /// Finalizes this section of the script and creates an <see cref="InsertDataBuilder{TObjectTemplate}"/> with the <typeparamref name="TObjectTemplate"/> as it's template and the table name of this instance.
    /// </summary>
    /// <returns>Returns an instance of <see cref="InsertDataBuilder{TObjectTemplate}"/> used to insert data into the table variable.</returns>
    public InsertDataBuilder<TObjectTemplate> InsertData()
    {
      var owner = EndSection();
      var insertDataSection = owner.InsertData<TObjectTemplate>(Name);

      foreach (var column in Columns)
        insertDataSection.AddColumn(column.Value);

      return insertDataSection;
    }
  }
}
