using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundPoolType { SFX, BGM, LOOP }
public class SoundManager : SingletonMono<SoundManager>
{
    protected override bool dontDestroyOnLoad => true;

    [SerializeField] private Transform sfxFolder, bgmFolder, loopFolder;

    [SerializeField] private int maxSfxCount, maxBgmCount, maxLoopCount;

    private Dictionary<SoundPoolType, SoundPool> soundPoolDict = new Dictionary<SoundPoolType, SoundPool>();

    [SerializeField] private AudioMixer mixer;

    public new static SoundManager GetOrCreateInstance()
    {
        if (!avaiable)
            return null;

        if (instance == null && Application.isPlaying)
        {
            SoundManager prefab = Resources.Load<SoundManager>("Mgr/SoundManager");
            instance = Instantiate(prefab);
        }
        return instance;
    }

    protected override void Init()
    {
        soundPoolDict.Add(SoundPoolType.SFX, new SoundPool(sfxFolder, new Stack<AudioSource>(), maxSfxCount, "SFX", SoundPoolType.SFX));
        soundPoolDict.Add(SoundPoolType.BGM, new SoundPool(bgmFolder, new Stack<AudioSource>(), maxBgmCount, "BGM", SoundPoolType.BGM));
        soundPoolDict.Add(SoundPoolType.LOOP, new SoundPool(loopFolder, new Stack<AudioSource>(), maxBgmCount, "SFX", SoundPoolType.LOOP));

        for (int i = 1; i <= maxSfxCount; i++)
            soundPoolDict[SoundPoolType.SFX].availables.Push(CreateAudioSource(SoundPoolType.SFX, i,1));

        for (int i = 1; i <= maxLoopCount; i++)
            soundPoolDict[SoundPoolType.LOOP].availables.Push(CreateAudioSource(SoundPoolType.LOOP, i,1));

        for (int i = 1; i <= maxBgmCount; i++)
            soundPoolDict[SoundPoolType.BGM].availables.Push(CreateAudioSource(SoundPoolType.BGM, i, 0));
    }

    private void Update()
    {
        foreach (var a in soundPoolDict)
        {
            SoundPool soundPool = a.Value;
            var node = soundPool.activeSounds.First;
            while (node != null)
            {
                ActiveSound p = node.Value;
                var nextNode = node.Next;

                if(p.followTrans!=null)
                    p.audio.transform.position = p.followTrans.position;

                if (p.lifeTime.TimerTick())
                {
                    p.audio.gameObject.SetActive(false);
                    soundPool.availables.Push(p.audio);
                    soundPool.activeSounds.Remove(node);
                }
                node = nextNode;
            }
        }
    }

    public void PlaySound(SoundPoolType type, AudioClip clip, Vector3 pos, float volume = 1, float stTime = 0, float lifeTime = 0)
    {
        if (soundPoolDict.TryGetValue(type, out var soundPool))
        {
            AudioSource audio = GetAudioSource(soundPool);
            audio.gameObject.SetActive(true);
            audio.transform.position = pos;
            audio.clip = clip;
            audio.time = stTime;
            audio.volume = volume;
            audio.loop = false;
            audio.Play();
            soundPool.activeSounds.AddLast(new ActiveSound(audio, type, lifeTime));
        }
    }
    public int PlayLoop(SoundPoolType type, AudioClip clip, float volume = 1,Transform followTrans = null)
    {
        if (soundPoolDict.TryGetValue(type, out var soundPool))
        {
            AudioSource audio = GetAudioSource(soundPool);
            audio.gameObject.SetActive(true);
            audio.clip = clip;
            audio.loop = true;
            audio.volume = volume;
            audio.Play();
            int loopId = Calc.GetUnuseIntInDic(soundPool.loopSounds);

            soundPool.loopSounds.Add(loopId, new ActiveSound(audio, type,followTrans:followTrans));
            return loopId;
        }
        return 0;
    }
    public void EndLoop(SoundPoolType type, int loopId)
    {
        if (soundPoolDict.TryGetValue(type, out var soundPool))
        {
            if (soundPool.loopSounds.TryGetValue(loopId, out ActiveSound p))
            {
                p.audio.gameObject.SetActive(false);
                soundPool.availables.Push(p.audio);
                soundPool.loopSounds.Remove(loopId);
            }
        }
    }

    private AudioSource GetAudioSource(SoundPool group)
    {
        if (group.availables.Count > 0)
        {
            return group.availables.Pop();
        }
        else
        {
            group.maxCount += 1;
            return CreateAudioSource(group.type, group.maxCount,group.type == SoundPoolType.BGM?0:1);
        }
    }

    private AudioSource CreateAudioSource(SoundPoolType type, int index,int spatialBlend)
    {
        if (soundPoolDict.TryGetValue(type, out var soundGroup))
        {
            GameObject sfx = new GameObject($"{soundGroup.mixerName} {index}");
            AudioSource audio = sfx.AddComponent<AudioSource>();
            audio.outputAudioMixerGroup = mixer.FindMatchingGroups("Master/" + soundGroup.mixerName)[0];
            audio.spatialBlend = spatialBlend;
            audio.maxDistance = 30;

            sfx.SetActive(false);
            sfx.transform.SetParent(soundGroup.objFolder);
            return audio;
        }
        return null;
    }

    private class SoundPool
    {
        public Transform objFolder;
        public Stack<AudioSource> availables;
        public int maxCount;
        public string mixerName;
        public SoundPoolType type;

        public LinkedList<ActiveSound> activeSounds = new LinkedList<ActiveSound>();
        public Dictionary<int, ActiveSound> loopSounds = new Dictionary<int, ActiveSound>();

        public SoundPool(Transform objFolder, Stack<AudioSource> availables, int maxCount, string mixerName, SoundPoolType type)
        {
            this.objFolder = objFolder;
            this.availables = availables;
            this.maxCount = maxCount;
            this.mixerName = mixerName;
            this.type = type;
        }
    }
    private class ActiveSound
    {
        public AudioSource audio;
        public float lifeTime;
        public SoundPoolType type;
        public Transform followTrans;

        public ActiveSound(AudioSource audio, SoundPoolType type, float lifeTime = 0,Transform followTrans = null)
        {
            this.audio = audio;
            this.type = type;
            this.lifeTime = lifeTime == 0 ? audio.clip.length : lifeTime;
            this.followTrans = followTrans;
        }
    }

}
