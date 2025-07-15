using UnityEngine;

public static class CoreHelper
{
    public static Vector3 GetLeadPoint(Vector3 shooterPos, ITarget target, float projectileSpeed, float maxRange, int maxIterations = 10, float epsilon = 0.01f,  float leadFactor = 0.9f)
    {
        var leadPoint = target.m_position;

        for (var i = 0; i < maxIterations; i++)
        {
            var toLead = leadPoint - shooterPos;
            var distance = Mathf.Min(toLead.magnitude, maxRange);
            var timeToReach = distance * leadFactor / projectileSpeed;
            var predictedTargetPos = target.m_position + target.m_velocity * timeToReach;
            
            if ((leadPoint - predictedTargetPos).sqrMagnitude < epsilon * epsilon)
                return predictedTargetPos;

            leadPoint = predictedTargetPos;
        }

        return leadPoint;
    }
}