using Cysharp.Threading.Tasks;
using UnityEngine;

public class StraightMovement : IProjectileMovement
{
    public async UniTask MoveAsync(Projectile projectileController)
    {
        while (projectileController && projectileController.gameObject.activeSelf)
        {
            projectileController.transform.position += projectileController.transform.forward * (projectileController.m_speed * Time.deltaTime);
            await UniTask.Yield();
        }
    }
}