using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CloseGame : MonoBehaviour
{
    private Button m_button;

    void OnEnable()
    {
        m_button = this.GetComponent<Button>();
        m_button.onClick.AddListener(CloseTheGame);
    }

    void OnDisable()
    {
        m_button.onClick.RemoveAllListeners();
    }

    private void CloseTheGame()
    {
        Application.Quit();
    }
}
