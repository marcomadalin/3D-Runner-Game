using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class feetLogic : MonoBehaviour
{
    public playerController pc;

    private void OnTriggerStay(Collider obj)
    {
        pc.canJump = true;
    }

    private void OnTriggerExit(Collider obj)
    {
        pc.canJump = false;
    }
}
