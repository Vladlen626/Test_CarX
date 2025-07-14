using Cysharp.Threading.Tasks;
using UnityEngine;

public class HomingMovement : IProjectileMovement
{
    private readonly ITarget _target;

    public HomingMovement(ITarget target)
    {
        _target = target;
    }

    public async UniTask MoveAsync(Projectile projectileController)
    {
        while (projectileController && projectileController.gameObject.activeSelf && _target != null)
        {
            var direction = (_target.Position - projectileController.transform.position).normalized;
            projectileController.transform.forward = direction;
            projectileController.transform.position += direction * (projectileController.m_speed * Time.deltaTime);
            await UniTask.Yield();
        }
    }
}