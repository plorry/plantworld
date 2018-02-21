using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerUnit : Unit {
    protected override void UpdateStatus () {
         if (hitPoints <= 0) Capture();
    }   
}