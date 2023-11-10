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
    [SerializeField] private TextMeshProUGUI scoreLabel;

    // Constant Variables
    private const string SCORETEXT = " SCORE";

    private void Update()
    {
        scoreLabel.text = Score + SCORETEXT;
    }
}