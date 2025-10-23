using UnityEngine;

public class BillboardUI : MonoBehaviour  // ganti nama
{
    void LateUpdate()
    {
        if (Camera.main != null)
            transform.LookAt(Camera.main.transform);
    }
}
