using UnityEngine;
using UnityEngine.InputSystem;

namespace cleon
{
    [RequireComponent(typeof(CapsuleCollider), typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerInput))]
    public class ThirdPersonPlayerController : MonoBehaviour
    {
        Transform camTransform => cameraPlvot.transform;

        [Header("Movement Settings")]
        [SerializeField, Min(1.0f)] float defaultSpeed = 5f;
        [SerializeField, Min(1.0f)] float walkSpeedModifier = 1f;
        [SerializeField, Min(1.5f)] float sprintSpeedModifier = 2f;
        [SerializeField, Min(0.5f)] float crouchSpeedModifier = .5f;
        [SerializeField, Min(1.0f)] float jumpingSpeed = 5f;
        [SerializeField] InputActionReference moveAction;
        [SerializeField] InputActionReference jumpAction;
        [SerializeField] InputActionReference sprintAction;
        [SerializeField] InputActionReference crouchAction;
        bool isSprinting;
        bool isCrouching;
        bool isGrounded;

        [Header("Look Settings")]
        [SerializeField] new ThirdPersonCamera camera;
        [SerializeField] InputActionReference lookAction;
        [SerializeField] Transform cameraPlvot;
        // This value iss set to a low number beacause our camera controller
        // is not framerate locker which makes it feel smoother.
        [SerializeField, Range(0, 3)] float sensitivity = .5f;
        // This is how far up and down the camera will be able to look.
        // 90 means full vertical look without inverting the camera.
        [SerializeField, Range(0, 90)] float verticalLookCap = 90f;

        new CapsuleCollider collider;
        new Rigidbody rigidbody;
        Animator anim;

        // The current rotation of the camera that gets update every
        // time the input is changer.
        Vector2 rotation = Vector2.zero;
        // The amount of movement that is waiting to be applied
        Vector3 movement = Vector3.zero;

        // Start is called before the first frame update
        void Start()
        {
            anim = gameObject.GetComponentInChildren<Animator>();
            collider = gameObject.GetComponent<CapsuleCollider>();
            rigidbody = gameObject.GetComponent<Rigidbody>();
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rigidbody.freezeRotation = true;

            // This is how you link the function into the actual press/usage
            // of the action such as moing the mouse will perform the lookAction
            lookAction.action.performed += OnLookPerformed;

            sprintAction.action.performed += (_context) => isSprinting = true;
            sprintAction.action.canceled += (_context) => isSprinting = false;
            crouchAction.action.performed += (_context) => isCrouching = true;
            crouchAction.action.canceled += (_context) => isCrouching = false;
            jumpAction.action.performed += OnJumpPerformed;

            sprintAction.action.GetBindingDisplayString(0, out string device, out string key);
            Debug.Log($"Sprint is mapped to <{device}>/{key}");

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Update is called once per frame
        void Update()
        {

            camTransform.localRotation = Quaternion.AngleAxis(rotation.y, Vector3.left);
            transform.localRotation = Quaternion.AngleAxis(rotation.x, Vector3.up);

            if (moveAction.action.ReadValue<Vector2>().x != 0 || moveAction.action.ReadValue<Vector2>().y != 0)
            {
                anim.SetBool("Walk", true);
            }
            else
            {
                anim.SetBool("Walk", false);
            }

            // Apply the movement and reset it to 0
            UpdateMovement();
            transform.position += movement;
            movement = Vector3.zero;
        }

        void OnJumpPerformed(InputAction.CallbackContext _context)
        {
            bool value = _context.ReadValueAsButton();
            if (value && isGrounded)
            {
                rigidbody.velocity = new Vector3(0, jumpingSpeed, 0);
                isGrounded = false;
            }
        }

        void OnLookPerformed(InputAction.CallbackContext _context)
        {
            // There has been some sort of input update
            Vector2 value = _context.ReadValue<Vector2>();
            rotation.x += value.x * sensitivity;
            rotation.y += value.y * sensitivity;

            // Prevent the vertical look from going outside the specified angle
            rotation.y = Mathf.Clamp(rotation.y, -verticalLookCap, verticalLookCap);

            // Apply the rotation early
            camTransform.localRotation = Quaternion.AngleAxis(rotation.y, Vector3.left);
            transform.localRotation = Quaternion.AngleAxis(rotation.x, Vector3.up);

            // Check if the rotation was valid based on the distance
            if (!camera.CanRotate())
            {
                // We were too close so revert the vertical rotation
                rotation.y -= value.y * sensitivity;
            }

            // Reapply the rotation
            camTransform.localRotation = Quaternion.AngleAxis(rotation.y, Vector3.left);
            transform.localRotation = Quaternion.AngleAxis(rotation.x, Vector3.up);
        }

        void UpdateMovement()
        {
            float speed = defaultSpeed *
                (isSprinting ? sprintSpeedModifier :
                isCrouching ? crouchSpeedModifier :
                walkSpeedModifier) * Time.deltaTime;

            Vector2 value = moveAction.action.ReadValue<Vector2>();
            movement += transform.forward * value.y * speed;
            movement += transform.right * value.x * speed;
        }

        private void OnCollisionEnter(Collision collision)
        {
            isGrounded = true;
        }
    }
}

