using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static System.Net.WebRequestMethods;

public class JSON : MonoBehaviour
{
    public InputField emailField;
    public InputField passwordField;
    public InputField rePasswordField;
    [SerializeField] public Button submitButton;
    
    private string apiUrl = "https://binusgat.rf.gd/unity-api-test/api/auth/signup.php";

    void Start()
    {
        submitButton.onClick.AddListener(OnSubmit);
    }

    private void OnSubmit()
    {
        string email = emailField.text;
        string password = passwordField.text;
        string rePassword = rePasswordField.text;

        
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(rePassword))
        {
            Debug.LogError("All fields must be filled out.");
            return;
        }

        if (password != rePassword)
        {
            Debug.LogError("Passwords do not match.");
            return;
        }

        
        SignUpData signUpData = new SignUpData
        {
            email = email,
            password = password
        };

        
    }

    
    private void SendDataToDatabase(string jsonData)
    {
        StartCoroutine(PostData(jsonData));
    }

    
    private IEnumerator PostData(string jsonData)
    {
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Data successfully sent to database.");
            CheckDataInserted();
        }
        else
        {
            Debug.LogError("Failed to send data: " + request.error);
        }
    }

    
    private void CheckDataInserted()
    {
        string checkUrl = "https://binusgat.rf.gd/unity-api-test/api/auth/signup.php";
        StartCoroutine(CheckDataRequest(checkUrl));
    }

    private IEnumerator CheckDataRequest(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Data check response: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Failed to check data: " + request.error);
        }
    }
}


public class SignUpData
{
    public string email;
    public string password;
    public string enterAgain;
}

