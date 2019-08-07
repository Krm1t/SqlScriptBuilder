using System.Collections.Generic;

namespace SqlScriptBuilder
{
  public class DataBuilder
  {
    private readonly InsertDataBuilder _owner;

    public DataBuilder(InsertDataBuilder owner)
    {
      _owner = owner;
    }

    public InsertDataBuilder FromValues(params string[] values)
    {
      return _owner;
    }

    public InsertDataBuilder FromValueList(IEnumerable<string[]> valueList)
    {
      return _owner;
    }

    public SelectDataBuilder FromSelect()
    {
      return null;
    }

    public object FromProcedure()
    {
      return null;
    }
  }
}