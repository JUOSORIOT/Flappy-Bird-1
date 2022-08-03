using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RestApiManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private string URL;
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
            Debug.Log(www.downloadHandler.text);
            ScoresData resData = JsonUtility.FromJson<ScoresData>(www.downloadHandler.text);

            foreach (Puntaje score in resData.scores)
            {
                Debug.Log(score.user_id + " | " + score.registro);
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
    public int user_id;
    public int registro;
}
[System.Serializable]
public class ScoresData
{
    public Puntaje[] scores;
}
