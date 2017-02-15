using UnityEngine;
using System.Collections;

public class PlayAniOnTimer : MonoBehaviour {

    public float gap;
    public float delayOnStart;
    public bool isPlayOnStart;
    public Transform go;
    private ImageAni _ani;

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
        if(_ani == null)
        {
            _ani = transform.GetComponent<ImageAni>();
        }

        StartCoroutine(playOnDelay());
    }

    IEnumerator playOnDelay()
    {
        yield return new WaitForSeconds(delayOnStart);
        _ani.play();
        StartCoroutine(playOnTimer());
    }
	
	IEnumerator playOnTimer()
    {
        while(true)
        {
            yield return new WaitForSeconds(gap);
           
            _ani.play();
        }
    }

    public void stop()
    {
        StopAllCoroutines();
    }
}
