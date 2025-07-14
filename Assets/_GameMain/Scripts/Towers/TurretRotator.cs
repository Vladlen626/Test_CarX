using Cysharp.Threading.Tasks;
using UnityEngine;

public class TurretRotator
{
    private readonly Transform _turret;
    private readonly TurretRotationSettings _settings;
    private Vector3 _targetPoint;
    private bool _isRunning;

    public TurretRotator(Transform turret, TurretRotationSettings settings)
    {
        _turret = turret;
        _settings = settings;
        _targetPoint = turret.position + turret.forward * 10f;
    }

    public void SetTargetPoint(Vector3 targetPoint)
    {
        _targetPoint = targetPoint;
    }

    public void StartRotationLoop()
    {
        if (_isRunning) return;
        _isRunning = true;
        RotationLoop().Forget();
    }

    public void StopRotationLoop()
    {
        _isRunning = false;
    }

    private async UniTaskVoid RotationLoop()
    {
        while (_isRunning)
        {
            var flatTarget = new Vector3(_targetPoint.x, _turret.position.y, _targetPoint.z);
            var direction = (flatTarget - _turret.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                _turret.rotation = Quaternion.RotateTowards(
                    _turret.rotation,
                    targetRotation,
                    _settings.rotationSpeed * Time.deltaTime
                );
            }

            await UniTask.Yield();
        }
    }

    public bool IsAimed()
    {
        Vector3 flatTarget = new Vector3(_targetPoint.x, _turret.position.y, _targetPoint.z);
        Vector3 direction = (flatTarget - _turret.position).normalized;
        if (direction == Vector3.zero) return false;
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        float angle = Quaternion.Angle(_turret.rotation, targetRotation);
        return angle < _settings.toleranceDegrees;
    }
}