﻿using UnityEngine;

/// <summary>
/// Generic Side-by-Side 3D.
/// </summary>
public class SBS : MonoBehaviour {
    /// <summary>
    /// Extend field of View upwards for the smaller viewports.
    /// </summary>
    static float OldFov, OldSize;
    static Camera OtherEye;
    static GameObject Holder;

    public static bool Enabled {
        get { return Holder; }
        set {
            if (value == OtherEye)
                return;
            if (!value) {
                Destroy(OtherEye.gameObject);
                Camera.main.rect = new Rect(0, 0, 1, 1);
                Camera.main.fieldOfView = OldFov;
                Camera.main.orthographicSize = OldSize;
                Destroy(Holder);
            } else {
                OtherEye = new GameObject().AddComponent<Camera>();
                OtherEye.gameObject.transform.parent = Camera.main.gameObject.transform;
                OtherEye.transform.localPosition = new Vector3(-.075f, 0, 0);
                OtherEye.transform.localEulerAngles = new Vector3(0, 2, 0);
                OtherEye.clearFlags = Camera.main.clearFlags;
                OtherEye.backgroundColor = Camera.main.backgroundColor;
                OtherEye.rect = new Rect(0, 0, .5f, 1);
                OldFov = Camera.main.fieldOfView;
                float FovExtension = Screen.width / (float)Screen.height;
                OtherEye.fieldOfView = Camera.main.fieldOfView = Camera.main.fieldOfView * FovExtension;
                OtherEye.orthographic = Camera.main.orthographic;
                OldSize = Camera.main.orthographicSize;
                OtherEye.orthographicSize = Camera.main.orthographicSize = Camera.main.orthographicSize * FovExtension;
                Camera.main.rect = new Rect(.5f, 0, .5f, 1);
                (Holder = new GameObject()).AddComponent<SBS>();
                DontDestroyOnLoad(Holder);
            }
        }
    }

    void Update() {
        // Re-enable through scenes
        if (!OtherEye)
            Enabled = true;
        // S key: flip left/right eye
        if (Input.GetKeyDown(KeyCode.S)) {
            Rect Temp = Camera.main.rect;
            Camera.main.rect = OtherEye.rect;
            OtherEye.rect = Temp;
        }
    }
}