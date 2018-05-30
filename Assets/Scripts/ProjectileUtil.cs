using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileUtil
{
    public static void MakeTrajectory(LineRenderer renderer, float elevation, float initialVelocity, Vector3 initialPosition, float gravity)
    {
        float verticalInitialSpeed = Mathf.Sin(Mathf.Deg2Rad * elevation) * initialVelocity;
        float horizontalInitialSpeed = Mathf.Cos(Mathf.Deg2Rad * elevation) * initialVelocity;

        Vector3 InitialPosition = Vector3.zero;
        const int PositionCount = 40;
        Vector3[] positions = new Vector3[PositionCount];
        for(int i = 0; i < PositionCount; ++i)
        {
            float t = i * 0.1f;
            float positionX = horizontalInitialSpeed * t;
            float positionY = (verticalInitialSpeed * t) + (0.5f * gravity * t * t) + initialPosition.y;
            positions[i] = InitialPosition +
                new Vector3(positionX, positionY, 0f);
        }
        renderer.SetPositions(positions);
        renderer.positionCount = PositionCount;
    }

    public static void MakeTrajectoryToTarget(LineRenderer renderer, float elevation, float initialVelocity, Vector3 initialPosition, Vector3 target, float gravity)
    {
        float radian = Mathf.Deg2Rad * elevation;
        Vector3 InitialPosition = Vector3.zero;
        const int PositionCount = 40;
        Vector3[] positions = new Vector3[PositionCount];
        float intervalX = (target.x - initialPosition.x) / (PositionCount - 1);
        for(int i = 0; i < PositionCount; ++i)
        {
            float positionX = i * intervalX;
            float positionY = positionX * Mathf.Tan(radian)
                + ((0.5f * gravity * positionX * positionX) / (initialVelocity * initialVelocity * Mathf.Cos(radian) * Mathf.Cos(radian))) + initialPosition.y;
            positions[i] = InitialPosition +
                new Vector3(positionX, positionY, 0f);
        }
        renderer.SetPositions(positions);
        renderer.positionCount = PositionCount;
    }
}
