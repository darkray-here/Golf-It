
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LaunchController : MonoBehaviour
{
    [SerializeField] private InputActionAsset playerControls;
    [SerializeField] private Rigidbody2D golfBall;
    [SerializeField] private Transform clubPivot;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip swingSound;

    [SerializeField] private GameObject aimDotPrefab;
    [SerializeField] private int dotCount = 12;
    [SerializeField] private float dotSpacing = 0.2f;
    [SerializeField] private float dotStartDistance = 0.25f;

    [SerializeField] private float aimSpeed = 60f;
    [SerializeField] private float minAngle = 20f;
    [SerializeField] private float maxAngle = 65f;

    [SerializeField] private float minPower = 7f;
    [SerializeField] private float maxPower = 12f;
    [SerializeField] private float chargeDuration = 1.5f;

    [SerializeField] private float backswingAngle = 45f;
    [SerializeField] private float swingAngle = 50f;
    [SerializeField] private float swingDuration = 0.18f;

    [SerializeField] private float stopVelocityThreshold = 0.05f;
    [SerializeField] private float stuckDelay = 1.5f;

    public bool isStuck;

    private InputActionMap gameplayMap;
    private InputAction aimAction;
    private InputAction swingAction;
    private GameObject[] aimDots;

    private float aimAngle = 25f;
    private float chargeTime;
    private float stuckTimer;

    private bool isCharging;
    private bool hasShot;

    private Quaternion startRotation;

    private void Awake()
    {
        if (playerControls == null || golfBall == null || clubPivot == null) return;
        gameplayMap = playerControls.FindActionMap("Gameplay");
        aimAction = gameplayMap.FindAction("Aim");
        swingAction = gameplayMap.FindAction("Swing");
    }

    private void OnEnable()
    {
        if (gameplayMap != null) gameplayMap.Enable();
    }

    private void OnDisable()
    {
        if (gameplayMap != null) gameplayMap.Disable();
    }

    private void Start()
    {
        startRotation = clubPivot.localRotation;
        aimDots = new GameObject[dotCount];
        for (int i = 0; i < dotCount; i++)
        {
            aimDots[i] = Instantiate(aimDotPrefab);
        }
    }

    private void Update()
    {
        if (hasShot)
        {
            HandleStuck();
            return;
        }
        HandleAim();
        HandleSwing();
    }

    private void HandleAim()
    {
        float input = aimAction.ReadValue<float>();
        aimAngle += input * aimSpeed * Time.deltaTime;
        aimAngle = Mathf.Clamp(aimAngle, minAngle, maxAngle);
        UpdateAimDots();
    }

    private void UpdateAimDots()
    {
        Vector2 direction = GetLaunchDirection();
        for (int i = 0; i < aimDots.Length; i++)
        {
            float distance = dotStartDistance + i * dotSpacing;
            aimDots[i].transform.position = golfBall.position + direction * distance;
        }
    }

    private void HandleSwing()
    {
        if (swingAction.WasPressedThisFrame())
        {
            isCharging = true;
            chargeTime = 0f;
        }

        if (isCharging && swingAction.IsPressed())
        {
            chargeTime += Time.deltaTime;
            float charge = Mathf.Clamp01(chargeTime / chargeDuration);
            clubPivot.localRotation = startRotation * Quaternion.Euler(0f, 0f, -backswingAngle * charge);
        }

        if (isCharging && swingAction.WasReleasedThisFrame())
        {
            isCharging = false;
            hasShot = true;
            Handheld.Vibrate();
            for (int i = 0; i < aimDots.Length; i++)
            {
                aimDots[i].SetActive(false);
            }
            float charge = Mathf.Clamp01(chargeTime / chargeDuration);
            float power = Mathf.Lerp(minPower, maxPower, charge);
            StartCoroutine(PlaySwing(power));
        }
    }

    private IEnumerator PlaySwing(float power)
    {
        Quaternion endRotation = startRotation * Quaternion.Euler(0f, 0f, swingAngle);
        float timer = 0f;
        bool hitBall = false;
        while (timer < swingDuration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / swingDuration);
            clubPivot.localRotation = Quaternion.Slerp(clubPivot.localRotation, endRotation, progress);
            if (!hitBall && progress >= 0.45f)
            {
                golfBall.AddForce(GetLaunchDirection() * power, ForceMode2D.Impulse);
                if (swingSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(swingSound);
                }
                hitBall = true;
            }
            yield return null;
        }
        clubPivot.localRotation = startRotation;
    }

    private Vector2 GetLaunchDirection()
    {
        float angle = aimAngle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    private void HandleStuck()
    {
        if (isStuck) return;
        if (golfBall.linearVelocity.magnitude <= stopVelocityThreshold)
        {
            stuckTimer += Time.deltaTime;
            if (stuckTimer >= stuckDelay)
            {
                isStuck = true;
            }
        }
        else
        {
            stuckTimer = 0f;
        }
    }

}
