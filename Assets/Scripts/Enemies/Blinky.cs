using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class Blinky : Enemy
    {
        protected override void ChaseMove()
        {
            ChaseBlinky();
        }
    }
}
