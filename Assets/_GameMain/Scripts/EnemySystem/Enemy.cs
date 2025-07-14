using UnityEngine;
using Zenject;

public class Enemy : MonoBehaviour, ITarget
{
    public Vector3 Position => transform.position;
    public Vector3 Velocity { get; private set; }

    public bool IsAlive => _currentHp > 0;

    [SerializeField] private float m_speed = 5f;
    [SerializeField] private int m_maxHP = 30;
    [SerializeField] private float m_reachDistance = 0.3f;

    private int _currentHp;
    private Transform _moveTarget;
    private ITargetRegistry _targetRegistry;
    private Vector3 _lastPosition;

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
        _targetRegistry = targetRegistry;
    }

    public void Init(Transform moveTarget)
    {
        _moveTarget = moveTarget;
        _currentHp = m_maxHP;
        _targetRegistry.Register(this);
        _lastPosition = transform.position;
        Velocity = Vector3.zero;
    }

    public void TakeDamage(int amount)
    {
        _currentHp -= amount;
        if (_currentHp <= 0)
        {
            Die();
        }
    }
    
    private void Update()
    {
        if (!_moveTarget || !IsAlive) return;

        if (Vector3.Distance(transform.position, _moveTarget.position) <= m_reachDistance)
        {
            gameObject.SetActive(false);
            return;
        }
        
        var dir = (_moveTarget.position - transform.position).normalized;
        transform.position += dir * (m_speed * Time.deltaTime);
        
        Velocity = (transform.position - _lastPosition) / Time.deltaTime;
        _lastPosition = transform.position;
    }

    private void Die()
    {
        _targetRegistry.Unregister(this);
        gameObject.SetActive(false);
    }
    
    private void OnDestroy()
    {
        _targetRegistry?.Unregister(this);
    }
}