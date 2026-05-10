using UnityEngine;
using UnityEngine.UI;

namespace AdventureGame
{
    public class TutorialPopup : MonoBehaviour
    {
        [Header("Pages")]
        public GameObject[] pages;

        [Header("Buttons")]
        public Button nextButton;
        public Button prevButton;
        public Button closeButton;

        private int currentPage = 0;

        private void Start()
        {
            // pause game
            Time.timeScale = 0f;

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

            // nyalakan page aktif
            pages[index].SetActive(true);

            // tombol prev
            prevButton.gameObject.SetActive(index > 0);

            // tombol next
            nextButton.gameObject.SetActive(index < pages.Length - 1);

            // tombol close hanya di page terakhir
            closeButton.gameObject.SetActive(index == pages.Length - 1);
        }

        public void ClosePopup()
        {
            // tutup popup
            gameObject.SetActive(false);

            // lanjut game
            Time.timeScale = 1f;

            // lock cursor kembali
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}