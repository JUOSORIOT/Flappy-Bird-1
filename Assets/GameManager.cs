using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverCanvas;
    [SerializeField] private string URL_FirstPost;
    [SerializeField] private Text TableData;
    [SerializeField] public GameObject TableScores;
    

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(PlayerPrefs.GetInt("oldScore"));
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        if (PlayerPrefs.GetInt("oldScore") < Score.score)
        {
            StartCoroutine(SetScores(GetScoreData(Score.score)));
        }
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0;
    }
    public void Replay()
    {
        SceneManager.LoadScene(1);
    }
    private string GetScoreData(int score)
    {
        Puntaje dataPost = new Puntaje();
        dataPost.username = PlayerPrefs.GetString("username");
        dataPost.score = score;
        string postData = JsonUtility.ToJson(dataPost);
        return postData;
    }

    public void ClickScores()
    {
        StartCoroutine(GetScores());
    }

    // Update is called once per frame
    IEnumerator SetScores(string postData)
    {
        string url = URL_FirstPost;
        UnityWebRequest www = UnityWebRequest.Put(url,postData);
        www.method = "PATCH";
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("x-token", PlayerPrefs.GetString("token"));
        yield return www.SendWebRequest();
        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR " + www.error);
        }
        else if(www.responseCode == 200)
        {
            AuthData resData = JsonUtility.FromJson<AuthData>(www.downloadHandler.text);
            Debug.Log("Actualizado con exito");
        }
        else
        {
            Debug.Log(www.error);
            Debug.Log(www.downloadHandler.text);
        }
    }
    IEnumerator GetScores()
    {
        string url = URL_FirstPost + "?limit=5&sort=true";
        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SetRequestHeader("x-token", PlayerPrefs.GetString("token"));
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR " + www.error);
        }
        else if(www.responseCode == 200)
        {
            TableScores.SetActive(true);
            ScoresData resData = JsonUtility.FromJson<ScoresData>(www.downloadHandler.text);

            foreach (User score in resData.usuarios)
            {
                TableData.text += score.username + ":  " + score.score + "\n";
                Debug.Log(score.username + " | " + score.score);
            }
        }
        else
        {
            Debug.Log(www.error);
        }
    }
}
[System.Serializable] 
public class User
{
    public string _id;
    public string username;
    public string password;
    public bool estado;
    public int score;
}
[System.Serializable]
public class ScoresData
{
    public User[] usuarios;
}
