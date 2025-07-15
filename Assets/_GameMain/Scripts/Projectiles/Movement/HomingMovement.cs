using Cysharp.Threading.Tasks;
using UnityEngine;

public class HomingMovement : IProjectileMovement
{
    private readonly ITarget m_target;
    private const float m_minHitDistance = 0.05f;

    public HomingMovement(ITarget mTarget)
    {
        m_target = mTarget;
    }

    public async UniTask MoveAsync(Projectile projectileController)
    {
        while (projectileController && projectileController.gameObject.activeSelf && m_target != null)
        {
            var distance = Vector3.Distance(projectileController.transform.position, m_target.m_position);
            if (distance < m_minHitDistance)
            {
                projectileController.Deactivate();
                return;
            }
            
            var direction = (m_target.m_position - projectileController.transform.position).normalized;
            projectileController.transform.forward = direction;
            projectileController.transform.position += direction * (projectileController.m_speed * Time.deltaTime);
            await UniTask.Yield();
        }
    }
}