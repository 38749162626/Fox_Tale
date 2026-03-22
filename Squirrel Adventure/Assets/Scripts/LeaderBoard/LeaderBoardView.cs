using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardView : MonoBehaviour
{
    public GameObject scoreCellPrefab;
    private bool hasActive = false;

    public void GetHightScoreCallBack(List<UserData> datas)
    {
        for(int i = datas.Count - 1; i >= 0; i--)
        {
            GameObject scoreCell = Instantiate(scoreCellPrefab, transform);
            scoreCell.GetComponent<ScoreCell>().SetData(datas[i]);
            scoreCell.GetComponent<ScoreCell>().NoText.text = "No."  + (datas.Count - i).ToString();
        }
    }

    void Update()
    {
        if (gameObject.activeSelf && !hasActive)
        {
            StartCoroutine(LeaderBoardManager.GetHighScoreList(GetHightScoreCallBack));
            hasActive = true;
        }else if (!gameObject.activeSelf)
        {
            hasActive = false;
        }
    }
}
