using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CallSceneButton : MonoBehaviour
{
    [SerializeField] string sceneName;
    private Button m_button;

    void OnEnable()
    {
        m_button = this.GetComponent<Button>();
        m_button.onClick.AddListener(ReturnToMenu);
    }

    void OnDisable()
    {
        m_button.onClick.RemoveAllListeners();
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene(sceneName);
    }
}
