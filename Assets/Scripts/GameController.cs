using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public static List<GameObject> unlockedMonsters; // The list that stores all the unlocked monsters
    private static bool created = false;

    private void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
    }

    private void Update()
    {
        /*if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            TouchDetection(Input.GetTouch(0).position);
        }*/

        if (Input.GetMouseButtonDown(0))
        {
            TouchDetection(Input.mousePosition);
        }

    }

    void TouchDetection(Vector2 touchPosition)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        if(Physics.Raycast(ray, out hit))
        {
            if(hit.collider.tag == "Nav")
            {
                Debug.Log("Going to " + hit.collider.name);
                ChangeScene(hit.collider.name);
            }
        }
        
    }

    public void PlayGame()
    {
        StartCoroutine(LoadGameAsync());
    }

    IEnumerator LoadGameAsync()
    {
        yield return new WaitForSeconds(1);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

    }

    public void ChangeScene(string sceneName)
    {
        if(sceneName == "ColorHunt" || sceneName == "Ranch") // Temporary while all the scenes are not created
        {
            SceneManager.LoadScene(sceneName);
        }
        
    }

}
