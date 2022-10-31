using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitMenu : MonoBehaviour
{
    public void toMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScreen");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
    private bool escEnabled = false;
    public void Update()
    {

        if (Input.GetKeyUp(KeyCode.Escape) && escEnabled)
        {
            Time.timeScale = 1;
            //transform.gameObject.SetActive(false);
            transform.Find("Escape").gameObject.SetActive(false);
            transform.Find("QuitGame").gameObject.SetActive(false);
            escEnabled = false;
            Cursor.visible = false;
        }
        else if (Input.GetKeyUp(KeyCode.Escape) && !escEnabled)
        {

            Time.timeScale = 0;
            //transform.gameObject.SetActive(true);
            transform.Find("Escape").gameObject.SetActive(true);
            transform.Find("QuitGame").gameObject.SetActive(true);
            escEnabled = true;
            Debug.Log("Esc open");
            Cursor.visible = true;
        }
    }
}
