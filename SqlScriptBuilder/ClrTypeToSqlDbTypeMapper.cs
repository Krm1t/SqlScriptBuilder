using System;
using System.Collections.Generic;
using System.Data;

namespace SqlScriptBuilder
{
  public static class ClrTypeToSqlDbTypeMapper
  {
    private static Dictionary<Type, SqlDbType> _clrTypeToSqlTypeMaps;

    /// <summary>
    /// Initializes the <see cref="ClrTypeToSqlDbTypeMapper"/> class.
    /// </summary>
    static ClrTypeToSqlDbTypeMapper()
    {
      _clrTypeToSqlTypeMaps = new Dictionary<Type, SqlDbType>
      {
        { typeof (bool), SqlDbType.Bit },
        { typeof (bool?), SqlDbType.Bit },
        { typeof (byte), SqlDbType.TinyInt },
        { typeof (byte?), SqlDbType.TinyInt },
        { typeof (string), SqlDbType.NVarChar },
        { typeof (DateTime), SqlDbType.DateTime },
        { typeof (DateTime?), SqlDbType.DateTime },
        { typeof (DateTimeOffset), SqlDbType.DateTimeOffset },
        { typeof (DateTimeOffset?), SqlDbType.DateTimeOffset },
        { typeof (short), SqlDbType.SmallInt },
        { typeof (short?), SqlDbType.SmallInt },
        { typeof (int), SqlDbType.Int },
        { typeof (int?), SqlDbType.Int },
        { typeof (long), SqlDbType.BigInt },
        { typeof (long?), SqlDbType.BigInt },
        { typeof (decimal), SqlDbType.Decimal },
        { typeof (decimal?), SqlDbType.Decimal },
        { typeof (double), SqlDbType.Float },
        { typeof (double?), SqlDbType.Float },
        { typeof (float), SqlDbType.Real },
        { typeof (float?), SqlDbType.Real },
        { typeof (TimeSpan), SqlDbType.Time },
        { typeof (TimeSpan?), SqlDbType.Time },
        { typeof (Guid), SqlDbType.UniqueIdentifier },
        { typeof (Guid?), SqlDbType.UniqueIdentifier },
        { typeof (byte[]), SqlDbType.Binary },
        { typeof (byte?[]), SqlDbType.Binary },
        { typeof (char[]), SqlDbType.Char },
        { typeof (char?[]), SqlDbType.Char }
       };
    }

    /// <summary>
    /// Gets the mapped SqlDbType for the specified CLR type.
    /// </summary>
    /// <param name="clrType">The CLR Type to get mapped SqlDbType for.</param>
    /// <returns></returns>
    public static SqlDbType GetSqlDbTypeFromClrType(Type clrType)
    {
      if (!_clrTypeToSqlTypeMaps.ContainsKey(clrType))
        throw new ArgumentOutOfRangeException("clrType", $"No mapped type found for '{clrType.FullName}'!");

      _clrTypeToSqlTypeMaps.TryGetValue(clrType, out SqlDbType result);
      return result;
    }
  }
}
