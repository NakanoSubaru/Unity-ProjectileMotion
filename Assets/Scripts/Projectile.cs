using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour {
    const float Gravity = 9.8f;
    const float AngleMin = 0f;
    const float AngleMax = 90f;
    const float VelocityMin = 5f;
    const float VelocityMax = 25f;

    [SerializeField]
    private Text _velocityDisplay = null;

    [SerializeField]
    private Slider _velocitySlider = null;

    [SerializeField]
    private Text _elevationDisplay = null;

    [SerializeField]
    private Slider _elevationSlider = null;

    private float _initialVelocity;
    private float _elevation;
    private LineRenderer _trajectoryRenderer;
    private bool _flying = false;

    private void Awake()
    {
        _initialVelocity = Mathf.Lerp(VelocityMin, VelocityMax, _velocitySlider.value);
        _elevation = Mathf.Lerp(AngleMin, AngleMax, _elevationSlider.value);

        _trajectoryRenderer = gameObject.AddComponent<LineRenderer>();
        _trajectoryRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        _trajectoryRenderer.receiveShadows = false;
        _trajectoryRenderer.allowOcclusionWhenDynamic = false;
        _trajectoryRenderer.startWidth = _trajectoryRenderer.endWidth = 0.1f;
    }

    private void Update()
    {
        if(!_flying)
        {
            _velocityDisplay.text = string.Format("Velocity: {0:f1}", _initialVelocity);
            _elevationDisplay.text = string.Format("Angle: {0:f1}", _elevation);
            ProjectileUtil.MakeTrajectory(_trajectoryRenderer, _elevation, _initialVelocity, transform.position, Gravity);
        }
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

    public void OnElevationChanged(float value)
    {
        _elevation = Mathf.Lerp(AngleMin, AngleMax, value);
    }
}
