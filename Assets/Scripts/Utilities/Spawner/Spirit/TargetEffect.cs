using UnityEngine;

namespace AdventureGame
{
    public class TargetEffect : MonoBehaviour
    {
        [SerializeField]
        private GameObject destroyEffectPrefab;

        public void PlayDestroyEffect()
        {
            if (destroyEffectPrefab == null)
                return;

            GameObject effect = Instantiate(
                destroyEffectPrefab,
                transform.position,
                Quaternion.identity
            );

            Destroy(effect, 2f);
        }
    }
}