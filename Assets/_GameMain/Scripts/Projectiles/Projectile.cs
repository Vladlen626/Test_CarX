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
    private Rigidbody m_rb;

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
        m_rb = GetComponent<Rigidbody>();
        SetPhysicsEnabled(false);
        m_projectileVisual = GetComponent<ProjectileVisual>();
    }
    
    public void SetPhysicsEnabled(bool newPhysState, Vector3? velocity = null)
    {
        if (!m_rb) return;

        m_rb.isKinematic = !newPhysState;
        m_rb.useGravity = newPhysState;
        m_rb.velocity = newPhysState && velocity.HasValue ? velocity.Value : Vector3.zero;
        m_rb.angularVelocity = Vector3.zero;
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
        SetPhysicsEnabled(false);
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