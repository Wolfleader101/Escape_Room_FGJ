using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour {

    [SerializeField] private Camera cam;

    void Update() {
        transform.eulerAngles = Quaternion.LookRotation(cam.transform.forward, cam.transform.up).eulerAngles;
    }
}
