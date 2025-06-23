using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;
using UnityEngine.SceneManagement;

public class InCameraDetector : MonoBehaviour
{
    public bool isOutOfCameraView;
    public static InCameraDetector instance;
    Camera _camera;
    MeshRenderer _renderer;
    Plane[] _cameraFrustum;
    Collider _collider;
    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        _camera = Camera.main;
        _renderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        var _bounds = _collider.bounds;
        _cameraFrustum = GeometryUtility.CalculateFrustumPlanes(_camera);

        if (GeometryUtility.TestPlanesAABB(_cameraFrustum, _bounds))
        {
            isOutOfCameraView = false;
        }
        else
        {
            isOutOfCameraView = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if(isOutOfCameraView)
            {
                GameObject.Find("Main Camera").transform.localPosition = new Vector3(0f, 0f, -10f);
                GameObject.Find("Main Camera").transform.parent.GetComponent<LeanPitchYaw>().Pitch = 0;
                GameObject.Find("Main Camera").transform.parent.GetComponent<LeanPitchYaw>().Yaw = 0;
                GameObject.Find("FaceParent").transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
