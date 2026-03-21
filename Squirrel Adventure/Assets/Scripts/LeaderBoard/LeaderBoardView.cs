using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardView : MonoBehaviour
{
    public GameObject scoreCellPrefab;

    void Start()
    {
        StartCoroutine(LeaderBoardManager.GetHighScoreList(GetHightScoreCallBack));
    }

    public void GetHightScoreCallBack(List<UserData> datas)
    {
        for(int i = datas.Count - 1; i >= 0; i--)
        {
            GameObject scoreCell = Instantiate(scoreCellPrefab, transform);
            scoreCell.GetComponent<ScoreCell>().SetData(datas[i]);
        }
    }

    void Update()
    {
        
    }
}
