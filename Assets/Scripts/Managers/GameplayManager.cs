using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public GameObject player;
    public GameObject roarSound;
    public GameObject monster;
    private Animator animator;
    private SceneController sceneManager;

    void Start()
    {
        sceneManager = FindObjectOfType<SceneController>();
    }

    public void GameWin() {
        SceneManager.LoadScene("Credits");
    }

    public void GameLose() {
        animator = monster.GetComponent<Animator>();
        player.transform.position = new Vector3(0, 100, 0);
        player.transform.eulerAngles = new Vector3(0, 0, 0);
        animator.SetBool("isRoaring", true);
        StartCoroutine(MiniWaiter());
        Instantiate(roarSound, player.transform.position, Quaternion.identity);
        StartCoroutine(Waiter());
        SceneManager.LoadScene("Credits");
    }

    IEnumerator Waiter() {
        yield return new WaitForSeconds(3);
    }

    IEnumerator MiniWaiter() {
        yield return new WaitForSeconds(0.4f);
    }
}
