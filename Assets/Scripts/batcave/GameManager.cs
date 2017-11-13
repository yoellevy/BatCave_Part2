using UnityEngine;
using UnityEngine.EventSystems;
using System;
using Infra.Audio;
using Infra.Gameplay;
using Infra.Gameplay.UI;

namespace BatCave
{
    /// <summary>
    /// The Game Manager.
    /// Allows starting and restarting the game.
    /// </summary>
    public class GameManager : MonoSingleton<GameManager>
    {
        public static event Action OnGameStarted;
        public static event Action OnGameReset;

        [SerializeField] AudioEvent startAudioEvent;
        [SerializeField] GameObject startGameUi;
        [SerializeField] GameObject gameOverUi;
        [SerializeField] EndlessWorldScroller[] scrollersToUpdate;

        [Header("Read Only")]
        [SerializeField]
        bool hasStarted;
        [SerializeField] bool isGameOver;

        public bool HasStarted
        {
            get
            {
                return hasStarted;
            }
        }

        private Transform cameraTransform;
        private Vector3 cameraInitialPosition;

        protected override void Init()
        {
            GameInputCapture.OnTouchDown += OnTouchDown;

            startGameUi.SetActive(true);
            gameOverUi.SetActive(false);

            // FindObjectOfType is expensive, but we can do it here because Init is
            // called only once.
            var sceneCamera = FindObjectOfType<Camera>();
            cameraTransform = sceneCamera.transform;
            cameraInitialPosition = cameraTransform.position;
        }

        protected void OnDestroy()
        {
            GameInputCapture.OnTouchDown -= OnTouchDown;
        }

        protected void Update()
        {
            if (hasStarted) return;

            // Handle keyboard input.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartNewGame();
            }
        }

        public void OnGameOver()
        {
            hasStarted = false;
            isGameOver = true;
            startGameUi.SetActive(false);
            gameOverUi.SetActive(true);
        }

        private void OnTouchDown(PointerEventData e)
        {
            if (hasStarted) return;
            if (!StartNewGame())
            {
                // Don't let the bat handle this event.
                e.Use();
            }
        }

        private bool StartNewGame()
        {
            if (isGameOver)
            {
                ResetGame();
                return false;
            }

            // EXERCISE: Play game started audio event.

            hasStarted = true;
            isGameOver = false;
            startGameUi.SetActive(false);
            gameOverUi.SetActive(false);

            if (OnGameStarted != null)
            {
                //startAudioEvent.Play();
                OnGameStarted();
            }
            return true;
        }

        private void ResetGame()
        {
            hasStarted = false;
            isGameOver = false;
            startGameUi.SetActive(true);
            gameOverUi.SetActive(false);

            cameraTransform.position = cameraInitialPosition;
            foreach (var scroller in scrollersToUpdate)
            {
                scroller.UpdateNow();
            }

            if (OnGameReset != null)
            {
                OnGameReset();
            }
        }
    }
}
