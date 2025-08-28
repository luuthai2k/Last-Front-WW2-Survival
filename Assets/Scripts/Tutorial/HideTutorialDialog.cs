using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideTutorialDialog : MonoBehaviour
{
    [SerializeField] private int dialogId = -1;//-2 mean unsubcribe all dialog
    // Start is called before the first frame update
    void OnEnable()
    {
        MessageManager.Instance.SendMessage(new Message(TeeMessageType.TutorialDialog, new object[] { dialogId }));
    }
}
