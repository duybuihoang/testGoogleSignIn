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
        // Tìm FirebaseAuthManager trong scene
        authManager = FindFirstObjectByType<Authens>();

        // Thêm listener cho button
        registerButton.onClick.AddListener(OnRegisterButtonClick);
        signinButton.onClick.AddListener(OnSigninButtonClick);
    }

    public void OnRegisterButtonClick()
    {
        // ?n thông báo c? (n?u có)
        //messageText.text = "";

        // L?y email và password t? input fields
        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        // Ki?m tra email và password có h?p l? không
        if (string.IsNullOrEmpty(email))
        {
            //messageText.text = "Vui lòng nh?p email";
            return;
        }

        if (string.IsNullOrEmpty(password))
        {
            //messageText.text = "Vui lòng nh?p m?t kh?u";
            return;
        }

        if (password.Length < 6)
        {
            //messageText.text = "M?t kh?u ph?i có ít nh?t 6 ký t?";
            return;
        }

        // Disable button trong khi ?ang x? lý
        registerButton.interactable = false;
        //messageText.text = "?ang ??ng ký...";

        // G?i hàm ??ng ký t? FirebaseAuthManager
        try
        {
            authManager.RegisterUser(email, password);
            // Hi?n th? thông báo thành công
            //messageText.text = "??ng ký thành công!";

            // Xóa n?i dung các input fields
            emailInput.text = "";
            passwordInput.text = "";
        }
        catch (System.Exception e)
        {
            //messageText.text = "Có l?i x?y ra: " + e.Message;
        }
        finally
        {
            // Enable l?i button
            registerButton.interactable = true;
        }
    }

    public void OnSigninButtonClick()
    {
        // ?n thông báo c? (n?u có)
        //messageText.text = "";

        // L?y email và password t? input fields
        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        // Ki?m tra email và password có h?p l? không
        if (string.IsNullOrEmpty(email))
        {
            //messageText.text = "Vui lòng nh?p email";
            return;
        }

        if (string.IsNullOrEmpty(password))
        {
            //messageText.text = "Vui lòng nh?p m?t kh?u";
            return;
        }

        if (password.Length < 6)
        {
            //messageText.text = "M?t kh?u ph?i có ít nh?t 6 ký t?";
            return;
        }

        // Disable button trong khi ?ang x? lý
        signinButton.interactable = false;
        //messageText.text = "?ang ??ng ký...";

        // G?i hàm ??ng ký t? FirebaseAuthManager
        try
        {
            authManager.LoginUser(email, password);
            // Hi?n th? thông báo thành công
            //messageText.text = "??ng ký thành công!";

            // Xóa n?i dung các input fields
            emailInput.text = "";
            passwordInput.text = "";
        }
        catch (System.Exception e)
        {
            //messageText.text = "Có l?i x?y ra: " + e.Message;
        }
        finally
        {
            // Enable l?i button
            signinButton.interactable = true;
        }
    }



}
