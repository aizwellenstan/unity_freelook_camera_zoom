using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [Space(10.0f)]
    [Tooltip("Point aimed by the camera")]
    public Transform Target;

    [Tooltip("Maximum distance between the camera and Target")]
	public float Distance = 2;

    [Tooltip("Distance lerp factor")]
    [Range(.0f, 1.0f)]
    public float LerpSpeed = .1f;

    [Space(10.0f)]
    [Tooltip("Collision parameters")]
    public TraceInfo RayTrace = new TraceInfo { Thickness = .2f };

    [Tooltip("Camera pitch limitations")]
    public LimitsInfo PitchLimits = new LimitsInfo { Minimum = -60.0f, Maximum = 60.0f };

    [Tooltip("Input axes used to control the camera")]
    public InputInfo InputAxes = new InputInfo
    {
        Horizontal = new InputAxisInfo { Name = "Mouse X", Sensitivity = 5.0f },
        Vertical = new InputAxisInfo { Name = "Mouse Y", Sensitivity = 5.0f }
    };

    #region Structs

    [System.Serializable]
    public struct LimitsInfo
    {
        [Tooltip("Minimum pitch angle, in the range [-90, Maximum]")]
        public float Minimum;

        [Tooltip("Maximum pitch angle, in the range [Minimum, 90]")]
        public float Maximum;
    }

    [System.Serializable]
    public struct TraceInfo
    {
        [Tooltip("Ray thickness")]
        public float Thickness;

        [Tooltip("Layers the camera collide with")]
        public LayerMask CollisionMask;
    }

    [System.Serializable]
    public struct InputInfo
    {
        [Tooltip("Horizontal axis")]
        public InputAxisInfo Horizontal;

        [Tooltip("Vertical axis")]
        public InputAxisInfo Vertical;
    }

    [System.Serializable]
    public struct InputAxisInfo
    {
        [Tooltip("Input axis name")]
        public string Name;

        [Tooltip("Axis sensitivity")]
        public float Sensitivity;
    }

    #endregion

    private float _pitch;
    private float _distance;
    public Camera cam;

    public void Start()
    {
        _pitch = Mathf.DeltaAngle(0, -transform.localEulerAngles.x);
        _distance = Distance;
    }

	public void Update()
	{
		float yaw = transform.localEulerAngles.y + Input.GetAxis(InputAxes.Horizontal.Name) * InputAxes.Horizontal.Sensitivity;
		
		_pitch += Input.GetAxis(InputAxes.Vertical.Name) * InputAxes.Vertical.Sensitivity;
		_pitch = Mathf.Clamp(_pitch, PitchLimits.Minimum, PitchLimits.Maximum);		
		
		transform.localEulerAngles = new Vector3(-_pitch, yaw, 0);

        // Zoom
         // -------------------Code for Zooming Out------------
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (cam.fieldOfView<=125)
                    cam.fieldOfView +=2;
    
            }
        // ---------------Code for Zooming In------------------------
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (cam.fieldOfView>2)
                    cam.fieldOfView -=2;
            }
        
        // -------Code to switch camera between Perspective and Orthographic--------
        if (Input.GetKeyUp(KeyCode.B ))
        {
            if (cam.orthographic==true)
                cam.orthographic=false;
            else
                cam.orthographic=true;
        }

        if (Input.GetKey ("escape")) {
            Application.Quit();
        }
	}
}
