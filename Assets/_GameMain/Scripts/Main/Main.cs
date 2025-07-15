using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class Main : IInitializable, IDisposable
{
    private readonly TowerSpawnService m_towerSpawnService;
    private readonly EnemySpawner m_enemySpawner;

    [Inject]
    public Main(TowerSpawnService towerSpawnService, EnemySpawner enemySpawner)
    {
        m_towerSpawnService = towerSpawnService;
        m_enemySpawner = enemySpawner;
    }
    
    public void Initialize()
    {
        StartGame().Forget();
    }
    
    public void Dispose()
    {
        m_enemySpawner.StopSpawn();
    }
    
    // _____________ Private _____________

    private async UniTaskVoid StartGame()
    {
        await InitializeAll();
        m_enemySpawner.StartSpawn();
    }

    private async UniTask InitializeAll()
    {
        await UniTask.Yield();
        m_towerSpawnService.SpawnAllTowers();
    } 
}