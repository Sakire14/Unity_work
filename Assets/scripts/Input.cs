using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input : MonoBehaviour
{
    public int collumn;
    public GameManager gm;
    void OnMouseDown()
        {

        
        
          
            
        
        gm.Turn(collumn);
        }
        
}
