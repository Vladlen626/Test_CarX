using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float interval = 3f;

    private Transform _moveTarget;
    private Enemy.Pool _enemyPool;
    private bool _isSpawning = false;

    [Inject]
    public void Construct(Enemy.Pool enemyPool)
    {
        _enemyPool = enemyPool;
    }

    public void StartSpawn()
    {
        if (_isSpawning) return;
        _isSpawning = true;
        SpawnLoop().Forget();
    }
    
    public void SetMoveTarget(Transform moveTarget)
    {
        _moveTarget = moveTarget;
    }

    public void StopSpawn()
    {
        _isSpawning = false;
    }

    private async UniTaskVoid SpawnLoop()
    {
        while (_isSpawning)
        {
            if (_moveTarget)
            {
                var enemy = _enemyPool.Spawn(_moveTarget);
                enemy.transform.position = transform.position;
            }
            await UniTask.Delay(System.TimeSpan.FromSeconds(interval));
        }
    }
}