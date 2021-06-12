using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameRoot : MonoBehaviour
{
   public bool run = false;

   public float deltaTime { get{ return run?Time.deltaTime:0f; }}
}
