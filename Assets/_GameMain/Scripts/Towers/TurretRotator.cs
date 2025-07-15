using Cysharp.Threading.Tasks;
using UnityEngine;

public class TurretRotator
{
    private readonly Transform m_turret;
    private readonly TurretRotationSettings m_settings;
    private Vector3 m_targetPoint;
    private bool m_isRunning;

    public TurretRotator(Transform mTurret, TurretRotationSettings mSettings)
    {
        m_turret = mTurret;
        m_settings = mSettings;
        m_targetPoint = mTurret.position + mTurret.forward * 10f;
    }

    public void SetTargetPoint(Vector3 targetPoint)
    {
        m_targetPoint = targetPoint;
    }

    public void StartRotationLoop()
    {
        if (m_isRunning) return;
        m_isRunning = true;
        RotationLoop().Forget();
    }

    public void StopRotationLoop()
    {
        m_isRunning = false;
    }

    private async UniTaskVoid RotationLoop()
    {
        while (m_isRunning)
        {
            var flatTarget = new Vector3(m_targetPoint.x, m_turret.position.y, m_targetPoint.z);
            var direction = (flatTarget - m_turret.position).normalized;
            if (direction != Vector3.zero)
            {
                var targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                m_turret.rotation = Quaternion.RotateTowards(
                    m_turret.rotation,
                    targetRotation,
                    m_settings.m_rotationSpeed * Time.deltaTime
                );
            }

            await UniTask.Yield();
        }
    }
}