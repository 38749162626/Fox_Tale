using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;
using System;
using UnityEngine.Events;

public class UserData
{
    public string UserName;
    public int score;
    public UserData(string UserName, int score)
    {
        this.UserName = UserName;
        this.score = score;
    }
}

public class LeaderBoardManager : MonoBehaviour
{
    private const string url = "http://dreamlo.com/lb/";
    private const string privateCode = "kgCvXpFzLk6n41EaZQBReQ1BV0Bd-3BUKLoY8Iywo5XQ";
    private const string publicCode = "69bbe1698f40bb2f60a68c10";

    private void Start()
    {

    }

    public static IEnumerator CreateNewHightScore(string userName, float score)
    {
        UnityWebRequest request = new UnityWebRequest(url + privateCode + "/add/" + UnityWebRequest.EscapeURL(userName) + "/" + score);
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log("Ìí¼Ó³É¹¦£¡");
        }
    }

    public static IEnumerator GetHighScoreList(UnityAction<List<UserData>> callBack)
    {
        UnityWebRequest request = UnityWebRequest.Get(url + publicCode + "/json");
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            JsonData data = JsonMapper.ToObject(request.downloadHandler.text);
            JsonData userDatas = data?["dreamlo"]?["leaderboard"]?["entry"];
            List<UserData> userDataList = new List<UserData>();
            if (userDatas != null && userDatas.IsArray)
            {
                foreach (JsonData user in userDatas)
                {
                    userDataList.Add(new UserData(user["name"].ToString(), int.Parse('-' + user["score"].ToString())));
                }
            }
            else if(userDatas != null)
            {
                userDataList.Add(new UserData(userDatas["name"].ToString(), int.Parse('-' + userDatas["score"].ToString())));
            }
            callBack(userDataList);
        }
    }
}
