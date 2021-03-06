﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class Inky : Enemy
    {
        protected override void ChaseMove()
        {
            PlayerLinearMovement plm = GameManager.Instance.Player.Movement;
            if (plm.CurrentNode != null && plm.TargetNode != null)
            {
                Vector3 vec = 2 * (plm.CurrentNode.transform.position - transform.position);
                ChaseWithVector(vec);
            }
            else
            {
                ChaseBlinky();
            }
        }
    }
}
