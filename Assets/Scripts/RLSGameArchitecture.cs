using System;
using Model;
using QFramework;

public class RLSGameArchitecture : Architecture<RLSGameArchitecture>
{
    protected override void Init()
    {
        RegisterSystem<IConfigSystem>(new ConfigSystem());
        RegisterSystem<ISoundSystem>(new SoundSystem());

        RegisterModel<IGameModel>(new GameModel());
    }
}
