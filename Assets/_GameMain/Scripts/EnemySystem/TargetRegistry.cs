using System.Collections.Generic;

public class TargetRegistry : ITargetRegistry
{
    private readonly List<ITarget> m_targets = new List<ITarget>();
    public IReadOnlyList<ITarget> m_Targets => m_targets;

    public void Register(ITarget target)
    {
        if (!m_targets.Contains(target))
            m_targets.Add(target);
    }

    public void Unregister(ITarget target)
    {
        m_targets.Remove(target);
    }
}