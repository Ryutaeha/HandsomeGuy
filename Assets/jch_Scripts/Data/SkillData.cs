using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Data{
    [System.Serializable]
    public class Skill
    {
        public string skillName;
        public string description;
        public string skillType;
        public float revivalHealthRatio;
        public float increamentDamageRatio;
        public float reductionDamageRatio;
        public float increamentMoveSpeedRatio;
        public float increamentSightRange;
        public float coolTime;
        public float duration;
        public float increamentAttackSpeedRatio;
    }

    

    [System.Serializable]
    public class SkillData : ILoader<string, Skill>
    {
        public List<Skill> skills = new List<Skill>();
        public Dictionary<string, Skill> MakeDictionary()
        {
            Dictionary<string, Skill> skillDict = new Dictionary<string, Skill>();

            foreach (Skill skill in skills)
            {
                Debug.Log(skill.skillName);
                skillDict.Add(skill.skillName, skill);
            }

            return skillDict;
        }
    }
   

}