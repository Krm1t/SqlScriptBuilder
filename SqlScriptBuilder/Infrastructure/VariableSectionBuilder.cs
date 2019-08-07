namespace SqlScriptBuilder
{
  /// <summary>
  /// Section builder for creating variables.
  /// </summary>
  public abstract class VariableSectionBuilder : SectionBuilderBase<ScriptBuilder>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="VariableSectionBuilder"/> class.
    /// </summary>
    /// <param name="owner">The <see cref="ScriptBuilder"/> that owns this <see cref="VariableSectionBuilder"/>.</param>
    /// <param name="name">The name that the veriable will get in the script.</param>
    public VariableSectionBuilder(ScriptBuilder owner, VariableName name)
      : base(owner)
    {
      Name = name;
    }

    /// <summary>
    /// Name of the variable without @ prefix.
    /// </summary>
    public VariableName Name { get; }
  }
}
