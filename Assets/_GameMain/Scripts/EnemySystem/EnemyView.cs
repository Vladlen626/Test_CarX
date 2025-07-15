using UnityEngine;
using Zenject;

public class EnemyView : MonoBehaviour, ITarget
{
    public Vector3 m_position => transform.position;
    public Vector3 m_velocity => m_model?.Velocity ?? Vector3.zero;
    public bool m_isAlive => m_model?.IsAlive ?? false;

    private EnemyModel m_model;
    private ITargetRegistry m_targetRegistry;

    [Inject]
    public void Construct(ITargetRegistry targetRegistry)
    {
        m_targetRegistry = targetRegistry;
    }

    public void Init(EnemyModel model)
    {
        SafeClearModel();
        m_model = model;
        m_targetRegistry.Register(this);
        m_model.OnDie += OnModelDie;
    }

    private void Update()
    {
        if (m_model == null || !m_model.IsAlive)
            return;

        m_model.Tick(transform.position);
        transform.position = m_model.Position;

        if (m_model.IsReachTarget() && m_model.IsAlive)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        m_model?.TakeDamage(damage);
        if (m_model != null && !m_model.IsAlive)
        {
            Die();
        }
    }

    private void Die()
    {
        SafeClearModel();
        m_targetRegistry.Unregister(this);
        gameObject.SetActive(false);
    }

    private void SafeClearModel()
    {  
        if (m_model == null) 
            return;
        
        m_model.OnDie -= OnModelDie;
        m_model = null;
    }

    private void OnDestroy()
    {
        SafeClearModel();
        m_targetRegistry?.Unregister(this);
    }
    
    private void OnDisable()
    {
        SafeClearModel();
        m_targetRegistry?.Unregister(this);
    }

    private void OnModelDie()
    {
        Die();
    }
}