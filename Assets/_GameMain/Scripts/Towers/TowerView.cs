using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class TowerView : MonoBehaviour
{
    [SerializeField] private Transform m_shootPoint;
    [SerializeField] private Transform m_turret;
    [SerializeField] private TurretRotationSettings m_rotationSettings;

    private IProjectileFactory m_projectileFactory;
    private TowerModel m_model;
    private TurretRotator m_turretRotator;
    private ProjectileType m_projectileType;
    private bool m_isRunning;

    [Inject]
    public void Construct(IProjectileFactory projectileFactory)
    {
        m_projectileFactory = projectileFactory;
    }

    public void Init(TowerModel model, ProjectileType projectileType)
    {
        m_model = model;
        m_projectileType = projectileType;
        if (m_rotationSettings.m_useTurretRotation && m_turret)
            m_turretRotator = new TurretRotator(m_turret, m_rotationSettings);
        
        StartLogic();
    }
    
    private void OnDisable()
    {
        StopLogic();
    }

    private void StartLogic()
    {
        m_isRunning = true;
        if (m_rotationSettings.m_useTurretRotation)
            m_turretRotator.StartRotationLoop();
        
        MainLoop().Forget();
    }
    
    private async UniTaskVoid MainLoop()
    {
        while (m_isRunning)
        {
            m_model.FindLeadPoint(transform.position, m_shootPoint.position);

            if (m_model.TryConsumeShot(out var target, out var aim, out var damage, out var speed))
            {
                Shoot(target, aim, damage, speed);
            }

            if (m_rotationSettings.m_useTurretRotation && target != null)
                m_turretRotator.SetTargetPoint(aim);

            await UniTask.Yield();
        }
    }
    public void StopLogic()
    {
        m_isRunning = false;
        m_turretRotator?.StopRotationLoop();
    }
    
    private void Shoot(ITarget target, Vector3 aimPoint, int damage, float projectileSpeed)
    {
        var direction = (aimPoint - m_shootPoint.position).normalized;
        var rotation = Quaternion.LookRotation(direction);
        var projectile = m_projectileFactory.Create(m_shootPoint.position, rotation, m_projectileType, target);
        projectile.Fire(projectileSpeed, damage);
    }
}
