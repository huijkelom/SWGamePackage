using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Countdown : MonoBehaviour, I_SmartwallInteractable
{
    [SerializeField] private List<GameObject> Numbers = new List<GameObject>();
    [SerializeField] private Image I_HitMe;
    [SerializeField] private UnityEvent CountdownFinished = new UnityEvent();

    private bool started = false;

    public void Hit(Vector3 hitPosition, InputType inputType)
    {
        if (!started)
        {
            I_HitMe.enabled = false;
            StartCoroutine(CountDown());
            started = true;
        }
    }

    private IEnumerator CountDown()
    {
        foreach (GameObject go in Numbers)
        {
            go.SetActive(true);
            go.GetComponent<Animation>().Play();
            AudioManager.Instance.Play("CountdownBeep");
            yield return new WaitForSeconds(1);
            go.SetActive(false);
        }
      
        CountdownFinished.Invoke();
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        gameObject.GetComponent<Image>().enabled = false;
        AudioManager.Instance.Play("CountdownGo");
    }
}