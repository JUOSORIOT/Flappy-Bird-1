using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RestApiManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private string URL;
    [SerializeField] private Text TableData;
    [SerializeField] public GameObject TableScores;
    void Start()
    {
        
    }


    public void ClickGetScores()
    {
        StartCoroutine(GetScores());
    }

    IEnumerator GetScores()
    {
        string url = URL + "/scores";
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR " + www.error);
        }
        else if(www.responseCode == 200)
        {
            TableScores.SetActive(true);
            Debug.Log(www.downloadHandler.text);
            ScoresData resData = JsonUtility.FromJson<ScoresData>(www.downloadHandler.text);

            foreach (Puntaje data in resData.scores)
            {
                Debug.Log(data.user_name + " | " + data.score);
                TableData.text += data.user_name + ":  " + data.score + "\n";
            }
        }
        else
        {
            Debug.Log(www.error);
        }
    }
}

[System.Serializable] 
public class Puntaje
{
    public string user_name;
    public int score;
}
[System.Serializable]
public class ScoresData
{
    public Puntaje[] scores;
}
