using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Plays one music track at a time and crossfades when the track changes.
    /// </summary>
    public class MusicPlayer : MonoBehaviour
    {
        [Tooltip("Bank with every music track.")]
        public SoundBank bank;

        [Tooltip("Seconds taken to fade between two tracks.")]
        public float fadeSeconds = 1.2f;

        [Range(0f, 1f)]
        public float masterVolume = 0.6f;

        /// <summary>Id of the track currently playing (or fading in). Empty when silent.</summary>
        public string Current { get; private set; }

        private AudioSource active;
        private AudioSource fading;
        private float activeTarget;

        private void Awake()
        {
            active = CreateSource();
            fading = CreateSource();
            Current = "";
        }

        private AudioSource CreateSource()
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.spatialBlend = 0f;
            source.volume = 0f;
            return source;
        }

        /// <summary>Starts a track. Does nothing if it is already the current one.</summary>
        public void Play(string id)
        {
            if (id == Current) return;

            SoundBank.Entry entry;
            if (bank == null || !bank.TryGet(id, out entry)) return;

            AudioSource swap = fading;
            fading = active;
            active = swap;

            Current = id;
            active.clip = entry.clips[Random.Range(0, entry.clips.Length)];
            active.loop = entry.loop;
            active.volume = 0f;
            active.Play();
            activeTarget = entry.volume;
        }

        private void Update()
        {
            float step = Time.unscaledDeltaTime / Mathf.Max(0.01f, fadeSeconds);
            active.volume = Mathf.MoveTowards(active.volume, activeTarget * masterVolume * VolumeSettings.Music, step);
            fading.volume = Mathf.MoveTowards(fading.volume, 0f, step);
            if (fading.isPlaying && fading.volume <= 0f) fading.Stop();
        }
    }
}
