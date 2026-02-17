using UnityEngine;

namespace AdventureGame
{
    public class PlayerAudio : MonoBehaviour
    {
        public AudioSource audioSource;

        [Header("SFX")]
        public AudioClip shootClip;
        public AudioClip runClip;
        public AudioClip runFastClip;
        public AudioClip explodeClip;

        private bool isRunning = false;

        void Start()
        {
            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();
        }

        public void PlayShoot()
        {
            audioSource.PlayOneShot(shootClip);
        }

        public void PlayExplode()
        {
            audioSource.PlayOneShot(explodeClip);
        }

        public void StartRun()
        {
            if (isRunning) return;

            audioSource.clip = runClip;
            audioSource.loop = true;
            audioSource.Play();
            isRunning = true;
        }

        public void StopRun()
        {
            if (!isRunning) return;

            audioSource.Stop();
            isRunning = false;
        }

        public void StartRunFast()
        {
            if (isRunning) return;

            audioSource.clip = runFastClip;
            audioSource.loop = true;
            audioSource.Play();
            isRunning = true;
        }

        public void StopRunFast()
        {
            if (!isRunning) return;

            audioSource.Stop();
            isRunning = false;
        }
    }
}
