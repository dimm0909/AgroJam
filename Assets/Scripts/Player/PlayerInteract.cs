using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Player
{

    [RequireComponent(typeof(Camera))]
    public class PlayerInteract : MonoBehaviour
    {
        internal const float USAGE_DISTANCE = 5f;

        [SerializeField] private GameObject _uiTip;

        private Camera _camera;
        private Interaction.MovableItem _itemToInteract = null;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _itemToInteract = null;
            ShowTip(false);
        }

        private void Update()
        {
            Vector3 point = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
            Ray ray = _camera.ScreenPointToRay(point);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, USAGE_DISTANCE))
            {
                Interaction.MovableItem isMovable = hit.transform.gameObject.GetComponent<Interaction.MovableItem>();
                _itemToInteract = isMovable;
            }
            else
            {
                _itemToInteract = null;
                ShowTip(false);
                return;
            }

            if (_itemToInteract)
            {
                ShowTip(true);

                if (Input.GetMouseButtonDown(0))
                    _itemToInteract.ChangeObjectState(!_itemToInteract.isGrabbed, this.transform);
            }
            else
                ShowTip(false);
        }

        private void ShowTip(bool state)
        {
            if (_uiTip.activeSelf != state)
                _uiTip.SetActive(state);
        }
    }
}
