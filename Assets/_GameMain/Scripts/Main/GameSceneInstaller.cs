using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] private TowerFactory towerFactoryPrefab;
    [SerializeField] private EnemySpawner enemySpawnerPrefab; 
    
    [SerializeField] private Projectile projectileControllerPrefab;
    [SerializeField] private Enemy enemyPrefab;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<Main>().AsSingle();
        
        Container.Bind<ITowerFactory>()
            .To<TowerFactory>()
            .FromComponentInNewPrefab(towerFactoryPrefab)
            .AsSingle();
        
        Container.Bind<TowerSpawnService>().AsSingle();
        
        Container.Bind<IProjectileFactory>().To<ProjectileFactory>().AsSingle();
        Container.Bind<ITargetRegistry>().To<TargetRegistry>().AsSingle();
        
        Container.Bind<EnemySpawner>()
            .FromComponentInNewPrefab(enemySpawnerPrefab)
            .AsSingle();
        
        Container.BindMemoryPool<Projectile, Projectile.Pool>()
            .WithInitialSize(30)
            .FromComponentInNewPrefab(projectileControllerPrefab);
        
        Container.BindMemoryPool<Enemy, Enemy.Pool>()
            .WithInitialSize(20)
            .FromComponentInNewPrefab(enemyPrefab);
    }
    
    
}