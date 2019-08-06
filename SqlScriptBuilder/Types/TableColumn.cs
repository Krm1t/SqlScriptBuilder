using System;
using System.Data;
using System.Linq.Expressions;

namespace SqlScriptBuilder
{

  /// <summary>
  /// Represents a column in a table variable.
  /// </summary>
  public sealed class TableColumn : Column
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="TableColumn"/> class.
    /// </summary>
    /// <param name="name">Name of the column</param>
    /// <param name="dataType">The sql data type of the column.</param>
    /// <param name="allowNull">Should the column allow nulls.</param>
    /// <param name="isPrimaryKey">Should the column act as a primary key.</param>
    private TableColumn(
      ColumnName name,
      SqlDbType dataType,
      bool allowNull,
      bool isPrimaryKey)
      : base(name, dataType)
    {
      AllowNull = allowNull;
      IsPrimaryKey = isPrimaryKey;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="TableColumn"/> class.
    /// </summary>
    /// <param name="name">Name of the column</param>
    /// <param name="dataType">The sql data type of the column.</param>
    /// <param name="allowNull">Should the column allow nulls.</param>
    /// <param name="isPrimaryKey">Should the column act as a primary key.</param>
    /// <returns>Returns an instance of the <see cref="TableColumn"/> initialized with the specified parameters.</returns>
    public static TableColumn Create(
      ColumnName name,
      SqlDbType dataType,
      bool allowNull = false,
      bool isPrimaryKey = false)
    {
      return new TableColumn(name, dataType, allowNull, isPrimaryKey);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="TableColumn"/> class.
    /// </summary>
    /// <typeparam name="TObjectTemplate">Object type to use as a template.</typeparam>
    /// <typeparam name="TColumnDataType">Type of the property.</typeparam>
    /// <param name="columnSelector">Property on type <typeparamref name="TObjectTemplate"/> to use as a template.</param>
    /// <param name="dataType">The sql data type of the column. If not defined then type will be inferred from <typeparamref name="TColumnDataType"/>.</param>
    /// <param name="allowNull">Should the column allow nulls. If not defined then type will be inferred from <typeparamref name="TColumnDataType"/>.</param>
    /// <param name="isPrimaryKey">Should the column act as a primary key.</param>
    /// <returns>Returns an instance of the <see cref="TableColumn"/> initialized with the specified parameters.</returns>
    public static TableColumn Create<TObjectTemplate, TColumnDataType>(
      Expression<Func<TObjectTemplate, TColumnDataType>> columnSelector,
      SqlDbType? dataType = null,
      bool? allowNull = null,
      bool isPrimaryKey = false)
      where TObjectTemplate : class
    {
      ColumnName columnName = (ColumnName)columnSelector.GetMemberName();
      Type propertyType = typeof(TColumnDataType);
      var columnDataType = dataType ?? ClrTypeToSqlDbTypeMapper.GetSqlDbTypeFromClrType(propertyType);
      var columnNullable = allowNull ?? propertyType.IsTypeNullable();
      return Create(columnName, columnDataType, columnNullable, isPrimaryKey);
    }

    /// <summary>
    /// Does the column allow nulls.
    /// </summary>
    public bool AllowNull { get; }

    /// <summary>
    /// Does the column represent the primary key.
    /// </summary>
    public bool IsPrimaryKey { get; }
  }
}
