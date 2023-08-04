using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
using DPR;
using UnityEngine.SceneManagement;
using TMPro;



public class LoginManager : MonoBehaviour
{

    [SerializeField] private GameObject LoginSection;
    [SerializeField] private TMP_InputField UsernameField;
    [SerializeField] private TMP_InputField PasswordField;
    [SerializeField] private TMP_Text debug;


    void Update()
    {
        if (LoginSection.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                LoginButton();
            }

            TabSwitching();
        }
    }

    private void TabSwitching()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (UsernameField.isFocused)
            {
                EventSystem.current.SetSelectedGameObject(PasswordField.gameObject, null);                
                PasswordField.OnPointerClick(new PointerEventData(EventSystem.current));
            }
            else 
            {
                EventSystem.current.SetSelectedGameObject(UsernameField.gameObject, null); 
                UsernameField.OnPointerClick(new PointerEventData(EventSystem.current));
            }
        }
    }



    public void SetUsername()
    {
        UsernameField.text = Regex.Replace(UsernameField.text, @"[^a-zA-Z0-9 -]", "");
        UsernameField.characterLimit = 16;
        UsernameField.text = UsernameField.text.Replace(" ", "");
    }

    public void SetPassword()
    {
        PasswordField.text = Regex.Replace(PasswordField.text, @"[^a-zA-Z0-9 -]", "");
        PasswordField.characterLimit = 32;
    }


    public void LoginButton()
    {
        if (string.IsNullOrEmpty(UsernameField.text) || string.IsNullOrEmpty(PasswordField.text))
        {
            Debug.Log("Please insert all the fields.");
            debug.text = "Please insert all the fields.";
            return;
        }

        if (UsernameField.text.Length < 3 || PasswordField.text.Length < 3)
        {
            Debug.Log("Username and Password must have at least 3 characters.");
            debug.text = "Username and Password must have at least 3 characters.";
            return;
        }
        Debug.Log("Processing request...\nPlease Stand By.");
        debug.text = "Processing request...\nPlease Stand By.";

        StartCoroutine("ProcessLoginRequest");

    }

    private IEnumerator ProcessLoginRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", UsernameField.text);
        form.AddField("password", PasswordField.text);

        UnityWebRequest www = UnityWebRequest.Post(WebServer.SERVER_NAME + "/login.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
             Debug.Log($"Server error, please retry later.\n\n" +
                $"{www.error}");

            debug.text = $"Server error, please retry later.\n\n" +
                $"{www.error}";

            yield return null;
        }
        else
        {
            if (Helper.GetData(www.downloadHandler.text, "MESSAGE:") == "Successfully logged in!\nLoading...")
            {
                debug.text = $"Successfully login!.";
                UserData.nickName = UsernameField.text;
                UserData.userID = int.Parse(Helper.GetData(www.downloadHandler.text, "ID:"));
                UserData.userName = Helper.GetData(www.downloadHandler.text, "USERNAME:");
                UserData.score = int.Parse(Helper.GetData(www.downloadHandler.text, "SCORE:"));
                UserData.coins = int.Parse(Helper.GetData(www.downloadHandler.text, "COINS:"));
                UserData.uziBullets = int.Parse(Helper.GetData(www.downloadHandler.text, "USZIBULLETS:"));
                UserData.shotgunBullets = int.Parse(Helper.GetData(www.downloadHandler.text, "SHOTGUNBULLETS:"));
                SceneManager.LoadScene("Loby");
            }
        }
    }

    public void EnableLoginSection()
    {
        LoginSection.SetActive(true);
    }

    public void DisableLoginSection()
    {
        LoginSection.SetActive(false);
    }
}
