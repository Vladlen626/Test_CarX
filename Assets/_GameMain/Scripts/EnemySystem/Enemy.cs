using UnityEngine;
using Zenject;

public class Enemy : MonoBehaviour, ITarget
{
    public Vector3 m_position => transform.position;
    public Vector3 m_velocity { get; private set; }

    public bool m_isAlive => m_currentHp > 0;

    [SerializeField] private float m_speed = 5f;
    [SerializeField] private int m_maxHP = 30;
    [SerializeField] private float m_reachDistance = 0.3f;

    private int m_currentHp;
    private Transform m_moveTarget;
    private ITargetRegistry m_targetRegistry;
    private Vector3 m_lastPosition;

    public class Pool : MonoMemoryPool<Transform, Enemy>
    {
        protected override void Reinitialize(Transform moveTarget, Enemy enemy)
        {
            enemy.Init(moveTarget);
            enemy.gameObject.SetActive(true);
        }
    }
    
    [Inject]
    public void Construct(ITargetRegistry targetRegistry)
    {
        m_targetRegistry = targetRegistry;
    }

    public void Init(Transform moveTarget)
    {
        m_moveTarget = moveTarget;
        m_currentHp = m_maxHP;
        m_targetRegistry.Register(this);
        m_lastPosition = transform.position;
        m_velocity = Vector3.zero;
    }

    public void TakeDamage(int amount)
    {
        m_currentHp -= amount;
        if (m_currentHp <= 0)
        {
            Die();
        }
    }
    
    private void Update()
    {
        if (!m_moveTarget || !m_isAlive) return;

        if (Vector3.Distance(transform.position, m_moveTarget.position) <= m_reachDistance)
        {
            gameObject.SetActive(false);
            return;
        }
        
        var dir = (m_moveTarget.position - transform.position).normalized;
        transform.position += dir * (m_speed * Time.deltaTime);
        
        m_velocity = (transform.position - m_lastPosition) / Time.deltaTime;
        m_lastPosition = transform.position;
    }

    private void Die()
    {
        m_targetRegistry.Unregister(this);
        gameObject.SetActive(false);
    }
    
    private void OnDestroy()
    {
        m_targetRegistry?.Unregister(this);
    }
}