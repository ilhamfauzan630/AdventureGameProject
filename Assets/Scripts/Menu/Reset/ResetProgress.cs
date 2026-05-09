using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdventureGame
{
    public class ResetProgress : MonoBehaviour
    {
        public void ResetGameProgress()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();

            Debug.Log("Progress berhasil direset!");

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
