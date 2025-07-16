using Cysharp.Threading.Tasks;
using UnityEngine;

public class BallisticMovement : IProjectileMovement
{
    private readonly Vector3 m_shootPos;
    private readonly ITarget m_target;

    public BallisticMovement(Vector3 shootPos, ITarget target)
    {
        m_shootPos = shootPos;
        m_target = target;
    }

    public async UniTask MoveAsync(Projectile projectileController)
    {
        var leadPoint = CoreHelper.GetLeadPoint(
            m_shootPos, m_target, projectileController.m_speed, maxRange: 100f);
        
        var velocity = CoreHelper.CalculateBallisticVelocity(
            m_shootPos, leadPoint, projectileController.m_speed, Physics.gravity.y);

        projectileController.SetPhysicsEnabled(true, velocity);
        while (projectileController && projectileController.gameObject.activeSelf)
            await UniTask.Yield();
    }
    
   
}