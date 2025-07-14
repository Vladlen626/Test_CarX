using UnityEngine;
using Zenject;

public class ProjectileFactory : IProjectileFactory
{
    private readonly Projectile.Pool _pool;
    private readonly DiContainer _container;

    [Inject]
    public ProjectileFactory(Projectile.Pool pool, DiContainer container)
    {
        _pool = pool;
        _container = container;
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

        var projectile = _pool.Spawn(position, rotation);
        projectile.Construct(movementStrategy);
        projectile.SetVisualByMovementType(type);
        return projectile;
    }
}