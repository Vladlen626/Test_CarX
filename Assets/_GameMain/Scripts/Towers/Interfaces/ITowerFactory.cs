using UnityEngine;

public interface ITowerFactory<out T>
{
    T CreateTower(Vector3 position, ProjectileType towerType);
}