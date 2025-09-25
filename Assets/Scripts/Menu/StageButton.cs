using UnityEngine;

namespace AdventureGame
{
    [RequireComponent(typeof(UnityEngine.UI.Button))]
    public class StageButton : MonoBehaviour
    {
        public int stageIndex;
        public StageSelectManager stageSelectManager; // set via inspector

        // hubungkan method ini ke Button.onClick (drag gameObject sendiri -> StageButton.OnClick)
        public void OnClick()
        {
            if (stageSelectManager != null)
                stageSelectManager.ShowStageInfo(stageIndex);
            else
                Debug.LogWarning("StageSelectUI reference not set on StageButton.");
        }
    }
}
