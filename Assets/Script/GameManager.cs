using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Static Variables
    [HideInInspector] public static int Score;

    // Attach GameObjects
    [Header("Attach GameObjects")]
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject sideCanvas;
    [SerializeField] private GameObject arrowCanvas;
    [SerializeField] private GameObject arrowImage;
    [SerializeField] private GameObject powerBar;
    [SerializeField] private RectTransform barTransform;

    // Movement Settings
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float moveLimit = 4f;

    // Swing Settings
    [Header("Swing Settings")]
    [SerializeField] private float swingSpeed = 60f;
    [SerializeField] private float swingLimit = 45f;

    // Charge Settings
    [Header("Charge Settings")]
    [SerializeField] private float chargeSpeed = 1f;

    // Charge Settings
    [Header("Launch Settings")]
    [SerializeField] private float launchStrength = 75f;
    [SerializeField] private float minimumSpeed = 0.5f;
    [SerializeField] private float minimumTime = 1f;
    [SerializeField] private float maximumTime = 5f;

    // Reference Variables
    private GameObject currentBall;
    private Rigidbody ballRigidbody;

    // Internal Variables
    private int mode = 0;
    private float chargeTime = 0;
    private float chargePower = 0;
    private float launchTime = 0;

    // Encapsulated Variables
    private float ElapsedTime => (Time.time - launchTime);

    // Constant Variables
    private const int MODE_SPAWN = 0;
    private const int MODE_MOVE = 1;
    private const int MODE_SWING = 2;
    private const int MODE_CHARGE = 3;
    private const int MODE_LAUNCHED = 4;
    private const string HORIZONTAL_AXIS = "Horizontal";

    private void Awake()
    {
        barTransform.localScale = new Vector3(1, 0, 1);
    }

    private void Update()
    {
        if (mode == MODE_SPAWN)
        {
            SpawnBall();
            mode = MODE_MOVE;
        }
        else if (mode == MODE_MOVE)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                EnableArrow();
                mode = MODE_SWING;
            }
            else
            {
                MoveBall();
            }
        }
        else if (mode == MODE_SWING)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                EnableChargeBar();
                mode = MODE_CHARGE;
            }
            else
            {
                SwingArrow();
            }
        }
        else if (mode == MODE_CHARGE)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                Launch();
                mode = MODE_LAUNCHED;
            }
            else
            {
                Charge();
            }
        }
        else if (mode == MODE_LAUNCHED)
        {
            CheckSpeedAndTime();
        }
    }

    private void SpawnBall()
    {
        if (ballPrefab != null)
        {
            currentBall = Instantiate(ballPrefab, new Vector3(0, 1, 2), Quaternion.identity);
            ballRigidbody = currentBall.GetComponent<Rigidbody>();
            ballRigidbody.isKinematic = true;
            sideCanvas.SetActive(true);
        }
        else
        {
            Debug.LogError("Ball prefab not assigned or not found!");
        }
    }

    private void MoveBall()
    {
        float xInput = Input.GetAxis(HORIZONTAL_AXIS);

        Vector3 newPosition = currentBall.transform.position + xInput * moveSpeed * Time.deltaTime * Vector3.right;
        newPosition.x = Mathf.Clamp(newPosition.x, -moveLimit, moveLimit);

        currentBall.transform.position = newPosition;
        sideCanvas.transform.position = newPosition;
    }
    private void EnableArrow()
    {
        arrowCanvas.transform.position = new Vector3(currentBall.transform.position.x, 1.1f, 2);
        sideCanvas.SetActive(false);
        arrowImage.SetActive(true);
    }
    private void SwingArrow()
    {
        float currentSwingAngle = Mathf.Sin(Time.time * swingSpeed * Mathf.Deg2Rad) * swingLimit;
        arrowCanvas.transform.rotation = Quaternion.Euler(90f, 0f, currentSwingAngle);
    }
    private void EnableChargeBar()
    {
        arrowImage.SetActive(false);
        powerBar.SetActive(true);
        chargeTime = 0;
    }
    private void Charge()
    {
        chargePower = Mathf.Lerp(0, 1, Mathf.PingPong(chargeTime * chargeSpeed, 1));
        barTransform.localScale = new Vector3(1, chargePower, 1);
        chargeTime += Time.deltaTime;
    }
    private void Launch()
    {
        currentBall.transform.rotation = Quaternion.LookRotation(arrowCanvas.transform.up, Vector3.up);

        powerBar.SetActive(false);
        ballRigidbody.isKinematic = false;
        ballRigidbody.AddForce(chargePower * launchStrength * currentBall.transform.forward, ForceMode.Impulse);
        launchTime = Time.time;
    }
    private void CheckSpeedAndTime()
    {
        float currentSpeed = ballRigidbody.velocity.magnitude;
        if ((currentSpeed <= minimumSpeed) && (ElapsedTime > minimumTime))
        {
            ballRigidbody.drag = 1;
            mode = MODE_SPAWN;
        }
        else if (ElapsedTime > maximumTime)
        {
            ballRigidbody.drag = 1;
            mode = MODE_SPAWN;
        }
    }
}