using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [Header("Scene Instance")]
    [SerializeField] private TowerSpawnPointService m_towerSpawnPointService;
    [SerializeField] private Transform m_enemiesGoal;
    
    [Header("Prefabs")]
    [SerializeField] private Projectile m_projectileControllerPrefab;
    [SerializeField] private EnemySpawner m_enemySpawnerPrefab;
    
    [Header("Configs")]
    [SerializeField] private List<TowerConfigSO> m_towerConfigs;
    [SerializeField] private List<EnemyConfigSO> m_enemyConfigs;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<Main>().AsSingle().NonLazy();
        
        Container.Bind<TowerSpawnService>().AsSingle();
        Container.Bind<IProjectileFactory>().To<ProjectileFactory>().AsSingle();
        Container.Bind<ITargetRegistry>().To<TargetRegistry>().AsSingle();
        Container.Bind<ITowerFactory<TowerView>>().To<TowerFactory>().AsSingle();
        Container.Bind<IEnemyFactory<EnemyView>>().To<EnemyFactory>().AsSingle();
        
        Container.Bind<EnemySpawner>()
            .FromComponentInNewPrefab(m_enemySpawnerPrefab)
            .AsSingle();
        
        //Pools
        Container.BindMemoryPool<Projectile, Projectile.Pool>()
            .WithInitialSize(30)
            .FromComponentInNewPrefab(m_projectileControllerPrefab);
        
        // Instance
        Container.Bind<TowerSpawnPointService>().FromInstance(m_towerSpawnPointService).AsSingle();
        Container.Bind<Transform>().WithId("EnemiesGoal").FromInstance(m_enemiesGoal).AsSingle();
        
        //Configs
        Container.Bind<List<TowerConfigSO>>().FromInstance(m_towerConfigs).AsSingle();
        Container.Bind<List<EnemyConfigSO>>().FromInstance(m_enemyConfigs).AsSingle();
       
    }
    
}