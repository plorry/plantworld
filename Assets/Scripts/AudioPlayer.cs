using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {
	private AudioSource source;
	public static AudioPlayer Instance { get; private set; }
	public List<Clip> ClipCache;

	[System.Serializable]
	public struct Clip {
		public string name;
		public AudioClip clipFile;
	}

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayOnce (AudioClip sound) {
		source.PlayOneShot(sound);
	}

	public void PlayOnce (string clipName) {
		source.PlayOneShot(ClipCache.Find(x => x.name == clipName).clipFile);
	}
}
