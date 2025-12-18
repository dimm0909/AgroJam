using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interaction
{
    public class MovableManager : MonoBehaviour
    {
        [SerializeField] internal List<Material> buildingModeMaterials;

        internal static MovableManager instance;

        private void Awake()
        {
            if (instance != this)
                DestroyImmediate(instance);
            instance = this;
        }
    }
}