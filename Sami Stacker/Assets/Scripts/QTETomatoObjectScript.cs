using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTETomatoObjectScript : MonoBehaviour
{
    private GameObject theObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        theObject = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        theObject = null;
    }

    void Update()
    {
        QTETomatoAttack parent = GetComponentInParent<QTETomatoAttack>();

        if (Input.anyKeyDown)
            if (theObject != null)
            {
                Debug.Log("Hit the object");
                parent.SFX.PlayOneShot(parent.QTEhit);
                Destroy(theObject);
            }
            else
            {
                parent.completedQTE = false;
                parent.SFX.PlayOneShot(parent.QTEmiss);
                Debug.Log("Missed the orb");
            }
    }
}
