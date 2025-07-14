using Cysharp.Threading.Tasks;

public interface IProjectileMovement
{
    UniTask MoveAsync(Projectile projectileController);
}