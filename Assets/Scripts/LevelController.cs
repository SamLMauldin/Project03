using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] GameObject _witchTimePanel = null;
    AudioSource audioSource = null;
    [SerializeField] AudioClip witchTimeSFX;
    bool alreadyPlayed = false;
    // Start is called before the first frame update
    void Start()
    {
        _witchTimePanel.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Enemy.dodged)
        {
            if (!alreadyPlayed)
            {
                //audioSource.PlayOneShot(witchTimeSFX, audioSource.volume);
            }
            _witchTimePanel.SetActive(true);
        }
        else
        {
            alreadyPlayed = false;
            _witchTimePanel.SetActive(false);
        }
        
    }
}
