using UnityEngine;

public interface ITarget
{
    Vector3 m_position { get; }
    Vector3 m_velocity { get; } 
    void TakeDamage(int amount);
    bool m_isAlive { get; }
}