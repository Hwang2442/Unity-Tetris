using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonBase<SoundManager>
{
    private AudioSource audioBG;
    private AudioSource audioFX;

    private Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>();

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        // 오디오 소스 컴포넌트 추가해줌
        audioBG = gameObject.AddComponent<AudioSource>();
        audioFX = gameObject.AddComponent<AudioSource>();

        audioBG.playOnAwake = false;
        audioFX.playOnAwake = false;

        audioBG.loop = true;
        audioFX.loop = false;

        // 오디오 클립 추가
        //** BGM
        sounds.Add("BGM-1", Resources.Load("Sounds/SoundBG_1") as AudioClip);
        sounds.Add("BGM-2", Resources.Load("Sounds/SoundBG_2") as AudioClip);

        //** FX
        sounds.Add("FX_GameOver", Resources.Load("Sounds/soundFX_Over") as AudioClip);
        sounds.Add("FX_Common", Resources.Load("Sounds/soundFX_Common") as AudioClip);

        Debug.Log(gameObject.name + "Start!!");
    }

    // BGM 재생용
    public void Play(string key, bool replay = false)
    {
        AudioClip clip = null;

        if (sounds.TryGetValue(key, out clip) && audioBG != null)
        {
            // 재생하고자 하는 클립이 같을 시 재실행 여부
            if (audioBG.clip == clip)
            {
                if (replay || !audioBG.isPlaying) audioBG.Play();
            }
            else
            {
                audioBG.clip = clip;

                audioBG.Play();
            }
        }
    }
    // FXM 재생용
    public void PlayOneShot(string key)
    {
        AudioClip clip;

        if (sounds.TryGetValue(key, out clip) && audioFX != null)
        {
            audioFX.PlayOneShot(clip);
        }
    }
    public void Stop()
    {
        audioBG.Stop();
    }

    // 볼륨 설정
    public void SetVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0, 1);

        audioBG.volume = volume;
        audioFX.volume = volume;
    }

    public float GetVolume()
    {
        return audioFX.volume;
    }
}
