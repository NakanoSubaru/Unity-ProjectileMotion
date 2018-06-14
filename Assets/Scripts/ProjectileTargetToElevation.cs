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
            if(CalculateElevation())
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

    private bool CalculateElevation()
    {
        float a = (0.5f * Physics.gravity.y * _target.transform.position.x * _target.transform.position.x) / (_initialVelocity * _initialVelocity);
        float b = _target.transform.position.x;
        float c = a - _target.transform.position.y + transform.position.y;

        float discriminant = b * b - 4 * a * c;
        if(discriminant > 0)
        {
            _elevation = Mathf.Atan((-b - Mathf.Sqrt(discriminant)) / (2f * a)) * Mathf.Rad2Deg;
            return true;
        }
        else if(discriminant == 0)
        {
            _elevation = Mathf.Atan(-b / (2f * a)) * Mathf.Rad2Deg;
            return true;
        }

        return false;
    }

    public void Throw()
    {
        float verticalInitialSpeed = Mathf.Sin(Mathf.Deg2Rad * _elevation) * _initialVelocity;
        float horizontalInitialSpeed = Mathf.Cos(Mathf.Deg2Rad * _elevation) * _initialVelocity;
        var rigid = GetComponent<Rigidbody>();
        rigid.velocity = new Vector3(horizontalInitialSpeed, verticalInitialSpeed, 0f);
        rigid.useGravity = true;
        _flying = true;
    }

    public void OnVelocityChanged(float value)
    {
        _initialVelocity = Mathf.Lerp(VelocityMin, VelocityMax, value);
    }
}
