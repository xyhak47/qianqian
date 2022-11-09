using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceManager : MonoBehaviour
{
    public static VoiceManager Instance = null;
    VoiceManager()
    {
        Instance = this;
    }
}
