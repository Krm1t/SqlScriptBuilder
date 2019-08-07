using System.Collections.Generic;

namespace SqlScriptBuilder
{
  /* Definer hvilke kolonner som skal have indsat data.
  * 
  * Definer data på én af 2 måder.
  * 1. Indsæt statiske værdier - Sørg gerne for at understøtte variabler.
  * 2. Select statement - Giver adgang til SelectionBuilder
  * 
  * I begge scenarier kommer man tilbage til en speciel instans så man kan tilføje flere inserts til den samme tabel uden at skulle definere kolonner igen.
  */

  public class InsertDataBuilder : SectionBuilderBase<ScriptBuilder>, IOwner
  {
    private readonly Dictionary<string, Column> _columns;
    private DataBuilder _defineData;

    private InsertDataBuilder(
      ScriptBuilder owner,
      string destinationTable)
      : base(owner)
    {
      DestinationTable = destinationTable;
      _columns = new Dictionary<string, Column>();
    }

    internal InsertDataBuilder(
      ScriptBuilder owner,
      VariableName destinationTable)
      : this(owner, destinationTable.ToString())
    {
    }

    internal InsertDataBuilder(
      ScriptBuilder owner,
      TableName destinationTable)
     : this(owner, destinationTable.ToString())
    {
    }

    /// <summary>
    /// The columns that have been added to the section.
    /// </summary>
    protected IReadOnlyDictionary<string, Column> Columns
    {
      get
      {
        return _columns;
      }
    }

    /// <summary>
    /// The name of the destination table.
    /// </summary>
    public string DestinationTable { get; }

    public DataBuilder DefineData
    {
      get
      {
        if (_defineData == null)
          _defineData = new DataBuilder(this);

        return _defineData;
      }
    }

    protected bool IsDataBuilderCreated()
    {
      return _defineData != null;
    }

    /// <summary>
    /// Checks if a particular column name have already been added to the table.
    /// </summary>
    /// <param name="name">Name of the column to check for.</param>
    /// <returns>Returns true if the column already exist in the table; Otherwise, returns false.</returns>
    public bool ColumnExists(string name)
    {
      return _columns.ContainsKey(name);
    }

    void IOwner.AddSection(SectionBuilderBase section)
    {
      throw new System.NotImplementedException();
    }
  }

  // *******************************************               **************************************************
  // *******************************************Generic version**************************************************
  // *******************************************               **************************************************

  public class InsertDataBuilder<TObjectTemplate> : InsertDataBuilder
  {
    // TODO: Fix
    internal InsertDataBuilder(
      ScriptBuilder owner,
      string destinationTable)
      : base(
          owner,
          string.IsNullOrWhiteSpace(destinationTable) ? (TableName)typeof(TObjectTemplate).Name : (TableName)destinationTable)
    {
    }
  }
}