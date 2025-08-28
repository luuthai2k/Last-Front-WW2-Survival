using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CallbackWhenClose : MonoBehaviour
{
    public Action callBackWhenClose = null;
    public void SetActionAfterClose(Action callBackWhenClose)
    {
        this.callBackWhenClose = callBackWhenClose;
    }

    private void OnDisable()
    {
        if (callBackWhenClose != null)
            callBackWhenClose.Invoke();
    }
}
