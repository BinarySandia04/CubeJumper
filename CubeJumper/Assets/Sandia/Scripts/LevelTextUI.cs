using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTextUI : MonoBehaviour
{
    TextMeshProUGUI text;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        StartCoroutine(showLevelText());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(showLevelText());
        }
    }

    IEnumerator showLevelText()
    {
        string name = SceneManager.GetActiveScene().name;
        text.text = name;
        yield return new WaitForSecondsRealtime(2);
        text.text = "";
        yield return null;
    }
}
