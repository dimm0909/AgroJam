using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interaction
{
    public enum PlacingSurface
    {
        Floor = 0,
        Wall = 1
    }
    [RequireComponent(typeof(Collider))]
    public class MovableItem : MonoBehaviour
    {
        internal const float MAGNITE_DISTANCE = 1f;

        [SerializeField] internal PlacingSurface surface;
        [SerializeField] internal Transform basicParent;

        internal bool isGrabbed { get; private set; }
        internal bool canPlace { get; private set; }

        private string[] _possibleSurfaces = { "Floor", "Wall" };
        private int _collisionsEntersCount = 0;
        private Material startMaterial;
        private MeshRenderer render;

        private void Awake()
        {
            canPlace = true;
            render = GetComponent<MeshRenderer>();
        }

        private void FixedUpdate()
        {
            if (!isGrabbed)
                return;

            if (canPlace != (_collisionsEntersCount == 0))
            {
                canPlace = _collisionsEntersCount == 0;
                ChangePlacingPossibility(canPlace);
            }
            RaycastHit hit, hit1;

            switch (surface)
            {
                case PlacingSurface.Floor:
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down),
                    out hit, MAGNITE_DISTANCE))
                    {
                        if (hit.transform.tag == _possibleSurfaces[(int)surface])
                        {
                            Vector3 rot = transform.rotation.eulerAngles;
                            transform.rotation = Quaternion.identity;

                            transform.Rotate(0, rot.y, 0);

                            transform.position = new Vector3(transform.position.x, hit.point.y + (this.transform.localScale.y / 1.9f),
                                transform.position.z);
                        }

                    }
                    break;

                case PlacingSurface.Wall:

                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left),
                    out hit1, MAGNITE_DISTANCE*1.5f))
                    {
                        if (hit1.transform.tag == _possibleSurfaces[(int)surface])
                        {
                            Vector3 rot = transform.rotation.eulerAngles;
                            transform.rotation = Quaternion.identity;
                            transform.Rotate(0, rot.y, rot.z);
                            transform.position = new Vector3(hit1.point.x, transform.position.y, transform.position.z);
                        }
                    }

                    break;
            }

        }

        private void OnTriggerEnter(Collider other) => _collisionsEntersCount++;
        private void OnTriggerExit(Collider other) => _collisionsEntersCount--;

        internal void ChangeObjectState(bool grabbing = true, Transform pinObject = null)
        {
            if (!canPlace)
                return;

            isGrabbed = grabbing;
            this.transform.parent = grabbing ? pinObject : basicParent;
            GetComponent<Collider>().isTrigger = grabbing;

            if (grabbing)
            {
                startMaterial = render.sharedMaterial;
                ChangePlacingPossibility(true);
            }

            else
                render.material = startMaterial;
        }

        private void ChangePlacingPossibility(bool state)
        {
            render.material = MovableManager.instance.buildingModeMaterials[state ? 0 : 1];
            canPlace = state;
        }
        private void OnBecameInvisible()
        {
            if (!isGrabbed)
                return;

            Transform cam = FindObjectOfType<Camera>()?.transform;
            if (!cam)
                return;

            Vector3 point = new Vector3(cam.GetComponent<Camera>().pixelWidth / 2, cam.GetComponent<Camera>().pixelHeight / 2, 0);
            Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(point);
            transform.position = ray.GetPoint(MAGNITE_DISTANCE * 4);
        }
    }
}