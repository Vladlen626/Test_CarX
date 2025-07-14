using UnityEngine;

public static class LeadCalculator
{
    public static Vector3 GetLeadPoint(Vector3 shooterPos, ITarget target, float projectileSpeed, float leadFactor = 0.9f)
    {
        var toTarget = target.Position - shooterPos;
        var distance = toTarget.magnitude;
        var timeToReach = (distance * leadFactor) / projectileSpeed;
        return target.Position + target.Velocity * timeToReach;
    }
}