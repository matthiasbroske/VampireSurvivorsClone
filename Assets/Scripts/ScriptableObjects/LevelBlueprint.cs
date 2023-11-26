using System.Collections.Generic;
using UnityEngine;

namespace Vampire
{
    [CreateAssetMenu(fileName = "Level", menuName = "Blueprints/Level", order = 1)]
    public class LevelBlueprint : ScriptableObject
    {
        [Header("Time")]
        public float levelTime = 600;
        [Header("Background")]
        public Texture2D backgroundTexture;
        [Header("Abilities")]
        public GameObject[] abilityPrefabs;
        [Header("Monster Settings")]
        public MonstersContainer[] monsters;
        public MiniBossContainer[] miniBosses;
        public BossContainer finalBoss;
        public MonsterSpawnTable monsterSpawnTable;
        [Header("Chest Settings")]
        public ChestBlueprint chestBlueprint;
        public float chestSpawnDelay = 30;
        public float chestSpawnAmount = 2;
        [Header("Exp Gem Settings")]
        public int initialExpGemCount = 25;
        public GemType initialExpGemType = GemType.White1;

        private Dictionary<int, (int, int)> monsterIndexMap;
        public Dictionary<int, (int, int)> MonsterIndexMap 
        { 
            get
            {
                if (monsterIndexMap == null)
                {
                    monsterIndexMap = new Dictionary<int, (int, int)>();
                    int monsterIndex = 0;
                    for (int i = 0; i < monsters.Length; i++)
                    {
                        for (int j = 0; j < monsters[i].monsterBlueprints.Length; j++)
                            monsterIndexMap[monsterIndex++] = (i, j);
                    }
                }
                return monsterIndexMap;
            }
        }

        [System.Serializable]
        public class MonstersContainer
        {
            public GameObject monstersPrefab;
            public MonsterBlueprint[] monsterBlueprints;
        }

        [System.Serializable]
        public class MiniBossContainer : BossContainer
        {
            public float spawnTime;
        }

        [System.Serializable]
        public class BossContainer
        {
            public GameObject bossPrefab;
            public BossMonsterBlueprint bossBlueprint;
        }
    }
}
