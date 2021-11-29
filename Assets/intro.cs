using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class intro : MonoBehaviour
{
    
public string sceneToLoad;
    AsyncOperation loadingOperation;
    //Slider progressBar;



    IEnumerator Start()
    {
        loadingOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        loadingOperation.allowSceneActivation = false;
        yield return new WaitForSeconds(2);
        loadingOperation.allowSceneActivation = true;
    }
    void Update()
    {
       //progressBar.value = Mathf.Clamp01(loadingOperation.progress / 0.9f);
    }


}