using System.Linq;
using UnityEngine;
using Zenject;

public class TowerSpawnService
{
    private readonly ITowerFactory<Tower> m_factory;
    private readonly TowerSpawnPointService m_spawnPointService;

    [Inject]
    public TowerSpawnService(ITowerFactory<Tower> factory, TowerSpawnPointService spawnPointService)
    {
        m_factory = factory;
        m_spawnPointService = spawnPointService;
    }

    public void SpawnAllTowers()
    {
        foreach (var spawnPoint in m_spawnPointService.m_Points.ToArray())
        {
            m_factory.CreateTower(spawnPoint.transform.position, spawnPoint.m_projectileType);
            Object.Destroy(spawnPoint.gameObject);
        }
    }
}