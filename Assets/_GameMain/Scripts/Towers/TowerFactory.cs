using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class TowerFactory : ITowerFactory<TowerView>
{
    private readonly List<TowerConfigSO> m_configs;
    private readonly DiContainer m_container;
    private readonly ITargetRegistry m_targetRegistry;

    [Inject]
    public TowerFactory(List<TowerConfigSO> configs, DiContainer container, ITargetRegistry targetRegistry)
    {
        m_configs = configs;
        m_container = container;
        m_targetRegistry = targetRegistry;
    }

    public TowerView CreateTower(Vector3 position, ProjectileType projectileType)
    {
        var config = m_configs.FirstOrDefault(c => c.m_projectileType == projectileType);
        if (!config || !config.m_towerViewPrefab)
            return null;

        var model = new TowerModel(
            m_targetRegistry,
            config.m_shootInterval,
            config.m_range,
            config.m_projectileSpeed,
            config.m_damage,
            config.m_enableLeadAim
        );

        var view = m_container.InstantiatePrefabForComponent<TowerView>(
            config.m_towerViewPrefab, position, Quaternion.identity, null);
        m_container.Inject(view);
        view.Init(model, projectileType);
        return view;
    }
}