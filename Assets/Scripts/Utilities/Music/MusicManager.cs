using UnityEngine;

namespace AdventureGame
{
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager instance;
        private AudioSource audioSource;

        void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();

            // load volume tersimpan
            audioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        }

        public void PlayMusic(AudioClip newClip)
        {
            if (audioSource.clip == newClip) return;

            audioSource.clip = newClip;
            audioSource.loop = true;
            audioSource.Play();
        }

        public void SetVolume(float volume)
        {
            audioSource.volume = volume;
            PlayerPrefs.SetFloat("MusicVolume", volume);
        }

        public float GetVolume()
        {
            return audioSource.volume;
        }
    }
}
