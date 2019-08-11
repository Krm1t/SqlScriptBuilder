using System;

namespace SqlScriptBuilder
{
  public class MSSqlScriptGenerator : IScriptGenerator
  {
    public string GenerateSection(SectionBuilder sectionBuilder)
    {
      if (sectionBuilder is TableVariableBuilderBase tableVarSection)
      {
        return tableVarSection.ToString();
      }
      else
      {
        throw new InvalidOperationException($"Cannot generate script sections for type '{sectionBuilder.GetType().FullName}'!");
      }
    }
  }
}