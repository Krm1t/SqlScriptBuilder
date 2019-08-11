namespace SqlScriptBuilder
{
  public interface IScriptGenerator
  {
    string GenerateSection(SectionBuilder sectionBuilder);
  }
}