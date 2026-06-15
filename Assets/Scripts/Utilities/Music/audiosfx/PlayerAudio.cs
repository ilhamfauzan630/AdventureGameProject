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

        private void Start()
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
            if (audioSource.isPlaying &&
                audioSource.clip == runClip &&
                audioSource.pitch == 1f)
            {
                return;
            }

            audioSource.clip = runClip;
            audioSource.loop = true;
            audioSource.pitch = 1f;

            if (!audioSource.isPlaying)
                audioSource.Play();
        }

        public void StopRun()
        {
            audioSource.Stop();
        }

        public void StartRunFast()
        {
            if (audioSource.isPlaying &&
                audioSource.clip == runFastClip)
            {
                return;
            }

            audioSource.clip = runFastClip;
            audioSource.loop = true;
            audioSource.pitch = 1.4f;

            if (!audioSource.isPlaying)
                audioSource.Play();
        }

        public void StopRunFast()
        {
            audioSource.Stop();
            audioSource.pitch = 1f;
        }
    }
}