using System.Data;

namespace SqlScriptBuilder
{
  /// <summary>
  /// Represents a column with simple information.
  /// </summary>
  public class Column
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Column"/> class.
    /// </summary>
    /// <param name="name">Name of the column</param>
    /// <param name="dataType">The sql data type of the column.</param>
    public Column(ColumnName name, SqlDbType dataType)
    {
      ScriptBuilderHelper.ValidateName(name);
      Name = name;
      DataType = dataType;
    }

    /// <summary>
    /// The name of the column.
    /// </summary>
    public ColumnName Name { get; }

    /// <summary>
    /// The sql data type of the column.
    /// </summary>
    public SqlDbType DataType { get; }
  }
}
