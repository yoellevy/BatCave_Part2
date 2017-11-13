using UnityEngine;
using UnityEngine.EventSystems;
using Infra.Audio;
using Infra.Gameplay.UI;

namespace BatCave
{
    /// <summary>
    /// The Bat controller. Responsible for playing bat animations, handling collision
    /// with the cave walls and responding to player input.
    /// </summary>
    [RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
    public class Bat : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField]
        float flyYSpeed;
        [SerializeField] float xSpeed;

        [Header("Animation")]
        [SerializeField]
        string flyUpBoolAnimParamName;
        [SerializeField] string isAliveBoolAnimParamName;

        [Header("Audio")]
        [SerializeField]
        GameObject wingsSoundSource;
        [SerializeField] AudioEvent hitAudioEvent;

        [Header("State")]
        [SerializeField]
        bool isAlive;

        [Header("Testing")]
        [SerializeField]
        bool isInvulnerable;

        [Header("Controller")]
        [SerializeField]
        BatController batController;

        private bool FlyUp
        {
            get
            {
                return _flyUp;
            }
            set
            {
                if (value != _flyUp)
                {
                    _flyUp = value;
                    if (wingsSoundSource != null)
                    {
                        wingsSoundSource.SetActive(_flyUp);
                    }
                }
            }
        }

        private int flyUpBoolAnimParamId;
        private int isAliveBoolAnimParamId;

        private bool _flyUp;
        private Animator animator;
        private Rigidbody2D body;

        private Vector2 initialPosition;

        protected void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();

            flyUpBoolAnimParamId = Animator.StringToHash(flyUpBoolAnimParamName);
            isAliveBoolAnimParamId = Animator.StringToHash(isAliveBoolAnimParamName);           

            GameManager.OnGameStarted += OnGameStarted;
            GameManager.OnGameReset += OnGameReset;

            initialPosition = body.position;
            OnGameReset();
        }

        protected void OnDestroy()
        {
            GameManager.OnGameStarted -= OnGameStarted;
            GameManager.OnGameReset -= OnGameReset;
        }

        protected void Update()
        {
            if (!isAlive) return;

            FlyUp = batController.WantsToFlyUp();
            
            animator.SetBool(flyUpBoolAnimParamId, FlyUp);
            animator.SetBool(isAliveBoolAnimParamId, isAlive);
        }

        protected void FixedUpdate()
        {
            if (!isAlive) return;
            var velocity = body.velocity;
            velocity.x = xSpeed;
            if (FlyUp)
            {
                velocity.y = flyYSpeed;
            }
            body.velocity = velocity;
        }

        protected void OnCollisionEnter2D(Collision2D collision)
        {
            // Play death sound.
            if (isAlive && hitAudioEvent != null)
            {
                hitAudioEvent.Play();
            }

            if (isInvulnerable) return;

            // Stop flying.
            FlyUp = false;
            body.velocity = Vector2.zero;

            // Play death animation.
            isAlive = false;
            animator.SetBool(isAliveBoolAnimParamId, isAlive);
            enabled = false;

            GameManager.Instance.OnGameOver();
        }

        private void OnGameStarted()
        {
            // Let the bat fly!
            body.constraints = RigidbodyConstraints2D.None;
        }

        private void OnGameReset()
        {
            // Stop the bat.
            body.velocity = Vector2.zero;
            body.angularVelocity = 0f;
            // Reset it's position.
            body.rotation = 0f;
            body.position = initialPosition;
            transform.position = initialPosition;
            // Prevent it from moving.
            body.constraints = RigidbodyConstraints2D.FreezeAll;

            // Reanimate the bat. Bring it back from the dead.
            isAlive = true;
            FlyUp = false;

            enabled = true;
        }
     
    }
}
