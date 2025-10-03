using UnityEngine;
using UnityEngine.SceneManagement;

namespace UB
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager Instance { get; private set; }

        public PlayerManager player;

        private PlayerControls playerControls;

        private Vector2 movementInput;
        public float verticalInput { get; private set; }
        public float horizontalInput { get; private set; }
        public float MovementAmount { get; private set; }

        public Vector2 MousePosition { get; private set; }
        public Vector3 MouseDirection { get; private set; }

        private void Awake()
        {
            CreateInstance();

            SceneManager.activeSceneChanged += OnSceneChange;

            Instance.enabled = false;
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if (newScene.buildIndex == WorldSaveGameManager.Instance.WorldSceneIndex) {
                Instance.enabled = true;
            }
            else {
                Instance.enabled = false;
            }
        }

        private void OnEnable()
        {
            if (playerControls == null) {
                playerControls = new PlayerControls();

                playerControls.PlayerMovement.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
            }

            playerControls.Enable();
        }

        private void Update()
        {
            HandleMovementInput();
            HandleMouseInput();
        }

        //Movement Input
        private void HandleMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;

            MovementAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));

            // TODO: Maybe clamp e.g if moveamount <= 05f, then moveamount = 0.5f;
            if (MovementAmount <= 0.5 && MovementAmount > 0) {
                MovementAmount = 0.5f;
            }
            else if (MovementAmount > 0.5 && MovementAmount <= 1) {
                MovementAmount = 1;
            }
        }

        //Mouse Input
        private void HandleMouseInput()
        {
            if (PlayerManager.Instance == null) {
                return;
            }
            MousePosition = playerControls.MouseActions.MousePosition.ReadValue<Vector2>();

            // Get player's screen position
            Vector3 playerScreenPos = PlayerCamera.Instance.MainCamera.WorldToScreenPoint(PlayerManager.Instance.transform.position);

            // Calculate direction from player to mouse in screen space
            Vector2 screenDirection = MousePosition - new Vector2(playerScreenPos.x, playerScreenPos.y);

            // Convert to normalized direction (just for rotation, not actual world position)
            MouseDirection = new Vector3(screenDirection.x, 0, screenDirection.y).normalized;
        }

        private void CreateInstance()
        {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else {
                Destroy(gameObject);
            }
        }

        private void OnApplicationFocus(bool focus)
        {
            if (enabled) {
                if (focus) {
                    playerControls.Enable();
                }
                else {
                    playerControls.Disable();
                }
            }
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;
        }
    }
}
