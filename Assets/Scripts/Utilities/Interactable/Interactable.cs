using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureGame
{
    public class Interactable : MonoBehaviour
    {
        [Header("UI")]
        public string promptLabel = "Interact";
        [TextArea] public string infoText = "Info";
        public GameObject worldPrompt; // opsional icon "E" di atas benda

        public void SetHighlighted(bool value)
        {
            if (worldPrompt != null) worldPrompt.SetActive(value);
        }

        public virtual void Interact()
        {
            UImanager.Instance?.ShowInfo(infoText);
        }
    }
}
