using System.Collections.Generic;

namespace SqlScriptBuilder
{
  public class InsertDataBuilder : SectionBuilderBase
  {
    private readonly Dictionary<string, Column> _columns;

    internal InsertDataBuilder(
      ScriptBuilder owner,
      string destinationTable)
      : base(owner)
    {
      ScriptBuilderHelper.ValidateName(destinationTable);
      DestinationTable = destinationTable;
      _columns = new Dictionary<string, Column>();
    }

    /// <summary>
    /// The name of the destination table
    /// </summary>
    public string DestinationTable { get; }

    /* Definer hvilke kolonner som skal have indsat data.
     * 
     * Definer data på én af 2 måder.
     * 1. Indsæt statiske værdier - Sørg gerne for at understøtte variabler.
     * 2. Select statement - Giver adgang til SelectionBuilder
     * 
     * I begge scenarier kommer man tilbage til en speciel instans så man kan tilføje flere inserts til den samme tabel uden at skulle definere kolonner igen.
     */

    internal InsertDataBuilder AddColumn(Column column)
    {
      if (IsFinalized) throw new ScriptBuilderException("Cannot add new columns to the section once it has been finalized!");

      if (_columns.ContainsKey(column.Name))
        throw new ScriptBuilderException($"Column name '{column.Name}' already exist!");

      _columns.Add(column.Name, column);
      return this;
    }
  }

  public class InsertDataBuilder<TObjectTemplate> : InsertDataBuilder
  {
    internal InsertDataBuilder(
      ScriptBuilder owner,
      string destinationTable)
      : base(
          owner,
          string.IsNullOrWhiteSpace(destinationTable) ? typeof(TObjectTemplate).Name : destinationTable)
    {
    }
  }
}