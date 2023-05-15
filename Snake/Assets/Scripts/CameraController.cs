using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public interface ICameraController
    {
        void Rotate(Vector3 axis, float angle, float duration); // Rotate camera from other classes - possible
    }

    public class CameraController : MonoBehaviour, ICameraController
    {
        private float _duration;
        private float targetRotate = 0f;
        private Vector3 _axis;
        public Camera currentCamera;
        private Quaternion targetRotation = Quaternion.identity;
        private Quaternion sourceRotation = Quaternion.identity;
        private float rotationProgress;

        public void Rotate(Vector3 axis, float angle, float duration)
        {
            targetRotation = currentCamera.transform.localRotation * Quaternion.Euler(Vector3.forward * -90f);
            sourceRotation = currentCamera.transform.localRotation;
            rotationProgress = 0f;
            
            targetRotate = angle;
            _duration = duration;
            _axis = axis;
        }

        void Update()
        {
            float rotate = Mathf.Lerp(currentCamera.transform.localEulerAngles.y, targetRotate, 0.005f * Time.deltaTime);

            if (Quaternion.Angle(targetRotation, currentCamera.transform.localRotation) > 0.1f)
            {
                var currentRotation = currentCamera.transform.localRotation;
                currentRotation = Quaternion.Lerp(sourceRotation, targetRotation, rotationProgress);
                rotationProgress += 1f / _duration;
                currentCamera.transform.rotation = currentRotation;
            }
        }
    }
}