using UnityEngine;

namespace Vampire
{
    [System.Serializable]
    public class MonsterSpawnTable
    {
        public SpawnRateKeyframe[] spawnRateKeyframes;
        public SpawnChanceKeyframe[] spawnChanceKeyframes;
        public HPMultiplierKeyframe[] hpMultiplierKeyframes;

        public float GetSpawnRate(float t)
        {
            if (t == 0)
                return spawnRateKeyframes[0].spawnRate;
            for (int i = 1; i < spawnRateKeyframes.Length; i++)
            {
                if (spawnRateKeyframes[i].t >= t)
                    return Mathf.Lerp(spawnRateKeyframes[i-1].spawnRate, spawnRateKeyframes[i].spawnRate, Remap01(spawnRateKeyframes[i-1].t, spawnRateKeyframes[i].t, t));
            }
            return 0;
        }

        public (int, float) SelectMonsterWithHPMultiplier (float t)
        {
            if (t == 0)
            {
                int monsterIdx = SelectMonster(1, t);
                float hpBuff = hpMultiplierKeyframes.Length > 0 ? hpMultiplierKeyframes[0].healthBuffs[monsterIdx] : 0;
                return (monsterIdx, hpBuff); 
            }
            for (int i = 0; i < spawnChanceKeyframes.Length; i++)
            {
                if (spawnChanceKeyframes[i].t >= t)
                {
                    int monsterIdx = SelectMonster(i, t);
                    for (i = 0; i < hpMultiplierKeyframes.Length; i++)
                    {
                        if (hpMultiplierKeyframes[i].t >= t)
                        {
                            return (monsterIdx, GetHPBuff(i, monsterIdx, t));
                        }
                    }
                    return (-1, 0);
                }
            }
            return (-1, 0);
        }

        public int SelectMonster (float t)
        {
            if (t == 0)
                return SelectMonster(1, t);
            for (int i = 0; i < spawnChanceKeyframes.Length; i++)
            {
                if (spawnChanceKeyframes[i].t >= t)
                {
                    return SelectMonster(i, t);
                }
            }
            return -1;
        }

        private int SelectMonster(int i, float t)
        {
            float rand = Random.Range(0f, 1.0f);
            float cumulative = 0;
            float tLerp = Remap01(spawnChanceKeyframes[i-1].t, spawnChanceKeyframes[i].t, t);
            for (int j = 0; j < spawnChanceKeyframes[i-1].spawnChances.Length; j++)
            {
                cumulative += Mathf.Lerp(spawnChanceKeyframes[i-1].spawnChances[j], spawnChanceKeyframes[i].spawnChances[j], tLerp);
                if (rand < cumulative)
                    return j;
            }
            return -1;
        }

        private float GetHPBuff(int i, int j, float t)
        {
            float tLerp = Remap01(hpMultiplierKeyframes[i-1].t, hpMultiplierKeyframes[i].t, t);
            return Mathf.Lerp(hpMultiplierKeyframes[i-1].healthBuffs[j], hpMultiplierKeyframes[i].healthBuffs[j], tLerp);
        }

        private float Remap01(float min, float max, float t)
        {
            return (t - min) / (max - min);
        }

        [System.Serializable]
        public class SpawnRateKeyframe
        {
            public float t;
            public float spawnRate;
        }

        [System.Serializable]
        public class SpawnChanceKeyframe
        {
            public float t;
            public float[] spawnChances;
        }

        [System.Serializable]
        public class HPMultiplierKeyframe
        {
            public float t;
            public float[] healthBuffs;
        }
    }
}