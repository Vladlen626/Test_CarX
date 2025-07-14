using UnityEngine;

public interface IProjectileFactory
{
    IProjectile Create(Vector3 position, Quaternion rotation, ProjectileType type,
        ITarget target = null);
}