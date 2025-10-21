using UnityEngine;

public class ArrowDestroy : MonoBehaviour
{
    public float lifeTime = 5f;
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
