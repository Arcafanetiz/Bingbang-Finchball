using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWall : MonoBehaviour
{
    // Movement Settings
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float xLimit = 3f;

    private void FixedUpdate()
    {
        transform.position = new Vector3(Mathf.Lerp(-1, 1, Mathf.PingPong(Time.time * moveSpeed, 1)) * xLimit, 0, transform.position.z);
    }
}