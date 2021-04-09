using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai : MonoBehaviour
{
    public GameManager gm;
    public void TaskOnClick()
    {

        gm.Turn_AI();
    }
}
