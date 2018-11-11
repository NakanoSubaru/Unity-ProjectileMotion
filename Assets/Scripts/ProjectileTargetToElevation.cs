using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileTargetToElevation : MonoBehaviour {
    const float VelocityMin = 5f;
    const float VelocityMax = 25f;

    [SerializeField]
    private float _elevation = 30.0f;

    [SerializeField]
    private Transform _target = null;

    [SerializeField]
    private Text _velocityDisplay = null;

    [SerializeField]
    private Slider _velocitySlider = null;

    [SerializeField]
    private Text _elevationDisplay = null;

    private float _initialVelocity;
    private LineRenderer _trajectoryRenderer;
    private bool _flying = false;

    private void Awake()
    {
        _initialVelocity = Mathf.Lerp(VelocityMin, VelocityMax, _velocitySlider.value);
        _trajectoryRenderer = gameObject.AddComponent<LineRenderer>();
        _trajectoryRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        _trajectoryRenderer.receiveShadows = false;
        _trajectoryRenderer.allowOcclusionWhenDynamic = false;
        _trajectoryRenderer.startWidth = _trajectoryRenderer.endWidth = 0.1f;
    }

    private void Update()
    {
        _velocityDisplay.text = string.Format("Velocity: {0:f1}", _initialVelocity);
        _elevationDisplay.text = string.Format("Angle: {0:f1}", _elevation);

        if(!_flying)
        {
            if(ProjectileUtil.CalculateElevation(ref _elevation, _initialVelocity, _target))
            {
                var positions = ProjectileUtil.MakeTrajectoryToTarget(_elevation, _initialVelocity, _target.position, Physics.gravity.y);
                _trajectoryRenderer.positionCount = positions.Length;
                _trajectoryRenderer.SetPositions(positions);
            }
            else
            {
                _trajectoryRenderer.positionCount = 0;
            }
        }
    }

    public void Throw()
    {
        float verticalInitialSpeed = Mathf.Sin(Mathf.Deg2Rad * _elevation) * _initialVelocity;
        float horizontalInitialSpeed = Mathf.Cos(Mathf.Deg2Rad * _elevation) * _initialVelocity;
        var rigid = GetComponent<Rigidbody>();
        rigid.velocity = new Vector3(horizontalInitialSpeed, verticalInitialSpeed, 0f);
        rigid.useGravity = true;
        rigid.drag = 0f;
        _flying = true;
    }

    public void OnVelocityChanged(float value)
    {
        _initialVelocity = Mathf.Lerp(VelocityMin, VelocityMax, value);
    }
}
