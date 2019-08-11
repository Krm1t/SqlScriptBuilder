using System.Data;
using Xunit;

namespace SqlScriptBuilder.Tests
{
  public class EndToEndTests
  {
    [Fact]
    public void EndToEnd_CreateTableVariableWithNoData_ExpectScriptWithVariableDeclared()
    {
      // Arrange
      var scriptBuilder = new ScriptBuilder();

      // Act
      var script = scriptBuilder
        .DeclareTableVariable("myTableVariable")
        .AddColumn("Column1", SqlDbType.BigInt, false, true)
        .AddColumn("Column2", SqlDbType.NVarChar)
        .AddColumn("Column3", SqlDbType.Decimal)
        .EndSection()
        .ToString();

      // Assert
    }
  }
}
