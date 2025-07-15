using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float m_interval = 3f;
    [SerializeField] private EnemyType m_type = EnemyType.Basic;

    private IEnemyFactory<EnemyView>  m_enemyFactory;
    private Transform m_goal;
    private bool m_isRunning;

    [Inject]
    public void Construct(IEnemyFactory<EnemyView> enemyFactory, [Inject(Id = "EnemiesGoal")] Transform goal)
    {
        m_enemyFactory = enemyFactory;
        m_goal = goal;
    }

    public void StartSpawn()
    {
        if (m_isRunning) return;
        m_isRunning = true;
        SpawnLoop().Forget();
    }

    public void StopSpawn()
    {
        m_isRunning = false;
    }

    private async UniTaskVoid SpawnLoop()
    {
        while (m_isRunning)
        {
            m_enemyFactory.TryCreate(m_type, transform.position, m_goal.position);
            await UniTask.Delay(System.TimeSpan.FromSeconds(m_interval));
        }
    }
}