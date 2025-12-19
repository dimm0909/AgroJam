using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Player.PlayerInteract interactScript;
        [SerializeField] private List<Camera> cameras;
        [SerializeField] private List<GameObject> panels;

        private void Awake()
        {
            TurnMode(0);
        }


        /// <summary>
        /// 0 --- Region Choose mode
        /// 1 --- Main mode
        /// </summary>
        /// <param name="num"></param>
        public void TurnMode(int num = 0)
        {
            if ((num > cameras.Count-1) || num < 0)
            {
                Debug.LogError("Unacceptable mode index");
                return;
            }

            switch (num)
            {
                case 0:
                    interactScript.transform.parent.gameObject.SetActive(false);
                    break;
                case 1:
                    interactScript.transform.parent.gameObject.SetActive(true);
                    break;
            }

            foreach(Camera item in cameras)
            {
                if (item.gameObject.activeSelf)
                    item.gameObject.SetActive(false);
            }
            foreach (GameObject item in panels)
            {
                if (item.gameObject.activeSelf)
                    item.gameObject.SetActive(false);
            }

            cameras[num].gameObject.SetActive(true);
            panels[num].SetActive(true);
        }
    }
}


