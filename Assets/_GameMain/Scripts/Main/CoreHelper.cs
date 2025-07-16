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
    
    public static Vector3 CalculateBallisticVelocity(
        Vector3 start, Vector3 end, float speed, float gravity)
    {
        var diff = end - start;
        var diffXZ = new Vector3(diff.x, 0, diff.z);
        var dist = diffXZ.magnitude;
        var yOffset = diff.y;

        var speed2 = speed * speed;
        var g = -gravity;
        var inside = speed2 * speed2 - g * (g * dist * dist + 2 * yOffset * speed2);

        if (inside < 0.01f)
            return diff.normalized * speed;

        var sqrt = Mathf.Sqrt(inside);
        var lowAngle = Mathf.Atan2(speed2 - sqrt, g * dist);

        var velocity = diffXZ.normalized * (speed * Mathf.Cos(lowAngle));
        velocity.y = speed * Mathf.Sin(lowAngle);
        return velocity;
    }
}