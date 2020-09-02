using UnityEngine;
using TMPro;
using System;
using System.IO;

public class Scores : MonoBehaviour
{
    TextMeshProUGUI text;
    Controls controls;
    public float highScore;
    public GameObject player;
    Moving moving;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        controls = player.GetComponent<Controls>();
        moving = player.GetComponent<Moving>();
    }

    void Update()
    {
        if (controls != null && moving != null)
        {
            text.text = $"Highscore: {Math.Floor(moving.highScore)} | Scores: {Math.Floor(controls.scores)}";
        }
    }
}