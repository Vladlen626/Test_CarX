using UnityEngine;

public interface ITowerFactory
{
    Tower CreateTower(Vector3 position, ProjectileType towerType);
}