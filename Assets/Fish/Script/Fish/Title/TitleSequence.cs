using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSequence : SingletonMonoBehaviour<TitleSequence> {
    public void PushStart(){
        FaderBehaviour.Fade("Home");
    }
}
