using UnityEngine;
using UnityEngine.UI;

namespace AdventureGame
{
    public class OptionsMenu : MonoBehaviour
    {
        public Slider musicSlider;

        void Start()
        {
            if (MusicManager.instance != null)
            {
                musicSlider.value = MusicManager.instance.GetVolume();
            }

            musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }

        public void OnMusicVolumeChanged(float value)
        {
            if (MusicManager.instance != null)
            {
                MusicManager.instance.SetVolume(value);
            }
        }
    }
}
