using UnityEngine;

namespace AdventureGame
{
    public class CollectEffectSpawner : MonoBehaviour
    {
        public static void Spawn(GameObject effectPrefab, Vector3 position)
        {
            if (effectPrefab == null)
                return;

            GameObject effect =
                Instantiate(effectPrefab, position, Quaternion.identity);

            Destroy(effect, 2f);
        }
    }
}