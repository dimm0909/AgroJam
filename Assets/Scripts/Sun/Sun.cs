using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
    public class Sun : MonoBehaviour
    {
        private const float DAY_SPEED = 0.1f;

        private void FixedUpdate()
        {
            this.transform.Rotate(DAY_SPEED, 0, 0);
        }
    }
}
