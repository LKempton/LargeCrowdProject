using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CrowdAI
{
    public class AnimationManager : MonoBehaviour {

        public static AnimationManager _instance;

        List<CrowdMemberInfo> _redTeam;
        List<CrowdMemberInfo> _blueTeam;
        private int _redTeamState = 0;
        private int _blueTeamState = 0;



        [SerializeField]
        float _maxTransistionTime = 3;
        [SerializeField]
        int _membersPerUpdate = 20;

        [SerializeField]
      private readonly string[] _crowdStates = { "Cheering", "Booing" };
        

        void Awake()
        {
            _redTeam = new List<CrowdMemberInfo>();
            _blueTeam = new List<CrowdMemberInfo>();

            _instance = this;
        }

        public void AddTeamMember( CrowdMemberInfo member)
        {
            if (member.Team == Team.BLUE)
            {
                _blueTeam.Add(member);
            }
            else  
            {
                _redTeam.Add(member);
            }
        }

        public string RedTeam
        {
            get
            {
                return _crowdStates[_redTeamState];
            }

            set
            {
                for (int i = 0; i < _crowdStates.Length; i++)
                {
                    if (_crowdStates[i] == value)
                    {
                        _redTeamState = i;
                        StartCoroutine(ChangeTeam(Team.RED));

                        return;
                    }
                }
            }
        }

        public string BlueTeam
        {
            get
            {
                return _crowdStates[_blueTeamState];
            }

            set
            {
                for (int i = 0; i < _crowdStates.Length; i++)
                {
                    if (_crowdStates[i] == value)
                    {
                        _blueTeamState = i;
                        StartCoroutine(ChangeTeam(Team.BLUE));

                        return;
                    }
                }
            }
        }

        IEnumerator ChangeTeam(Team team)
        {
            if (team == Team.RED)
            {
                for (int i = 0; i < _redTeam.Count; i++)
                {
                    for (int j = 0; j < _membersPerUpdate && i + j < _redTeam.Count; j++)
                    {
                        _redTeam[i + j].TryChangeState(_redTeamState);
                    }
                    yield return null;
                }
            }
            else
            {
                for (int i = 0; i < _blueTeam.Count; i++)
                {
                    for (int j = 0; j < _membersPerUpdate && i + j < _blueTeam.Count; j++)
                    {
                        _blueTeam[i + j].TryChangeState(_blueTeamState);
                    }
                    yield return null;
                }
            }

           
           
        }

        public string[] States
        {
            get
            {
                return _crowdStates;
            }
        }
        
    }
}
