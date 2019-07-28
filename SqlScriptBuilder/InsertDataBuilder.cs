namespace SqlScriptBuilder
{
  public class InsertDataBuilder : SectionBuilderBase
  {
    internal InsertDataBuilder(ScriptBuilder owner, string destinationTable)
      : base(owner)
    {
      ScriptBuilderHelper.ValidateName(destinationTable);
      DestinationTable = destinationTable;
    }

    public string DestinationTable { get; }
  }

  public class InsertDataBuilder<TObjectTemplate> : InsertDataBuilder
  {
    internal InsertDataBuilder(ScriptBuilder owner, string destinationTable)
      : base(owner, destinationTable)
    {
    }
  }
}
