using System.Collections.Generic;

public class TargetRegistry : ITargetRegistry
{
    private readonly List<ITarget> _targets = new List<ITarget>();

    public IReadOnlyList<ITarget> Targets => _targets;

    public void Register(ITarget target)
    {
        if (!_targets.Contains(target))
            _targets.Add(target);
    }

    public void Unregister(ITarget target)
    {
        _targets.Remove(target);
    }
}