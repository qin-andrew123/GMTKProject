using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerController : MonoBehaviour, IPlayerController
{
    [SerializeField] private PlayerControllerStats _stats;
    private Player player;
    private float fallSpeedYDampingThreshold;
    private Rigidbody2D _rb;
    private CapsuleCollider2D _col;
    private FrameInput _frameInput;
    private Vector2 _frameVelocity;
    private Vector2 lookDirection = Vector2.right;
    private bool _cachedQueryStartInColliders;
    private bool bCanClimb = false;
    private bool bSlidingOnIce = false;
    private Vector2 frameMouseInput;
    private bool bCanBreakBlock = true;
    public bool SlidingOnIce
    {
        set { bSlidingOnIce = value; }
    }
    public bool CanClimb
    {
        get { return bCanClimb; }
        set { bCanClimb = value; }
    }

    [SerializeField] private string interactString = "Interact [H]";
    [SerializeField] private string breakableString = "Break [Left Click]";

    #region Interface

    public Vector2 FrameInput => _frameInput.Move;
    public event Action<bool, float> GroundedChanged;
    public event Action Jumped;
    public static event Action<float> OnHorizontalChangeDirection;
    #endregion

    private float _time;
    private bool bCanGatherInput = true;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<CapsuleCollider2D>();

        _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
    }
    private void Start()
    {
        fallSpeedYDampingThreshold = CameraManager.Instance._fallSpeedYDampingChangeThreshold;
        player = GetComponent<Player>();
        numJumps = numJumpsTotal;
        Player.OnUpgradePurchased += ReceiveAdditionalJumps;
        Blacksmith.OnActivateBlacksmithUI += UpdateCanMineBlock;
        SpawnPortalComponent.OnTravellingToSpawn += IgnoreInputTravelling;
    }

    private void OnDestroy()
    {
        Player.OnUpgradePurchased -= ReceiveAdditionalJumps;
        Blacksmith.OnActivateBlacksmithUI -= UpdateCanMineBlock;
        SpawnPortalComponent.OnTravellingToSpawn -= IgnoreInputTravelling;
    }

    private void IgnoreInputTravelling(float time)
    {
        StartCoroutine(IgnoreInput(time));
    }
    private IEnumerator IgnoreInput(float time)
    {
        bCanGatherInput = false;
        yield return new WaitForSeconds(time);
        bCanGatherInput = true;
    }
    private void Update()
    {
        _time += Time.deltaTime;
        if (!bCanGatherInput)
        {
            return;
        }
        GatherInput();
        
        if (Input.GetButtonDown("Interact"))
        {
            Debug.Log("Interact Button Pressed: Sending Signal");
            PlayerAttemptHarvest?.Invoke(gameObject);
        }
        if (Input.GetButtonDown("Interact"))
        {
            Debug.Log("Blacksmith Shop Button Pressed: Sending Signal");
            PlayerAttemptShop?.Invoke(gameObject);
        }
        if (Input.GetButtonDown("Fire1") && bCanBreakBlock)
        {
            frameMouseInput = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            frameMouseInput.Normalize();
            Debug.Log(frameMouseInput);
            MineBlock();
        }

        if (_rb.velocity.y < fallSpeedYDampingThreshold && !CameraManager.Instance.IsLerpingYDamping && !CameraManager.Instance.LerpedFromPlayerFalling)
        {
            CameraManager.Instance.LerpYDamping(true);
        }

        if (_rb.velocity.y >= 0f && !CameraManager.Instance.IsLerpingYDamping && CameraManager.Instance.LerpedFromPlayerFalling)
        {
            // reset so can be called again
            CameraManager.Instance.LerpedFromPlayerFalling = false;

            CameraManager.Instance.LerpYDamping(false);
        }
    }

    private void GatherInput()
    {
        _frameInput = new FrameInput
        {
            JumpDown = Input.GetButtonDown("Jump"),
            JumpHeld = Input.GetButton("Jump"),
            DashDown = Input.GetButtonDown("Dash"),
            DashHeld = Input.GetButton("Dash"),
            Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
        };

        if (_frameInput.Move.x != 0)
        {
            lookDirection.x = _frameInput.Move.x;
            lookDirection.Normalize();
        }
        if (_stats.SnapInput)
        {
            _frameInput.Move.x = Mathf.Abs(_frameInput.Move.x) < _stats.HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.x);
            _frameInput.Move.y = Mathf.Abs(_frameInput.Move.y) < _stats.VerticalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.y);
        }
        if (!CameraManager.Instance.OverrideLookatLerping)
        {
            OnHorizontalChangeDirection?.Invoke(_frameInput.Move.x);
        }

        if (_frameInput.JumpDown)
        {
            _jumpToConsume = true;
            _timeJumpWasPressed = _time;
        }
        if (_frameInput.DashDown && bCanDashNow && !bSlidingOnIce)
        {
            bIsDashing = true;
            bCanDashNow = false;
            dashDirection = new Vector2(_frameInput.Move.x, 0);
            if (dashDirection == Vector2.zero)
            {
                dashDirection = lookDirection;
            }
            StartCoroutine(StopDashing());
        }
    }
    private void UpdateCanMineBlock(bool isBlacksmithActive)
    {
        bCanBreakBlock = !isBlacksmithActive;
    }
    private void MineBlock()
    {
        RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, frameMouseInput, _stats.MiningDistance, _stats.MiningLayer);
        if (hit)
        {
            Debug.Log("Hit something!");
            BreakableBlockComponent breakableBlockComponent = hit.rigidbody.gameObject.GetComponent<BreakableBlockComponent>();
            if (breakableBlockComponent != null)
            {
                breakableBlockComponent.AttemptBreakBlock(player.ObstacleDestructionLevel);
            }
        }
    }
    private void FixedUpdate()
    {
        if (!bCanGatherInput)
        {
            return;
        }
        CheckCollisions();

        if (player.CanDash)
        {
            if (bIsDashing)
            {
                _frameVelocity = dashDirection * _stats.DashingVelocity;
                ApplyMovement();
                return;
            }
        }

        if (!bSlidingOnIce)
        {
            HandleJump();
        }
        if (bCanClimb)
        {
            HandleClimb();
        }
        else
        {
            HandleDirection();
            HandleGravity();
        }

        ApplyMovement();
    }


    #region Dash
    private Vector2 dashDirection = Vector2.right;
    private bool bIsDashing = false;
    private bool bCanDashNow = true;

    private IEnumerator StopDashing()
    {
        PlayerHealth playerHealthComponent = GetComponent<PlayerHealth>();
        playerHealthComponent.CallInvulnerability(_stats.DashingDuration);
        yield return new WaitForSeconds(_stats.DashingDuration);
        bIsDashing = false;
        yield return new WaitForSeconds(_stats.DashingCooldown);
        bCanDashNow = true;
    }
    #endregion

    #region Collisions

    private float _frameLeftGrounded = float.MinValue;
    private bool _grounded = true;
    public bool Grounded
    {
        get { return _grounded; }
        set { _grounded = value; }
    }
    private void CheckCollisions()
    {
        Physics2D.queriesStartInColliders = false;

        // Ground and Ceiling
        bool groundHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.down, _stats.GrounderDistance, ~_stats.PlayerLayer);
        bool ceilingHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.up, _stats.GrounderDistance, ~_stats.PlayerLayer);

        // Hit a Ceiling
        if (ceilingHit) _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);

        // Landed on the Ground
        if (!_grounded && groundHit)
        {
            _grounded = true;
            _coyoteUsable = true;
            _bufferedJumpUsable = true;
            _endedJumpEarly = false;
            numJumps = numJumpsTotal;
            GroundedChanged?.Invoke(true, Mathf.Abs(_frameVelocity.y));
        }
        // Left the Ground and is not climbing
        else if (_grounded && !groundHit && !bCanClimb)
        {
            _grounded = false;
            _frameLeftGrounded = _time;
            GroundedChanged?.Invoke(false, 0);
        }

        Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
    }

    #endregion

    #region Climbing
    private void HandleClimb()
    {
        if (_frameInput.Move.x == 0)
        {
            _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, _stats.GroundDeceleration * Time.fixedDeltaTime);
        }
        else
        {
            _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _stats.MaxSpeed, _stats.Acceleration * Time.fixedDeltaTime);
        }
        HandleJump();
        if (_frameInput.Move.y == 0)
        {
            _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, 0, _stats.GroundDeceleration * Time.fixedDeltaTime);
        }
        else
        {
            _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, _frameInput.Move.y * _stats.MaxClimbSpeed, _stats.Acceleration * Time.fixedDeltaTime);
        }
    }
    #endregion

    #region Jumping
    private int numJumpsTotal = 1;
    private int numJumps = 0;
    private bool _jumpToConsume;
    private bool _bufferedJumpUsable;
    private bool _endedJumpEarly;
    private bool _coyoteUsable;
    private float _timeJumpWasPressed;

    private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + _stats.JumpBuffer;
    private bool CanUseCoyote => _coyoteUsable && !_grounded && _time < _frameLeftGrounded + _stats.CoyoteTime;
    private void ReceiveAdditionalJumps(Upgrade type, int amount)
    {
        Debug.Log("Received additional jumps. This should only appear once");
        numJumpsTotal += amount;
    }
    private void HandleJump()
    {
        if (!_endedJumpEarly && !_grounded && !_frameInput.JumpHeld && _rb.velocity.y > 0) _endedJumpEarly = true;

        if (!_jumpToConsume && !HasBufferedJump) return;

        if (_grounded || CanUseCoyote || numJumps > 0)
        {
            ExecuteJump();
        }

        _jumpToConsume = false;
    }

    private void ExecuteJump()
    {
        _endedJumpEarly = false;
        _timeJumpWasPressed = 0;
        _bufferedJumpUsable = false;
        _coyoteUsable = false;
        _frameVelocity.y = _stats.JumpPower;
        numJumps--;
        Jumped?.Invoke();
    }

    #endregion

    #region Horizontal

    private void HandleDirection()
    {
        if (_frameInput.Move.x == 0)
        {
            var deceleration = _grounded ? _stats.GroundDeceleration : _stats.AirDeceleration;
            if (bSlidingOnIce)
            {
                deceleration = _stats.IceDeceleration;
            }
            _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
        }
        else
        {
            var acceleration = bSlidingOnIce ? _stats.IceAcceleration : _stats.Acceleration;
            _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _stats.MaxSpeed, acceleration * Time.fixedDeltaTime);
        }
    }

    #endregion

    #region Gravity

    private void HandleGravity()
    {
        if (_grounded && _frameVelocity.y <= 0f)
        {
            _frameVelocity.y = _stats.GroundingForce;
        }
        else
        {
            var inAirGravity = _stats.FallAcceleration;
            if (_endedJumpEarly && _frameVelocity.y > 0) inAirGravity *= _stats.JumpEndEarlyGravityModifier;
            _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_stats.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
        }
    }

    #endregion

    private void ApplyMovement() => _rb.velocity = _frameVelocity;

    #region Harvesting
    public static event Action<GameObject> PlayerAttemptHarvest;
    #endregion

    #region Interacting
    public static event Action<GameObject> PlayerAttemptShop;
    #endregion

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_stats == null) Debug.LogWarning("Please assign a ScriptableStats asset to the Player Controller's Stats slot", this);
    }
#endif
}

public struct FrameInput
{
    public bool JumpDown;
    public bool JumpHeld;
    public bool DashDown;
    public bool DashHeld;
    public Vector2 Move;
}

public interface IPlayerController
{
    public event Action<bool, float> GroundedChanged;

    public event Action Jumped;
    public Vector2 FrameInput { get; }
}
