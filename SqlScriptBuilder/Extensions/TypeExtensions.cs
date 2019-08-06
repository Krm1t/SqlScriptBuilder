using System;

namespace SqlScriptBuilder
{
  internal static class TypeExtensions
  {
    public static bool IsTypeNullable(this Type clrType)
    {
      if (clrType == null) throw new ArgumentNullException(nameof(clrType));
      if (!clrType.IsGenericType || clrType.GetGenericTypeDefinition() != typeof(Nullable<>))
        return false;

      return true;
    }
  }
}
