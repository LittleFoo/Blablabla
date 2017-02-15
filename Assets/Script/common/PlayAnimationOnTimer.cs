using UnityEngine;
using System.Collections;

public class PlayAnimationOnTimer : MonoBehaviour {

    public float gap;
    public float delayOnStart;
    public bool isPlayOnStart;
    public Transform go;
    private Animator ani;
    
    void Awake()
    {
        go = transform;
    }
    
    void Start () {
        if(isPlayOnStart)
            startPlay();
    }
    
    public void startPlay()
    {
        stop();
        go.gameObject.SetActive(true);

        if(ani == null)
        {
            ani = transform.GetComponent<Animator>();
        }
        StartCoroutine(playOnDelay());
    }
    
    IEnumerator playOnDelay()
    {
        yield return new WaitForSeconds(delayOnStart);
        ani.SetBool("isPlaying", true);
    }
    
    IEnumerator playOnTimer()
    {
            yield return new WaitForSeconds(gap);
            ani.SetBool("isPlaying", true);
    }

    public void pause()
    {
        ani.SetBool("isPlaying", false);
        StartCoroutine(playOnTimer());
    }
    
    public void stop()
    {
        StopAllCoroutines();
    }
}
