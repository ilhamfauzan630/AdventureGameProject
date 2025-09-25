using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureGame
{
    public class BillBoard : MonoBehaviour
    {
        void LateUpdate()
        {
            if (Camera.main) transform.forward = Camera.main.transform.forward;
        }
    }
}
