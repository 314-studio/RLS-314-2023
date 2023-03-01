using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public interface ISoundSystem : ISystem {
    void PlayClickSound();
}

public class SoundSystem : AbstractSystem, ISoundSystem {


    public void PlayClickSound() {
        DebugTool.LogWithHexColor("播放了声音");
    }

    protected override void OnInit() {
        
    }
}
