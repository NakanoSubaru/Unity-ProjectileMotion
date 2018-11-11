using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileUtil
{
    public static bool CalculateElevation(ref float elevation, float initialVelocity, Transform target)
    {
        float a = (0.5f * Physics.gravity.y * target.transform.position.x * target.transform.position.x) / (initialVelocity * initialVelocity);
        float b = target.transform.position.x;
        float c = a - target.transform.position.y;

        float discriminant = b * b - 4 * a * c;
        if(discriminant > 0)
        {
            elevation = Mathf.Atan((-b - Mathf.Sqrt(discriminant)) / (2f * a)) * Mathf.Rad2Deg;
            return true;
        }
        else if(discriminant == 0)
        {
            elevation = Mathf.Atan(-b / (2f * a)) * Mathf.Rad2Deg;
            return true;
        }

        return false;
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

    public static Vector3[] MakeTrajectory(float elevation, float initialVelocity, float gravity)
    {
        const int MaxTime = 4;
        const int PositionCount = 40;
        const float TimeResolution = (float)MaxTime / PositionCount;
        Vector3[] positions = new Vector3[PositionCount];

        float horizontalInitialSpeed = Mathf.Cos(Mathf.Deg2Rad * elevation) * initialVelocity;
        float verticalInitialSpeed = Mathf.Sin(Mathf.Deg2Rad * elevation) * initialVelocity;
        for(int i = 0; i < PositionCount; ++i)
        {
            float t = i * TimeResolution;
            float positionX = horizontalInitialSpeed * t;
            float positionY = (verticalInitialSpeed * t) + (0.5f * gravity * t * t);
            positions[i] = new Vector3(positionX, positionY, 0f);
        }
        return positions;
    }

}
