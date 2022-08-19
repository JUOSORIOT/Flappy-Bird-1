using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestApiManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private string URL_FirstPost;
    [SerializeField] private InputField _usernameInputField;
    [SerializeField] private InputField _passwordInputField;
    
    private string Token;
    private string Username;
    void Start()
    {
        Token = PlayerPrefs.GetString("token");
        Username = PlayerPrefs.GetString("username");
        StartCoroutine(GetProfile());
        Debug.Log(Token); }
    public void ClickGetScores()
    {

    }
    public void ClickSignUp()
    {
        string postData = GetInputData();
        StartCoroutine(SignUp(postData));
    }
    public void ClickLogIn()
    {
        string postData = GetInputData();
        StartCoroutine(LogIn(postData));
    }
    private string GetInputData()
    {
        AuthData dataPost = new AuthData();
        dataPost.username = _usernameInputField.text;
        dataPost.password = _passwordInputField.text;
        string postData = JsonUtility.ToJson(dataPost);
        return postData;
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
            Debug.Log(PlayerPrefs.GetInt("oldScore"));
            AuthData resData = JsonUtility.FromJson<AuthData>(www.downloadHandler.text);
            StartCoroutine(LogIn(postData));


        }
        else
        {
            Debug.Log(www.error);
            Debug.Log(www.downloadHandler.text);
        }
    }
    IEnumerator LogIn(string postData)
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
            Debug.Log(PlayerPrefs.GetInt("oldScore"));
            AuthData resData = JsonUtility.FromJson<AuthData>(www.downloadHandler.text);
            Debug.Log(resData.token);
            PlayerPrefs.SetString("token", resData.token);
            PlayerPrefs.SetString("username", resData.usuario.username);
            PlayerPrefs.SetInt("oldScore", resData.usuario.score);
            PlayerPrefs.Save();
            Debug.Log(resData.usuario.score);
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.Log(www.error);
            Debug.Log(www.downloadHandler.text);
        }
    }
    IEnumerator GetProfile()
    {
        string url = URL_FirstPost + "/api/usuarios/" + Username;
        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SetRequestHeader("x-token", Token);
        
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR " + www.error);
        }
        else if(www.responseCode == 200)
        {

            AuthData resData = JsonUtility.FromJson<AuthData>(www.downloadHandler.text);
            Debug.Log("Hola " + resData.usuario.score);
            PlayerPrefs.SetInt("oldScore", (int)resData.usuario.score);
            PlayerPrefs.Save();
            Debug.Log(PlayerPrefs.GetString("oldScore"));
            
            SceneManager.LoadScene(1);
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
    public string username;
    public int score;
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
    public string password;
    public bool estado;
    public int score;
}
