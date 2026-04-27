using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakedownAnimatorEventController : MonoBehaviour
{
    public Takedown takedownController;
    public void KillCurrentTarget()
    {
        if (takedownController == null)
            return;

        takedownController.KillCurrentTarget();
    }

    public void EndTakedown()
    {
        if (takedownController == null)
            return;

        takedownController.EndTakedown();
    }
}
