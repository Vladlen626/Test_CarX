using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [Header("Scene Instance")]
    [SerializeField] private TowerSpawnPointService m_towerSpawnPointService;
    [SerializeField] private Transform m_enemiesGoal;
    
    [Header("Prefabs")]
    [SerializeField] private TowerFactory m_towerFactoryPrefab;
    [SerializeField] private EnemySpawner m_enemySpawnerPrefab; 
    [SerializeField] private Projectile m_projectileControllerPrefab;
    [SerializeField] private Enemy m_enemyPrefab;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<Main>().AsSingle().NonLazy();
        
        // Instance
        Container.Bind<TowerSpawnPointService>().FromInstance(m_towerSpawnPointService).AsSingle();
        Container.Bind<Transform>().WithId("EnemiesGoal").FromInstance(m_enemiesGoal).AsSingle();
        
        Container.Bind<TowerSpawnService>().AsSingle();
        Container.Bind<IProjectileFactory>().To<ProjectileFactory>().AsSingle();
        Container.Bind<ITargetRegistry>().To<TargetRegistry>().AsSingle();
        
        Container.Bind<ITowerFactory<Tower>>()
            .To<TowerFactory>()
            .FromComponentInNewPrefab(m_towerFactoryPrefab)
            .AsSingle();
        
        Container.Bind<EnemySpawner>()
            .FromComponentInNewPrefab(m_enemySpawnerPrefab)
            .AsSingle();
        
        //Pools
        Container.BindMemoryPool<Projectile, Projectile.Pool>()
            .WithInitialSize(30)
            .FromComponentInNewPrefab(m_projectileControllerPrefab);
        
        Container.BindMemoryPool<Enemy, Enemy.Pool>()
            .WithInitialSize(20)
            .FromComponentInNewPrefab(m_enemyPrefab);
        
       
    }
    
}