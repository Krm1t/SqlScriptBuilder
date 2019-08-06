using System;
using System.Linq.Expressions;

namespace SqlScriptBuilder
{
  internal static class ExpressionExtensions
  {
    public static string GetMemberName<T>(this Expression<T> source)
    {
      switch (source.Body)
      {
        case MemberExpression m:
          return m.Member.Name;
        case UnaryExpression u when u.Operand is MemberExpression m:
          return m.Member.Name;
        default:
          throw new NotImplementedException($"Expression type '{source.GetType().FullName}' is not supported!");
      }
    }
  }
}
