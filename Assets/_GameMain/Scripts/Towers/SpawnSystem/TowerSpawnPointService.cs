using System.Collections.Generic;
using UnityEngine;

public class TowerSpawnPointService : MonoBehaviour
{
    [SerializeField] private List<TowerSpawnPoint> m_points;
    public IReadOnlyList<TowerSpawnPoint> m_Points => m_points;
}