using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfigSO", menuName = "Configs/EnemyConfigSO")]
public class EnemyConfigSO : ScriptableObject
{
    public EnemyType m_type;
    public EnemyView m_enemyViewPrefab;
    public float m_speed = 5f;
    public int m_maxHP = 30;
    public float m_reachDistance = 0.3f;
}