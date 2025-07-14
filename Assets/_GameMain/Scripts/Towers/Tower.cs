using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class Tower : MonoBehaviour
{
    [SerializeField] private float shootInterval = 0.5f;
    [SerializeField] private float range = 6f;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private int damage = 10;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform turret;
    [SerializeField] private TurretRotationSettings rotationSettings;
    [SerializeField] private bool useTurretRotation;
    [FormerlySerializedAs("isEnableLeadAimPoint")] [FormerlySerializedAs("useLeadSelector")] [SerializeField] private bool enableLeadAimPoint;

    private IProjectileFactory _projectileFactory;
    private ITargetRegistry _targetRegistry;
    private TurretRotator _turretRotator;

    private ITarget _currentTarget;
    private Vector3 _currentAimPoint;
    private bool _isRunning;
    private ProjectileType _projectileType;

    [Inject]
    public void Construct(IProjectileFactory projectileFactory, ITargetRegistry targetRegistry)
    {
        _projectileFactory = projectileFactory;
        _targetRegistry = targetRegistry;
    }

    public void Init(ProjectileType projectileType)
    {
        _projectileType = projectileType;

        if (useTurretRotation && turret)
            _turretRotator = new TurretRotator(turret, rotationSettings);

        StartLogic();
    }

    private void StartLogic()
    {
        if (_isRunning) return;
        _isRunning = true;
        TargetingLoop().Forget();
        ShootingLoop().Forget();
        _turretRotator?.StartRotationLoop();
    }

    public void StopLogic()
    {
        _isRunning = false;
        _turretRotator?.StopRotationLoop();
    }
    
    private async UniTaskVoid TargetingLoop()
    {
        while (_isRunning)
        {
            _currentTarget = FindTarget();
            if (_currentTarget == null)
            {
                await UniTask.Yield();
                continue; 
            }
                
            
            _currentAimPoint = _currentTarget.Position;

            if (enableLeadAimPoint)
                _currentAimPoint = LeadCalculator.GetLeadPoint(shootPoint.position, _currentTarget, projectileSpeed);
            
            
            _turretRotator?.SetTargetPoint(_currentAimPoint);

            await UniTask.Yield();
        }
    }
    
    public ITarget FindTarget()
    {
        ITarget closestTarget = null;
        var minDistance = float.MaxValue;

        foreach (var target in _targetRegistry.Targets)
        {
            if (target == null || !target.IsAlive)
                continue;

            var distance = Vector3.Distance(transform.position, target.Position);
            if (!(distance <= range) || !(distance < minDistance)) continue;
            minDistance = distance;
            closestTarget = target;
        }

        return closestTarget;
    }
    
    private async UniTaskVoid ShootingLoop()
    {
        while (_isRunning)
        {
            if (_currentTarget != null)
            {
                Shoot(_currentTarget, _currentAimPoint);
            }
            
            await UniTask.Delay(System.TimeSpan.FromSeconds(shootInterval));
        }
    }
    
    public void Shoot(ITarget target, Vector3 aimPoint)
    {
        var rotation = Quaternion.LookRotation((aimPoint - shootPoint.position).normalized);
        var projectile = _projectileFactory.Create(shootPoint.position, rotation, _projectileType, target);
        projectile.Fire(projectileSpeed, damage);
    }
}
