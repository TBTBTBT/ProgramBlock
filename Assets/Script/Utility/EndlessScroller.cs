using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EndlessScroller : UIBehaviour
{

    [System.Serializable]
    public class OnUpdate : UnityEngine.Events.UnityEvent<int, GameObject> { }
    public OnUpdate OnItemUpdate = new OnUpdate();

}
