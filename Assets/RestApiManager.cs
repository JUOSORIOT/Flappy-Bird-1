using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RestApiManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private string URL_FalseApi;
    [SerializeField] private string URL_FirstPost;
    [SerializeField] private Text TableData;
    [SerializeField] public GameObject TableScores;
    [SerializeField] private InputField _usernameInputField;
    [SerializeField] private InputField _passwordInputField;

    void Start()
    {
        
    }


    public void ClickGetScores()
    {
        StartCoroutine(GetScores());
    }
    public void ClickSignUp()
    {
        AuthData dataPost = new AuthData();
        dataPost.username = _usernameInputField.text;
        dataPost.password = _passwordInputField.text;
        string postData = JsonUtility.ToJson(dataPost);

        StartCoroutine(SignUp(postData));
    }
    public void ClickSignIn()
    {
        AuthData dataPost = new AuthData();
        dataPost.username = _usernameInputField.text;
        dataPost.password = _passwordInputField.text;
        string postData = JsonUtility.ToJson(dataPost);

        StartCoroutine(SignIn(postData));
    }

    IEnumerator GetScores()
    {
        string url = URL_FalseApi + "/scores";
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
    IEnumerator SignUp(string postData)
    {
        
        string url = URL_FirstPost + "/api/usuarios";
        UnityWebRequest www = UnityWebRequest.Put(url,postData);
        www.method = "POST";
        www.SetRequestHeader("Content-Type", "application/json");
        
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR " + www.error);
        }
        else if(www.responseCode == 200)
        {
          
            //Debug.Log(www.downloadHandler.text);
            AuthData resData = JsonUtility.FromJson<AuthData>(www.downloadHandler.text);
            Debug.Log("Bienvenido " + resData.usuario.username + ", id:" + resData.usuario._id);
            
            //StartCoroutine(LogIn()postData)

        }
        else
        {
            Debug.Log(www.error);
            Debug.Log(www.downloadHandler.text);
        }
    }
    IEnumerator SignIn(string postData)
    {
        
        string url = URL_FirstPost + "/api/auth/login";
        UnityWebRequest www = UnityWebRequest.Put(url,postData);
        www.method = "POST";
        www.SetRequestHeader("Content-Type", "application/json");
        
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR " + www.error);
        }
        else if(www.responseCode == 200)
        {
          
            //Debug.Log(www.downloadHandler.text);
            AuthData resData = JsonUtility.FromJson<AuthData>(www.downloadHandler.text);
            
            Debug.Log(resData.token);
            PlayerPrefs.SetString("token",resData.token);
            PlayerPrefs.Save();
            //StartCoroutine(LogIn()postData)
            //PlayerPrefs("token", resData.token);

        }
        else
        {
            Debug.Log(www.error);
            Debug.Log(www.downloadHandler.text);
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
[System.Serializable]
public class AuthData
{
    public string username;
    public string password;
    public UserData usuario;
    public string token;
}
[System.Serializable]
public class UserData
{
    public string _id;
    public string username;
    public bool estado;
    public int score;
}
