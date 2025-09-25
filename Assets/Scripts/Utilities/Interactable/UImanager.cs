using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdventureGame
{
    public class UImanager : MonoBehaviour
    {
        public static UImanager Instance;

        [SerializeField] private Text pressEText;
        [SerializeField] private Text infoText;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void ShowPressE(string msg)
        {
            if (pressEText != null)
            {
                pressEText.text = msg;
                pressEText.gameObject.SetActive(true);
            }
        }

        public void HidePressE()
        {
            if (pressEText != null) pressEText.gameObject.SetActive(false);
        }

        public void ShowInfo(string msg)
        {
            if (infoText != null)
            {
                infoText.text = msg;
                infoText.gameObject.SetActive(true);
            }
        }
    }
}
