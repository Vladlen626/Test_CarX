using UnityEngine;
using Zenject;

public class TowerSpawnService
{
    private readonly ITowerFactory _factory;

    [Inject]
    public TowerSpawnService(ITowerFactory factory)
    {
        _factory = factory;
    }

    public void SpawnAllFromPlaceholders()
    {
        foreach (var placeholder in Object.FindObjectsOfType<TowerPlaceholder>())
        {
            _factory.CreateTower(placeholder.transform.position, placeholder.projectileType);
            Object.Destroy(placeholder.gameObject);
        }
    }
}