using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Zenject;

[RequireComponent(typeof(ProjectileVisual))]
public class Projectile : MonoBehaviour, IProjectile
{
    public float m_speed { get; private set; }
    public int m_damage { get; private set; }
    
    private IProjectileMovement m_movementStrategy;
    private ProjectileVisual _projectileVisual;
    
    public class Pool : MonoMemoryPool<Vector3, Quaternion, Projectile>
    {
        protected override void Reinitialize(Vector3 position, Quaternion rotation, Projectile item)
        {
            item.transform.position = position;
            item.transform.rotation = rotation;
            item.gameObject.SetActive(true);
        }
    }

    public void Construct(IProjectileMovement movementStrategy)
    {
        m_movementStrategy = movementStrategy;
    }

    public void Fire(float speed, int damage)
    {
        m_speed = speed;
        m_damage = damage;
        m_movementStrategy.MoveAsync(this).Forget();
    }

    public void SetVisualByMovementType(ProjectileType type)
    {
        _projectileVisual.SetVisual(type);
    }
    
    private void Awake()
    {
        _projectileVisual = GetComponent<ProjectileVisual>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy == null)
            return;

        enemy.TakeDamage(m_damage);
        Destroy(gameObject);
    }
    
}