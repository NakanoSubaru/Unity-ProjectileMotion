using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileUtil
{
    public static Vector3[] MakeTrajectory(float elevation, float initialVelocity, float gravity)
    {
        float verticalInitialSpeed = Mathf.Sin(Mathf.Deg2Rad * elevation) * initialVelocity;
        float horizontalInitialSpeed = Mathf.Cos(Mathf.Deg2Rad * elevation) * initialVelocity;
        
        const int PositionCount = 40;
        Vector3[] positions = new Vector3[PositionCount];
        for(int i = 0; i < PositionCount; ++i)
        {
            float t = i * 0.1f;
            float positionX = horizontalInitialSpeed * t;
            float positionY = (verticalInitialSpeed * t) + (0.5f * gravity * t * t);
            positions[i] = new Vector3(positionX, positionY, 0f);
        }
        return positions;
    }

    public static Vector3[] MakeTrajectoryToTarget(float elevation, float initialVelocity, Vector3 target, float gravity)
    {
        float radian = Mathf.Deg2Rad * elevation;
        const int PositionCount = 40;
        Vector3[] positions = new Vector3[PositionCount];
        float intervalX = target.x / (PositionCount - 1);
        for(int i = 0; i < PositionCount; ++i)
        {
            float positionX = i * intervalX;
            float positionY = positionX * Mathf.Tan(radian)
                + ((0.5f * gravity * positionX * positionX) / (initialVelocity * initialVelocity * Mathf.Cos(radian) * Mathf.Cos(radian)));
            positions[i] = new Vector3(positionX, positionY, 0f);
        }
        return positions;
    }
}
