using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    public GameObject[] gameObjects;
    void Start()
    {
        StartCoroutine("LifeCycle");
    }

    IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(60f);
        foreach(var gameObject in gameObjects)
        {
            gameObject.SetActive(true);
        }
        gameObject.SetActive(false);
    }
}
