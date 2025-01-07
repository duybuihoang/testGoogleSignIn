using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterButton : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField emailInput;

    [SerializeField]
    private TMP_InputField passwordInput; 

    [SerializeField]
    private Button registerButton;

    [SerializeField]
    private Button signinButton;

    [SerializeField]
    private TextMeshProUGUI messageText; 

    private Authens authManager;

    void Start()
    {
        // T�m FirebaseAuthManager trong scene
        authManager = FindFirstObjectByType<Authens>();

        // Th�m listener cho button
        registerButton.onClick.AddListener(OnRegisterButtonClick);
        signinButton.onClick.AddListener(OnSigninButtonClick);
    }

    public void OnRegisterButtonClick()
    {
        // ?n th�ng b�o c? (n?u c�)
        //messageText.text = "";

        // L?y email v� password t? input fields
        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        // Ki?m tra email v� password c� h?p l? kh�ng
        if (string.IsNullOrEmpty(email))
        {
            //messageText.text = "Vui l�ng nh?p email";
            return;
        }

        if (string.IsNullOrEmpty(password))
        {
            //messageText.text = "Vui l�ng nh?p m?t kh?u";
            return;
        }

        if (password.Length < 6)
        {
            //messageText.text = "M?t kh?u ph?i c� �t nh?t 6 k� t?";
            return;
        }

        // Disable button trong khi ?ang x? l�
        registerButton.interactable = false;
        //messageText.text = "?ang ??ng k�...";

        // G?i h�m ??ng k� t? FirebaseAuthManager
        try
        {
            authManager.RegisterUser(email, password);
            // Hi?n th? th�ng b�o th�nh c�ng
            //messageText.text = "??ng k� th�nh c�ng!";

            // X�a n?i dung c�c input fields
            emailInput.text = "";
            passwordInput.text = "";
        }
        catch (System.Exception e)
        {
            //messageText.text = "C� l?i x?y ra: " + e.Message;
        }
        finally
        {
            // Enable l?i button
            registerButton.interactable = true;
        }
    }

    public void OnSigninButtonClick()
    {
        // ?n th�ng b�o c? (n?u c�)
        //messageText.text = "";

        // L?y email v� password t? input fields
        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        // Ki?m tra email v� password c� h?p l? kh�ng
        if (string.IsNullOrEmpty(email))
        {
            //messageText.text = "Vui l�ng nh?p email";
            return;
        }

        if (string.IsNullOrEmpty(password))
        {
            //messageText.text = "Vui l�ng nh?p m?t kh?u";
            return;
        }

        if (password.Length < 6)
        {
            //messageText.text = "M?t kh?u ph?i c� �t nh?t 6 k� t?";
            return;
        }

        // Disable button trong khi ?ang x? l�
        signinButton.interactable = false;
        //messageText.text = "?ang ??ng k�...";

        // G?i h�m ??ng k� t? FirebaseAuthManager
        try
        {
            authManager.LoginUser(email, password);
            // Hi?n th? th�ng b�o th�nh c�ng
            //messageText.text = "??ng k� th�nh c�ng!";

            // X�a n?i dung c�c input fields
            emailInput.text = "";
            passwordInput.text = "";
        }
        catch (System.Exception e)
        {
            //messageText.text = "C� l?i x?y ra: " + e.Message;
        }
        finally
        {
            // Enable l?i button
            signinButton.interactable = true;
        }
    }



}
