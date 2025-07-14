using System.Collections.Generic;

public interface ITargetRegistry
{
    IReadOnlyList<ITarget> Targets { get; }
    void Register(ITarget target);
    void Unregister(ITarget target);
}