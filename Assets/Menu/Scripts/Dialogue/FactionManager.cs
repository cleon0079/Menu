using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cleon
{
    [System.Serializable]
    class Factions
    {
        public string factionName;
        float _approval;
        public float approval
        {
            set
            {
                _approval = Mathf.Clamp(value, -1, 1);
            }
            get
            {
                return _approval;
            }
        }

        public Factions(float _inltialApproval)
        {
            approval = _inltialApproval;
        }
    }


    public class FactionManager : MonoBehaviour
    {
        // A list of factions me make in scene
        [SerializeField] List<Factions> inltialiseFactions;

        // Dictinary the use to connect faction list and faction name
        Dictionary<string, Factions> factions;

        private void Awake()
        {
            // Save all the faction we make and connect it with faction name key word in to the dictionary
            factions = new Dictionary<string, Factions>();
            foreach (Factions faction in inltialiseFactions)
            {
                factions.Add(faction.factionName, new Factions(faction.approval));
            }
        }

        public float? FactionsApproval(string _factionName, float _value)
        {
            // Change the faction approval var faction name
            if (factions.ContainsKey(_factionName))
            {
                factions[_factionName].approval += _value;
                return factions[_factionName].approval;
            }
            return null;
        }

        public float? FactionsApproval(string _factionName)
        {
            // Get the factions approval var faction name
            if (factions.ContainsKey(_factionName))
            {
                return factions[_factionName].approval;
            }
            return null;
        }
    }
}
