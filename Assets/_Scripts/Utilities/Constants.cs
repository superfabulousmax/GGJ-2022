using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class Constants
    {
        public const string PlayerTag = "Player";
        public const string PrimariesFolder = "Abilities/Primaries/";
        public const string SecondariesFolder = "Abilities/Secondaries/";
        public const string EnemiesFolder = "Prefabs/Enemies/";
        public const int SecondaryThreshold = 10;
        public const int MaxEnemies = 128;
    }

    public enum Elements { Fire, Water, Air, Earth }
}