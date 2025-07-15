using System;
using UnityEngine;

public class EnemyModel
{
    private readonly EnemyConfigSO m_config;
    private int m_currentHp;
    private Vector3 m_position;
    private Vector3 m_velocity;
    private Vector3 m_moveTargetPosition;
    private bool m_isAlive;

    public event Action OnDie;

    public EnemyModel(EnemyConfigSO config)
    {
        m_config = config;
        Reset();
    }

    public void Reset()
    {
        m_currentHp = m_config.m_maxHP;
        m_isAlive = true;
        m_velocity = Vector3.zero;
    }

    public bool IsAlive => m_isAlive;
    public Vector3 Position => m_position;
    public Vector3 Velocity => m_velocity;
    public float Speed => m_config.m_speed;
    public float ReachDistance => m_config.m_reachDistance;

    public void SetMoveTarget(Vector3 pos)
    {
        m_moveTargetPosition = pos;
    }

    public void Tick(Vector3 currentPos)
    {
        if (!m_isAlive) return;
        var dir = (m_moveTargetPosition - currentPos).normalized;
        var newPos = currentPos + dir * (m_config.m_speed * Time.deltaTime);
        m_velocity = (newPos - currentPos) / Time.deltaTime;
        m_position = newPos;
    }

    public void TakeDamage(int amount)
    {
        if (!m_isAlive) return;
        m_currentHp -= amount;
        if (m_currentHp <= 0)
        {
            m_isAlive = false;
            OnDie?.Invoke();
        }
    }

    public bool IsReachTarget()
    {
        return Vector3.Distance(m_position, m_moveTargetPosition) <= m_config.m_reachDistance;
    }
}