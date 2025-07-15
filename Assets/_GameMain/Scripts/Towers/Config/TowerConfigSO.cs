using UnityEngine;

[CreateAssetMenu(fileName = "TowerConfigSO", menuName = "Configs/TowerConfigSO")]
public class TowerConfigSO : ScriptableObject
{
    public ProjectileType m_projectileType;
    public TowerView m_towerViewPrefab;
    public float m_shootInterval = 0.5f;
    public float m_range = 6f;
    public float m_projectileSpeed = 10f;
    public int m_damage = 10;
    public bool m_enableLeadAim;
}

