using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class Player2D : MonoBehaviour
{

    public static Player2D instance;

    [HideInInspector] public BoxCollider2D box_collider;

    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;
    private float accelerationTimeAirborne = .2f;
    private float accelerationTimeGrounded = .1f;
    private float moveSpeed = 6;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    private float timeToWallUnstick;

    private float gravity;
    private float maxJumpVelocity;
    private float minJumpVelocity;
    public Vector3 Velocity { get; private set; }

    private float velocityXSmoothing;

    private Controller2D controller;

    private Vector2 directionalInput;
    private bool wallSliding;
    private int wallDirX;

    private bool doubleJumpLeftThisJump;

    [Space]
    [Header("Dashing")]
    [SerializeField] private KeyCode activateDashKey;
    [SerializeField] private float dashDuration;
    private float dashDurationCountdownTimer;

    [SerializeField] private float dashDistance;

    [SerializeField] private float dashCooldownTime;
    private float dashCountdownTimer;

    [SerializeField] private ParticleSystem dashParticle;

    private bool currentlyDashing;

    private Vector3 dashDirection;

    private Vector3 velocityBeforeDash;

    private bool dashLeftThisJump;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("There can't be more than one player2D in the scene at any given time.");
        }

        box_collider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

        dashCountdownTimer = dashCooldownTime;

        if (dashParticle != null)
        {
            ParticleSystem.EmissionModule emission = dashParticle.emission;
            emission.enabled = false;
        }
        else
        {
            Debug.LogError("Player, no dashParticle refrence given.");
        }

        doubleJumpLeftThisJump = false;
    }

    void Update()
    {
        if (!currentlyDashing)
        {
            CalculateVelocity();
        }

        /*
        HandleWallSliding();
        */

        HandleDash();

        controller.Move(Velocity * Time.deltaTime, directionalInput);

        if (controller.collisions.above || controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope)
            {
                Velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
            }
            else
            {
                Velocity.y = 0;
            }
        }
    }

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }

    public void OnJumpInputDown()
    {
        if (wallSliding)
        {
            if (wallDirX == directionalInput.x)
            {
                Velocity.x = -wallDirX * wallJumpClimb.x;
                Velocity.y = wallJumpClimb.y;
            }
            else if (directionalInput.x == 0)
            {
                Velocity.x = -wallDirX * wallJumpOff.x;
                Velocity.y = wallJumpOff.y;
            }
            else
            {
                Velocity.x = -wallDirX * wallLeap.x;
                Velocity.y = wallLeap.y;
            }
        }

        if (controller.collisions.below || doubleJumpLeftThisJump)
        {
            if (controller.collisions.below)
            {
                doubleJumpLeftThisJump = true;
            }
            else
            {
                doubleJumpLeftThisJump = false;
            }

            if (controller.collisions.slidingDownMaxSlope)
            {
                if (directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x))
                { // not jumping against max slope
                    Velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
                    Velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
                }
            }
            else
            {
                Velocity.y = maxJumpVelocity;
            }
        }
    }

    public void OnJumpInputUp()
    {
        if (Velocity.y > minJumpVelocity)
        {
            Velocity.y = minJumpVelocity;
        }
    }


    void HandleWallSliding()
    {
        wallDirX = (controller.collisions.left) ? -1 : 1;
        wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && Velocity.y < 0)
        {
            wallSliding = true;

            if (Velocity.y < -wallSlideSpeedMax)
            {
                Velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                Velocity.x = 0;

                if (directionalInput.x != wallDirX && directionalInput.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }
        }
    }

    private void HandleDash()
    {
        dashCountdownTimer -= Time.deltaTime;

        if (dashCountdownTimer <= 0f)
        {
            if (Input.GetKeyDown(activateDashKey) && dashLeftThisJump)
            {
                velocityBeforeDash = Velocity;

                dashCountdownTimer = dashCooldownTime;
                currentlyDashing = true;
                dashDurationCountdownTimer = dashDuration;

                dashLeftThisJump = false;

                dashDirection = Velocity.normalized;

                Velocity = dashDirection * (dashDistance / dashDuration);

                if (dashParticle != null)
                {
                    ParticleSystem.EmissionModule emission = dashParticle.emission;
                    emission.enabled = true;
                }
            }
        }

        if (currentlyDashing)
        {
            Velocity = dashDirection * (dashDistance / dashDuration);
        }

        if (currentlyDashing && dashDurationCountdownTimer <= 0f)
        {
            currentlyDashing = false;

            Velocity = velocityBeforeDash;

            Velocity.y = Mathf.Max(Velocity.y, 0f);

            if (dashParticle != null)
            {
                ParticleSystem.EmissionModule emission = dashParticle.emission;
                emission.enabled = false;
            }
        }

        dashDurationCountdownTimer -= Time.deltaTime;

        if (controller.collisions.below)
        {
            dashLeftThisJump = true;
        }
    }

    void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        Velocity.x = Mathf.SmoothDamp(Velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        Velocity.y += gravity * Time.deltaTime;
    }
}