using System.Collections;
using UnityEngine;

internal class CameraBehaviour : MonoBehaviour, IUpdate
{
    private CameraModel _model;
    private Camera camera;

    private float deltaAngle;
    private float angle;

    private float height;
    private float minHeight;
    private float maxHeight;
    private float heightDuration;

    private Coroutine rout_Fov;

    internal void Init(in CameraModel setModel)
    {
        _model = setModel;
        camera = Camera.main;

        deltaAngle = 360f / _model.roundDuration;

        height = _model.height;
        minHeight = height - _model.roamingRadius;
        maxHeight = height + _model.roamingRadius;
        heightDuration = _model.roamingDuration;
        
        GlobalUpdate.AddToUpdate(this);
        
        if (rout_Fov != null)
            StopCoroutine(rout_Fov);
        rout_Fov = StartCoroutine(FovLerp());
    }

    public void GUpdate()
    {
        height = Mathf.Lerp(minHeight, maxHeight,
            Mathf.PingPong(Time.time / heightDuration, 1f));
        
        angle += deltaAngle * Time.deltaTime;
        Quaternion camRot = Quaternion.Euler(0f, angle, 0f);
        Vector3 upPos = new Vector3(0f, height, 0f);
        transform.localPosition = upPos + camRot * Vector3.forward * _model.roundRadius;

        Vector3 lookPos = transform.parent.position + new Vector3(0f, _model.lookAtHeight, 0f);
        transform.LookAt(lookPos, Vector3.up);
    }

    private IEnumerator FovLerp()
    {
        float duration = _model.fovDuration;
        float startFov = camera.fieldOfView;
        float targetFov = Random.Range(_model.fovMin, _model.fovMax);
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float setFov = Mathf.Lerp(startFov, targetFov, timer / duration);
            camera.fieldOfView = setFov;
            yield return null;
        }

        yield return new WaitForSeconds(_model.fovDelay);
        
        if (rout_Fov != null)
            StopCoroutine(rout_Fov);
        rout_Fov = StartCoroutine(FovLerp());
    }
}
