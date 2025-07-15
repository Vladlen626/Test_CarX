using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Zenject;

[RequireComponent(typeof(ProjectileVisual))]
public class Projectile : MonoBehaviour, IProjectile
{
    [SerializeField] private float m_lifeTime = 5f;
    public float m_speed { get; private set; }
    public int m_damage { get; private set; }

    private CancellationTokenSource m_lifeTimeCts;
    private IProjectileMovement m_movementStrategy;
    private ProjectileVisual m_projectileVisual;

    public class Pool : MonoMemoryPool<Vector3, Quaternion, Projectile>
    {
        protected override void Reinitialize(Vector3 position, Quaternion rotation, Projectile item)
        {
            item.transform.position = position;
            item.transform.rotation = rotation;
            item.gameObject.SetActive(true);
        }
    }
    private void Awake()
    {
        m_projectileVisual = GetComponent<ProjectileVisual>();
    }
    
    public void SetVisualByMovementType(ProjectileType type)
    {
        m_projectileVisual.SetVisual(type);
    }
    
    public void Construct(IProjectileMovement movementStrategy)
    {
        m_movementStrategy = movementStrategy;
    }

    public void Deactivate()
    {
        m_lifeTimeCts?.Cancel();
        m_lifeTimeCts = null;
        gameObject.SetActive(false);
    }
    
    public void Fire(float speed, int damage)
    {
        m_speed = speed;
        m_damage = damage;
        m_movementStrategy.MoveAsync(this).Forget();
        
        m_lifeTimeCts?.Cancel();
        m_lifeTimeCts = new CancellationTokenSource();
        LifeTimeWatcher(m_lifeTimeCts.Token).Forget();
    }
    
    private async UniTaskVoid LifeTimeWatcher(CancellationToken token)
    {
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(m_lifeTime), cancellationToken: token);
            if (!token.IsCancellationRequested)
                Deactivate();
        }
        catch (OperationCanceledException) { }
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.gameObject.GetComponent<EnemyView>();
        if (enemy == null)
            return;

        enemy.TakeDamage(m_damage);
        Deactivate();
    }
}