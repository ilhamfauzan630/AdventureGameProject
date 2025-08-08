using System;
using UnityEngine;

namespace AdventureGame
{
    [Serializable]
    public class PlayerAirborneData
    {
        [field: SerializeField] public PlayerJumpData JumpData { get; private set; }
    }
}
