using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorRelay : MonoBehaviour
{
    // Start is called before the first frame update
    public EnemyController enemy;
    public void EnableMaceCollider()
    {
        if (enemy == null)
            return;

        enemy.EnableMace();
    }

    public void DisableMaceCollider()
    {
        if (enemy == null)
            return;

        enemy.DisableMace();
    }

}
