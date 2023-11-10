using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class BallScore : MonoBehaviour
{
    private void Update()
    {
        if (transform.position.y < 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ScoreZone"))
        {
            GameManager.Score++;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ScoreZone"))
        {
            GameManager.Score--;
        }
    }
}