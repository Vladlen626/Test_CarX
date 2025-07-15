using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float m_interval = 3f;

    private Transform m_moveTarget;
    private Enemy.Pool m_enemyPool;
    private bool m_isSpawning;

    [Inject]
    public void Construct(Enemy.Pool enemyPool, [Inject(Id = "EnemiesGoal")] Transform enemiesGoal)
    {
        m_enemyPool = enemyPool;
        m_moveTarget = enemiesGoal;
    }

    public void StartSpawn()
    {
        if (m_isSpawning) return;
        m_isSpawning = true;
        SpawnLoop().Forget();
    }
    
    public void SetMoveTarget(Transform moveTarget)
    {
        m_moveTarget = moveTarget;
    }

    public void StopSpawn()
    {
        m_isSpawning = false;
    }

    private async UniTaskVoid SpawnLoop()
    {
        while (m_isSpawning)
        {
            if (m_moveTarget)
            {
                var enemy = m_enemyPool.Spawn(m_moveTarget);
                enemy.transform.position = transform.position;
            }
            await UniTask.Delay(System.TimeSpan.FromSeconds(m_interval));
        }
    }
}