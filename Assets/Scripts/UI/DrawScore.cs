using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DrawScore : MonoBehaviour
{
    [SerializeField] private TMP_Text tmp;
    private int currentScore = 0;
    private void Update()
    {
        var targetScore = GameManager.Instance.Score;
        if (currentScore != targetScore)
        {
            currentScore = Mathf.CeilToInt(Mathf.Lerp(currentScore, targetScore, Time.deltaTime  * 5.0f));
        }
        tmp.text = $"{currentScore}";
    }
}
