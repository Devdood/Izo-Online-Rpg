using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRegistration : MonoBehaviour
{
    [SerializeField]
    private InputField userField, passField1, passField2, emailField;

    [SerializeField]
    private Button registerButton;

    private void Awake()
    {
        registerButton.onClick.AddListener(Register);
    }

    public void Register()
    {
        Connection.Instance.SendData(new RegisterPacket(new RegisterData()
        {
            username = userField.text,
            pass1 = passField1.text,
            pass2 = passField2.text,
            email = emailField.text,
        }));
    }
}

public class RegisterData
{
    public string username, pass1, pass2, email;
}