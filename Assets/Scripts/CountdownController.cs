using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

// Nowy interfejs dla klas, które chc¹ byæ zainicjalizowane po za³adowaniu sceny
public interface IInitializable
{
    void Initialize();
}

public class CountdownController : MonoBehaviour, IInitializable
{
    public TMP_Text countdownText;
    public float initialDelay = 1f;
    public float countdownDuration = 3f;
    public GameObject[] objectsToFreeze;

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartCoroutine(StartCountdown());
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Initialize();
    }

    public void Initialize()
    {
        FreezeOrUnfreezeObjects(true);
    }

    IEnumerator StartCountdown()
    {
        FreezeOrUnfreezeObjects(true);
        yield return new WaitForSecondsRealtime(initialDelay);

        float currentTime = countdownDuration;

        while (currentTime > 0)
        {
            countdownText.text = Mathf.CeilToInt(currentTime).ToString();
            yield return new WaitForSecondsRealtime(1f);
            currentTime -= 1f;
        }

        countdownText.text = "Go!";
        yield return new WaitForSecondsRealtime(1f);
        countdownText.gameObject.SetActive(false);

        FreezeOrUnfreezeObjects(false);
    }

    void FreezeOrUnfreezeObjects(bool freeze)
    {
        foreach (GameObject obj in objectsToFreeze)
        {
            if (obj != null)
            {
                Rigidbody[] rigidbodies = obj.GetComponentsInChildren<Rigidbody>();
                foreach (Rigidbody rb in rigidbodies)
                {
                    rb.isKinematic = freeze;
                }

                MonoBehaviour[] scripts = obj.GetComponentsInChildren<MonoBehaviour>();
                foreach (MonoBehaviour script in scripts)
                {
                    script.enabled = !freeze;
                }
            }
        }
    }
}
