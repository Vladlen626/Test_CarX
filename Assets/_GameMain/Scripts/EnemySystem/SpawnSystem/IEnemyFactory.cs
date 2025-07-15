using UnityEngine;

public interface IEnemyFactory<out T>
{
    T TryCreate(EnemyType type, Vector3 spawnPosition, Vector3 moveTarget);
}