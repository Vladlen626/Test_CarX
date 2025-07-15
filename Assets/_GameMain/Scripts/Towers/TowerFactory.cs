using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using DiContainer = Zenject.DiContainer;

[System.Serializable]
public class TowerByProjectileTypePrefab
{
    public ProjectileType m_projectileType;
    public Tower m_towerPrefab;
}

public class TowerFactory : MonoBehaviour, ITowerFactory<Tower>
{
    [SerializeField] private List<TowerByProjectileTypePrefab> m_towerPrefabs;

    private Dictionary<ProjectileType, Tower> m_prefabDict;
    private DiContainer m_container;

    [Inject]
    public void Construct(DiContainer container)
    {
        m_container = container;
        m_prefabDict = new Dictionary<ProjectileType, Tower>();
        foreach (var pair in m_towerPrefabs)
        {
            if (!m_prefabDict.ContainsKey(pair.m_projectileType))
                m_prefabDict.Add(pair.m_projectileType, pair.m_towerPrefab);
        }
    }

    public Tower CreateTower(Vector3 position, ProjectileType towerType)
    {
        if (!m_prefabDict.TryGetValue(towerType, out var prefab) || prefab == null)
            return null;
        
        var tower = m_container.InstantiatePrefabForComponent<Tower>(prefab, position, Quaternion.identity, null);
        tower.Init(towerType);
        return tower;
    }
}