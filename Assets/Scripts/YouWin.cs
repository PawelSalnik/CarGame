using UnityEngine;
using UnityEngine.UI;

public class YouWin : MonoBehaviour
{
    public GameObject CanvasToActivate;
    public GameObject CanvasToDeactivate;
    public GameObject[] objectsToActivateOnWin;
    public GameObject[] objectsToDeactivateOnWin;

    public Text sourceTimeLabel; // Etykieta z której pobieramy czas
    public Text destinationTimeLabel; // Etykieta do której przekazujemy czas

    private bool raceCompleted = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Trigger") && !raceCompleted)
        {
            raceCompleted = true;
            CanvasToActivate.SetActive(true);
            CanvasToDeactivate.SetActive(false);

            ActivateObjectsOnWin();
            DeactivateObjectsOnWin();

            // Pobierz czas z jednej etykiety i przekszta³æ na tekst
            string raceTimeText = sourceTimeLabel != null ? sourceTimeLabel.text : "0.00s";

            // Przypisz czas do drugiej etykiety
            if (destinationTimeLabel != null)
            {
                destinationTimeLabel.text = "Time: " + raceTimeText;
            }
        }
    }

    private void ActivateObjectsOnWin()
    {
        foreach (GameObject obj in objectsToActivateOnWin)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }

    private void DeactivateObjectsOnWin()
    {
        foreach (GameObject obj in objectsToDeactivateOnWin)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
}
