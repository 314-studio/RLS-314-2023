using QFramework;

namespace ShootingEditor2D
{
    public class ShootingEditor2D : Architecture<ShootingEditor2D>
    {
        protected override void Init()
        {
            DebugTool.LogWithHexColor("ShootingEditor2D Init");
            this.RegisterSystem<ITimeSystem>(new TimeSystem());
            this.RegisterSystem<IStatSystem>(new StatSystem());
            this.RegisterSystem<IGunSystem>(new GunSystem());
            
            this.RegisterModel<IGunConfigModel>(new GunConfigModel());
            
            this.RegisterModel<IPlayerModel>(new PlayerModel());
        }
    }
}
