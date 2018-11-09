using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour {

    public static GameController sharedInstance = null;

    public MonsterManageScreen monsterManage;

    private WaitForSeconds timeToLoad;
    private int fingerID = -1;

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

        #if !UNITY_EDITOR
            fingerID = 0; 
        #endif

    }

    private void Start()
    {
        timeToLoad = new WaitForSeconds(1);

    }

    private void Update()
    {
#if !UNITY_EDITOR
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            TouchDetection(Input.GetTouch(0).position);
            return;
        }
#endif
        if (Input.GetMouseButtonDown(0))
        {
            TouchDetection(Input.mousePosition);
        }

    }

    void TouchDetection(Vector2 touchPosition)
    {
        if (EventSystem.current.IsPointerOverGameObject(fingerID))
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
                //hit.transform.gameObject.GetComponent<Training>().ShowMonsterList();
                monsterManage.ShowSelectedList("Training");
            }
            else if (hit.collider.tag == "Breed")
            {
                //hit.transform.gameObject.GetComponent<Training>().ShowMonsterList();
                monsterManage.ShowSelectedList("Breeding");
            }
            else if (hit.collider.tag == "Fight")
            {
                monsterManage.ShowSelectedList("Fighting");
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
        
            SceneManager.LoadScene(sceneName);
        
    }

}
