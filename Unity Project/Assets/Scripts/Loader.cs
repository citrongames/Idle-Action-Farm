using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    [SerializeField] private int _levelIndex;
    [SerializeField] private int _refreshRate;

    void Awake()
    {
        Application.targetFrameRate = _refreshRate;
    }

    private void Start() 
    {
        if (_levelIndex > 0)
            StartCoroutine(LoadLevel(_levelIndex));
    }
    IEnumerator LoadLevel(int levelIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelIndex);

        while (asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
