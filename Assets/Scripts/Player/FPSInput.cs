using UnityEngine;
using System.Collections;

namespace Movement
{
    [RequireComponent(typeof(CharacterController))]
    [AddComponentMenu("Control Script/FPS Input")]
    public class FPSInput : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Animator _anim;

        public float speed = 6.0f;
        public float gravity = -9.8f;

        private CharacterController _charController;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _charController = GetComponent<CharacterController>();
            _anim.SetBool("IsWalking", false);
        }

        void OnGUI()
        {
            int size = 12;
            float posX = _camera.pixelWidth / 2 - size / 4;
            float posY = _camera.pixelHeight / 2 - size / 2;
            GUI.Label(new Rect(posX, posY, size, size), "*");
        }

        void Update()
        {
            float deltaX = Input.GetAxis("Horizontal") * speed;
            float deltaZ = Input.GetAxis("Vertical") * speed;
            Vector3 movement = new Vector3(deltaX, 0, deltaZ);
            movement = Vector3.ClampMagnitude(movement, speed);

            movement.y = gravity;

            movement *= Time.deltaTime;
            movement = transform.TransformDirection(movement);
            _charController.Move(movement);

            if ((Mathf.Abs(deltaX) + Mathf.Abs(deltaZ)) > 0.2f)
            {
                _anim.SetBool("IsWalking", true);
            }
            else
            {
                _anim.SetBool("IsWalking", false);
            }
        }
    }
}