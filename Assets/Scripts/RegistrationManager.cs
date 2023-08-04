using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
using DPR;
using TMPro;

public class RegistrationManager : MonoBehaviour
{
    [SerializeField] private GameObject registrationSection;
    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private TMP_InputField confirmPasswordField;

    [SerializeField] private TMP_Text debug;


    void Start()
    {
        registrationSection.SetActive(false);
    }

    void Update()
    {

        if (registrationSection.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                RegisterButton();
            }

            TabSwitching();
        }

    }


    private void TabSwitching()
    {    
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (usernameField.isFocused)
            {
                EventSystem.current.SetSelectedGameObject(passwordField.gameObject, null);
                passwordField.OnPointerClick(new PointerEventData(EventSystem.current));
            }
            else 
            {
                EventSystem.current.SetSelectedGameObject(usernameField.gameObject, null);
                usernameField.OnPointerClick(new PointerEventData(EventSystem.current));
            }
        }
    }

    public void SetUsername()
    {
        usernameField.text = Regex.Replace(usernameField.text, @"[^a-zA-Z0-9 -]", "");
        usernameField.characterLimit = 16;
        usernameField.text = usernameField.text.Replace(" ", "");
    }

    public void SetPassword()
    {
        passwordField.text = Regex.Replace(passwordField.text, @"[^a-zA-Z0-9 -]", "");
        passwordField.characterLimit = 32;
    }

    public void SetConfirmPassword()
    {
        confirmPasswordField.text = Regex.Replace(confirmPasswordField.text, @"[^a-zA-Z0-9 -]", "");
        confirmPasswordField.characterLimit = 32;
    }

    public void RegisterButton()
    {
        #region Errors Check
        if (string.IsNullOrEmpty(usernameField.text) ||
            string.IsNullOrEmpty(passwordField.text) || string.IsNullOrEmpty(confirmPasswordField.text))
        {
            Debug.Log("Please insert all the fields.");
            debug.text = "Please insert all the fields.";
            return;
        }

        if (passwordField.text != confirmPasswordField.text)
        {
            Debug.Log("Passwords does not match");
            debug.text = "Passwords does not match";
            return;
        }

        if (passwordField.text == usernameField.text)
        {
            Debug.Log("The password cannot be the same as the Username");
            debug.text = "The password cannot be the same as the Username";
            return;
        }

        if (usernameField.text.Length < 3 || passwordField.text.Length < 3)
        {
            Debug.Log("Username and Password must have at least 3 characters.");
            debug.text = "Username and Password must have at least 3 characters.";
            return;
        }

        #endregion

        Debug.Log("Processing request...\nPlease Stand By.");
        debug.text = "Processing request...\nPlease Stand By.";

        StartCoroutine("ProcessRegistrationRequest");

    }


    private IEnumerator ProcessRegistrationRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", usernameField.text);
        form.AddField("password", passwordField.text);

        UnityWebRequest www  = UnityWebRequest.Post(WebServer.SERVER_NAME + "/register.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Server error, please retry later.\n" +
                $"--> {www.error} <--");
            debug.text = "Server error, please retry later.";
            yield return null;
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            debug.text = www.downloadHandler.text;
        }

    }


    public void EnableRegistrationSection()
    {
        registrationSection.SetActive(true);
    }

    public void DisableRegistrationSection()
    {
        registrationSection.SetActive(false);
    }


}
