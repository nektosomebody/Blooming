using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ScoreSystem
{
    public class Class1: MonoBehaviour
    {
        DatabaseConnection connection;
        private void Awake()
        {
            connection = new();
            connection.AddNewScore("ige1", 120);
        }

    }
}
