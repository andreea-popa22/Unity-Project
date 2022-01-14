using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    public Vector3 crashPosition;
    public bool isTransitioning = false;
    private bool collisionDisabled = false;
    private bool arrivedAtFinish = false;

    private float manaPerPickable = 25f;
    public GameObject ui;
    public Canvas menu;
    
    public PlayerController pc;

    public void Start()
    {
        // pc = GameObject.Find("Player").GetComponent<PlayerController>();
        // int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // ui = GameObject.Find("UI");
        // menu = ui.transform.Find("Start Canvas").GetComponent<Canvas>();
        // if (currentSceneIndex == 0)
        // {
        //     menu.gameObject.SetActive(true);
        // }
        
           
    }

    private void handleObject(GameObject gameObject)
    {
        //if (isTransitioning || collisionDisabled) { return; }

        switch (gameObject.tag)
        {
            case "Pickable":
                PlayerScore playerScore = GameObject.Find("Player").GetComponent<PlayerScore>();
                playerScore.itemsPicked += 1;
                gameObject.SetActive(false);
                SetMana(GetMana() + manaPerPickable);
                break;
            case "Fin":
                arrivedAtFinish = true;
                LoadNextLevel();
                UnloadPreviousLevel();
                break;
            case "Friendly":
                GameObject.Find("Player").GetComponent<PlayerController>().inAir = false;
                break;
            default:
                StartCrashSequence();
                break;
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        handleObject(other.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        handleObject(other.gameObject);
    }
    

    public void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    
    void StartCrashSequence()
    {
        float damage = IsInvincibile() ? 0 : 25f;
        crashPosition = transform.position;
        gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);      // Decrease health
        transform.position += Vector3.back * 3;                          // Move player back
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        
        if (nextSceneIndex == 3)
        {
            nextSceneIndex = 0;
        }
        
        if (nextSceneIndex == 0)
        {
            SceneManager.LoadScene("TotalScene");
        }
        if (nextSceneIndex == 1)
        {
            SceneManager.LoadScene("TotalScene1");
        }
        if (nextSceneIndex == 2)
        {
            SceneManager.LoadScene("TotalScene2");
        }

        // if (nextSceneIndex != 0)
        // {
        //     pc.enabled = true;
        // }
        
        

        // Debug.Log("sceneManager.sceneCountInBuildSettings");
        // Debug.Log(SceneManager.sceneCountInBuildSettings);
        // Debug.Log("currentSceneIndexs");
        // Debug.Log(currentSceneIndex);
        // Debug.Log("nextSceneIndex");
        // Debug.Log(nextSceneIndex);
    }

    void UnloadPreviousLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    
        if (currentSceneIndex == 1)
            {
                if(SceneManager.GetSceneByName("TotalScene").isLoaded)
                {
                    SceneManager.UnloadSceneAsync("TotalScene");
                }
            }

        if (currentSceneIndex == 2)
        {
            if(SceneManager.GetSceneByName("TotalScene1").isLoaded)
            {
                SceneManager.UnloadSceneAsync("TotalScene1");
            }

        }
        
        if (currentSceneIndex == 0)
        {
            if(SceneManager.GetSceneByName("TotalScene2").isLoaded)
            {
                SceneManager.UnloadSceneAsync("TotalScene2");
            }

        }
        
    }

    public float GetMana()
    {
        return gameObject.GetComponent<PlayerMana>().GetMana();
    }

    public void SetMana(float mana)
    {
        gameObject.GetComponent<PlayerMana>().SetMana(mana);
    }

    public bool IsInvincibile()
    {
        return gameObject.GetComponent<PlayerController>().isInvincibile();
    }
}
