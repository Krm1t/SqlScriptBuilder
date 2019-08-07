using System;

namespace SqlScriptBuilder
{
  public interface IOwner
  {
    void AddSection(SectionBuilderBase section);
  }

  /// <summary>
  /// Contains common logic for creating script sections. This class is abstract.
  /// </summary>
  public abstract class SectionBuilderBase
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="SectionBuilderBase{TOwner}"/> class.
    /// </summary>
    /// <param name="owner">The instance that owns this section builder.</param>
    public SectionBuilderBase()
    {
      IsFinalized = false;
    }

    /// <summary>
    /// Indicates if the script section have been finalized.
    /// </summary>
    public bool IsFinalized { get; private set; }

    /// <summary>
    /// Finalizes the section.
    /// </summary>
    protected void SetFinalized()
    {
      IsFinalized = true;
    }
  }

  /// <summary>
  /// Contains common logic for creating script sections. This class is abstract.
  /// </summary>
  public abstract class SectionBuilderBase<TOwner> : SectionBuilderBase
    where TOwner : class, IOwner
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="SectionBuilderBase{TOwner}"/> class.
    /// </summary>
    /// <param name="owner">The instance that owns this section builder.</param>
    public SectionBuilderBase(TOwner owner)
      : base()
    {
      Owner = owner ?? throw new ArgumentNullException(nameof(owner));
    }

    /// <summary>
    /// The <see cref="TOwner"/> that owns this section builder.
    /// </summary>
    public TOwner Owner { get; }

    /// <summary>
    /// Finalizes the script section and returns focus to the owner.
    /// </summary>
    /// <returns>The instance that owns this section.</returns>
    public virtual TOwner EndSection()
    {
      Owner.AddSection(this);
      SetFinalized();
      return Owner;
    }
  }
}
