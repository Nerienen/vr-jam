using UnityEngine;
using UnityEngine.Playables;

namespace VRJammies.Framework.ScriptedEvents
{
    public class TimelinePlayer : MonoBehaviour
    {
        [SerializeField] private PlayableDirector director;

        [ContextMenu("Play Timeline")]
        public void Play()
        {
            director.Play();
        }
    }
}
