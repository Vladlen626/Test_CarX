using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using DiContainer = Zenject.DiContainer;

[System.Serializable]
public class TowerByProjectileTypePrefab
{
    public ProjectileType projectileType;
    public Tower towerPrefab;
}

public class TowerFactory : MonoBehaviour, ITowerFactory
{
    [SerializeField] private List<TowerByProjectileTypePrefab> towerPrefabs;

    private Dictionary<ProjectileType, Tower> _prefabDict;
    private DiContainer _container;

    [Inject]
    public void Construct(DiContainer container)
    {
        _container = container;
        _prefabDict = new Dictionary<ProjectileType, Tower>();
        foreach (var pair in towerPrefabs)
        {
            if (!_prefabDict.ContainsKey(pair.projectileType))
                _prefabDict.Add(pair.projectileType, pair.towerPrefab);
        }
    }

    public Tower CreateTower(Vector3 position, ProjectileType towerType)
    {
        if (!_prefabDict.TryGetValue(towerType, out var prefab) || prefab == null)
            return null;
        
        var tower = _container.InstantiatePrefabForComponent<Tower>(prefab, position, Quaternion.identity, null);
        tower.Init(towerType);
        return tower;
    }
}