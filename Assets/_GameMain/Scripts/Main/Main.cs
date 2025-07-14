using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class Main : IInitializable, IDisposable
{
    private readonly TowerSpawnService _towerSpawnService;
    private readonly EnemySpawner _enemySpawner;

    [Inject]
    public Main(TowerSpawnService towerSpawnService, EnemySpawner enemySpawner)
    {
        _towerSpawnService = towerSpawnService;
        _enemySpawner = enemySpawner;
    }
    
    public void Initialize()
    {
        StartGame().Forget();
    }
    
    public void Dispose()
    {
        _enemySpawner.StopSpawn();
    }
    
    // _____________ Private _____________

    private async UniTaskVoid StartGame()
    {
        await InitializeAll();
        _enemySpawner.StartSpawn();
    }

    private async UniTask InitializeAll()
    {
        _towerSpawnService.SpawnAllFromPlaceholders();
        
        var moveTarget = GameObject.FindWithTag("EnemiesTarget")?.transform;
        _enemySpawner.SetMoveTarget(moveTarget);
        
        await UniTask.Yield();
    }
}