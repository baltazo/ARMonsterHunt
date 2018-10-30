using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour {

    public static GameController sharedInstance = null;

    private WaitForSeconds timeToLoad;

    private void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (sharedInstance != this)
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        timeToLoad = new WaitForSeconds(1);
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
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        if(Physics.Raycast(ray, out hit))
        {
            if(hit.collider.tag == "Nav")
            {
                Debug.Log("Going to " + hit.collider.name);
                ChangeScene(hit.collider.name);
            }
            else if (hit.collider.tag == "Collection")
            {
                hit.transform.gameObject.GetComponent<MonsterCollection>().ShowCollection();
            }
            else if(hit.collider.tag == "Train")
            {
                hit.transform.gameObject.GetComponent<Training>().ShowMonsterList();
            }
        }
        
    }

    public void PlayGame()
    {
        StartCoroutine(LoadGameAsync());
    }

    IEnumerator LoadGameAsync()
    {
        yield return timeToLoad;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);

        while (!asyncLoad.isDone && LocalizationManager.sharedInstance.GetIsReady())
        {
            yield return null;
        }

    }

    public void ChangeScene(string sceneName)
    {
        if(sceneName == "ColorHunt" || sceneName == "Ranch" || sceneName == "MonsterViz") // Temporary while all the scenes are not created
        {
            SceneManager.LoadScene(sceneName);
        }
        
    }

}
