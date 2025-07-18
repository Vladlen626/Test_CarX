﻿using System;
using UnityEngine;
using UnityEngine.Serialization;

public class ProjectileVisual : MonoBehaviour
{
    [SerializeField] private GameObject m_straightVisual;
    [SerializeField] private GameObject m_homingVisual;
    [SerializeField] private GameObject m_ballisticVisual;

    public void SetVisual(ProjectileType type)
    {
        m_straightVisual.SetActive(type == ProjectileType.Straight);
        m_homingVisual.SetActive(type == ProjectileType.Homing);
        m_ballisticVisual.SetActive(type == ProjectileType.Ballistic);
    }
}