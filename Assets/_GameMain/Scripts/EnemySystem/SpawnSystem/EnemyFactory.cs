using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyFactory : IEnemyFactory<EnemyView>
{
    private readonly Dictionary<EnemyType, EnemyConfigSO> m_configMap;
    private readonly List<EnemyView> m_pool = new List<EnemyView>();
    private readonly DiContainer m_container;

    [Inject]
    public EnemyFactory(
        List<EnemyConfigSO> enemyConfigs,
        DiContainer container,
        ITargetRegistry targetRegistry)
    {
        m_configMap = new Dictionary<EnemyType, EnemyConfigSO>();
        foreach (var config in enemyConfigs)
        {
            m_configMap[config.m_type] = config;
        }
        m_container = container;
    }

    public EnemyView TryCreate(EnemyType type, Vector3 spawnPosition, Vector3 moveTarget)
    {
        m_configMap.TryGetValue(type, out var m_config);
        if (m_config == null)
            return null;

        var m_view = GetFromPool(type);
        if (m_view == null)
        {
            m_view = m_container.InstantiatePrefabForComponent<EnemyView>(m_config.m_enemyViewPrefab, spawnPosition, Quaternion.identity, null);
            m_container.Inject(m_view);
            m_pool.Add(m_view);
        }
        else
        {
            m_view.transform.position = spawnPosition;
            m_view.gameObject.SetActive(true);
        }

        var m_model = new EnemyModel(m_config);
        m_model.SetMoveTarget(moveTarget);
        m_view.Init(m_model);

        return m_view;
    }

    private EnemyView GetFromPool(EnemyType type)
    {
        foreach (var m_enemy in m_pool)
        {
            if (!m_enemy.gameObject.activeInHierarchy &&
                m_enemy.name.Contains(type.ToString()))
                return m_enemy;
        }
        return null;
    }
    
}
