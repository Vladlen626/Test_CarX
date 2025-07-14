using System;
using UnityEngine;
public class ProjectileVisual : MonoBehaviour
{
    [SerializeField] private GameObject straightVisual;
    [SerializeField] private GameObject homingVisual;

    public void SetVisual(ProjectileType type)
    {
        straightVisual.SetActive(type == ProjectileType.Straight);
        homingVisual.SetActive(type == ProjectileType.Homing);
    }
}