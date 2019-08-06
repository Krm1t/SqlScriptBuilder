using System;

namespace SqlScriptBuilder
{
  /// <summary>
  /// Contains common logic for creating script sections. This class is abstract.
  /// </summary>
  public abstract class SectionBuilderBase
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="SectionBuilderBase"/> class.
    /// </summary>
    /// <param name="owner">The <see cref="ScriptBuilder"/> that owns this section builder.</param>
    public SectionBuilderBase(ScriptBuilder owner)
    {
      Owner = owner ?? throw new ArgumentNullException(nameof(owner));
      IsFinalized = false;
    }

    /// <summary>
    /// The <see cref="ScriptBuilder"/> that owns this section builder.
    /// </summary>
    public ScriptBuilder Owner { get; }

    /// <summary>
    /// Indicates if the script section have been finalized.
    /// </summary>
    public bool IsFinalized { get; private set; }

    /// <summary>
    /// Finalizes the script section and returns focus to the <see cref="ScriptBuilder"/>.
    /// </summary>
    /// <returns>The <see cref="ScriptBuilder"/> that owns this section.</returns>
    public virtual ScriptBuilder EndSection()
    {
      Owner.AddSectionBuilder(this);
      IsFinalized = true;
      return Owner;
    }
  }
}
