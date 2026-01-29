using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureGame
{
    public class SceneMusic : MonoBehaviour
    {
        public AudioClip sceneMusic;

        void Start()
        {
            if (MusicManager.instance != null)
            {
                MusicManager.instance.PlayMusic(sceneMusic);
            }
        }
    }
}
