using UnityEngine;

public class TowerModel
{
    private readonly float m_shootInterval;
    private readonly float m_range;
    private readonly float m_projectileSpeed;
    private readonly int m_damage;
    private readonly ITargetRegistry m_targetRegistry;
    private readonly bool m_leadAimEnabled;

    private float m_timer;
    private ITarget m_currentTarget;
    private Vector3 m_aimPoint;

    public TowerModel(
        ITargetRegistry targetRegistry,
        float shootInterval,
        float range,
        float projectileSpeed,
        int damage,
        bool mLeadAimEnabled)
    {
        m_targetRegistry = targetRegistry;
        m_shootInterval = shootInterval;
        m_range = range;
        m_projectileSpeed = projectileSpeed;
        m_damage = damage;
        m_leadAimEnabled = mLeadAimEnabled;
    }

    public void FindLeadPoint(Vector3 towerPos, Vector3 shootPoint)
    {
        m_timer += Time.deltaTime;
        m_currentTarget = FindTarget(towerPos);

        if (m_currentTarget == null)
            return;

        m_aimPoint = m_currentTarget.m_position;
        if (m_leadAimEnabled)
            m_aimPoint = CoreHelper.GetLeadPoint(shootPoint, m_currentTarget, m_projectileSpeed, m_range);
    }

    public bool TryConsumeShot(out ITarget target, out Vector3 aimPoint, out int damage, out float projectileSpeed)
    {
        if (m_currentTarget != null && m_timer >= m_shootInterval)
        {
            m_timer = 0f;
            target = m_currentTarget;
            aimPoint = m_aimPoint;
            damage = m_damage;
            projectileSpeed = m_projectileSpeed;
            return true;
        }
        else
        {
            target = null;
            aimPoint = Vector3.zero;
            damage = 0;
            projectileSpeed = 0f;
            return false;
        }
    }

    private ITarget FindTarget(Vector3 towerPos)
    {
        ITarget closestTarget = null;
        var minDistance = float.MaxValue;

        foreach (var target in m_targetRegistry.m_Targets)
        {
            if (target == null || !target.m_isAlive)
                continue;

            var distance = Vector3.Distance(towerPos, target.m_position);
            
            if (distance > m_range || distance >= minDistance)
                continue;
            
            minDistance = distance;
            closestTarget = target;
        }

        return closestTarget;
    }
}