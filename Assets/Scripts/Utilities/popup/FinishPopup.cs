using UnityEngine;
using UnityEngine.UI;

namespace AdventureGame
{
    public class FinishPopup : MonoBehaviour
    {
        [Header("Pages")]
        public GameObject[] pages;

        [Header("Buttons")]
        public Button nextButton;
        public Button prevButton;
        public Button closeButton;

        private int currentPage = 0;

        private void OnEnable()
        {
            currentPage = 0;

            // tampilkan cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            ShowPage(currentPage);
        }

        public void NextPage()
        {
            if (currentPage < pages.Length - 1)
            {
                currentPage++;
                ShowPage(currentPage);
            }
        }

        public void PrevPage()
        {
            if (currentPage > 0)
            {
                currentPage--;
                ShowPage(currentPage);
            }
        }

        private void ShowPage(int index)
        {
            // matikan semua page
            for (int i = 0; i < pages.Length; i++)
            {
                pages[i].SetActive(false);
            }

            // tampilkan page aktif
            pages[index].SetActive(true);

            // tombol prev
            if (prevButton != null)
                prevButton.gameObject.SetActive(index > 0);

            // tombol next
            if (nextButton != null)
                nextButton.gameObject.SetActive(index < pages.Length - 1);

            // tombol close
            if (closeButton != null)
                closeButton.gameObject.SetActive(index == pages.Length - 1);
        }

        public void ClosePopup()
        {
            // tutup popup
            gameObject.SetActive(false);

            // cursor tetap aktif karena ini menu
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}