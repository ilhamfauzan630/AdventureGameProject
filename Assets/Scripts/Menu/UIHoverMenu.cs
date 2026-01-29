using UnityEngine;
using UnityEngine.EventSystems;

namespace AdventureGame
{
    public class UIHoverMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public RectTransform hoverCorner;

        public void OnPointerEnter(PointerEventData eventData)
        {
            hoverCorner.gameObject.SetActive(true);
            hoverCorner.position = transform.position;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            hoverCorner.gameObject.SetActive(false);
        }

        void OnDisable()
        {
            if (hoverCorner != null)
                hoverCorner.gameObject.SetActive(false);
        }
    }
}
