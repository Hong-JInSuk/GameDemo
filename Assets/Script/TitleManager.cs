using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TitleManager : MonoBehaviour
{

    public Button btnGameStart;
    public Button btnGameClose;

    void Start()
    {
        btnGameStart.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Main");
            Debug.Log("Main �� �ε�");
        });

        btnGameClose.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}