using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class Tower : MonoBehaviour
{
    [SerializeField] private float m_shootInterval = 0.5f;
    [SerializeField] private float m_range = 6f;
    [SerializeField] private float m_projectileSpeed = 10f;
    [SerializeField] private int m_damage = 10;
    [SerializeField] private Transform m_shootPoint;
    [SerializeField] private Transform m_turret;
    [SerializeField] private TurretRotationSettings m_rotationSettings;
    [SerializeField] private bool m_useTurretRotation;
    [SerializeField] private bool m_enableLeadAimPoint;

    private IProjectileFactory m_projectileFactory;
    private ITargetRegistry m_targetRegistry;
    private TurretRotator m_turretRotator;

    private ITarget m_currentTarget;
    private Vector3 m_currentAimPoint;
    private bool m_isRunning;
    private ProjectileType m_projectileType;

    [Inject]
    public void Construct(IProjectileFactory projectileFactory, ITargetRegistry targetRegistry)
    {
        m_projectileFactory = projectileFactory;
        m_targetRegistry = targetRegistry;
    }

    public void Init(ProjectileType projectileType)
    {
        m_projectileType = projectileType;

        if (m_useTurretRotation && m_turret)
            m_turretRotator = new TurretRotator(m_turret, m_rotationSettings);

        StartLogic();
    }

    private void StartLogic()
    {
        if (m_isRunning) return;
        m_isRunning = true;
        TargetingLoop().Forget();
        ShootingLoop().Forget();
        m_turretRotator?.StartRotationLoop();
    }

    public void StopLogic()
    {
        m_isRunning = false;
        m_turretRotator?.StopRotationLoop();
    }
    
    private async UniTaskVoid TargetingLoop()
    {
        while (m_isRunning)
        {
            m_currentTarget = FindTarget();
            if (m_currentTarget == null)
            {
                await UniTask.Yield();
                continue; 
            }
                
            
            m_currentAimPoint = m_currentTarget.m_position;

            if (m_enableLeadAimPoint)
                m_currentAimPoint = CoreHelper.GetLeadPoint(m_shootPoint.position, m_currentTarget, m_projectileSpeed, m_range);
            
            
            m_turretRotator?.SetTargetPoint(m_currentAimPoint);

            await UniTask.Yield();
        }
    }
    
    public ITarget FindTarget()
    {
        ITarget closestTarget = null;
        var minDistance = float.MaxValue;

        foreach (var target in m_targetRegistry.m_Targets)
        {
            if (target == null || !target.m_isAlive)
                continue;

            var distance = Vector3.Distance(transform.position, target.m_position);
            if (!(distance <= m_range) || !(distance < minDistance)) continue;
            minDistance = distance;
            closestTarget = target;
        }

        return closestTarget;
    }
    
    private async UniTaskVoid ShootingLoop()
    {
        while (m_isRunning)
        {
            if (m_currentTarget != null)
            {
                Shoot(m_currentTarget, m_currentAimPoint);
            }
            
            await UniTask.Delay(System.TimeSpan.FromSeconds(m_shootInterval));
        }
    }
    
    public void Shoot(ITarget target, Vector3 aimPoint)
    {
        var rotation = Quaternion.LookRotation((aimPoint - m_shootPoint.position).normalized);
        var projectile = m_projectileFactory.Create(m_shootPoint.position, rotation, m_projectileType, target);
        projectile.Fire(m_projectileSpeed, m_damage);
    }
}
