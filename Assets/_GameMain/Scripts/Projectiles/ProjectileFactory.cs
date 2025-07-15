using UnityEngine;
using Zenject;

public class ProjectileFactory : IProjectileFactory
{
    private readonly Projectile.Pool m_pool;
    private readonly DiContainer m_container;

    [Inject]
    public ProjectileFactory(Projectile.Pool mPool, DiContainer mContainer)
    {
        m_pool = mPool;
        m_container = mContainer;
    }

    public IProjectile Create(Vector3 position, Quaternion rotation, ProjectileType type,
        ITarget target = null)
    {
        IProjectileMovement movementStrategy;

        switch (type)
        {
            case ProjectileType.Straight:
                movementStrategy = new StraightMovement();
                break;
            case ProjectileType.Homing:
                movementStrategy = new HomingMovement(target);
                break;
            default:
                movementStrategy = new StraightMovement();
                break;
        }

        var projectile = m_pool.Spawn(position, rotation);
        projectile.Construct(movementStrategy);
        projectile.SetVisualByMovementType(type);
        return projectile;
    }
}