using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class Clide : Enemy
    {
        protected override void ChaseMove()
        {
            if (Vector3.Distance(transform.position, player.transform.position) > ClideRange)
            {
                ChaseBlinky();
            }
            else
            {
                ScatterMove();
            }
        }
    }
}
