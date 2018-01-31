using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public void SetVolume(float val)
    {
        GetComponent<AudioSource>().volume = val;
    }
}
