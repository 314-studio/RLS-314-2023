using QFramework;
using Utility;

namespace Model
{
    public interface IGameModel : IModel
    {
        BindableProperty<bool> SceneLoaded { get; }
        BindableProperty<bool> SceneLoading { get; }
        BindableProperty<SceneID> LoadingTargetSceneID { get; }
    }

    public class GameModel : AbstractModel, IGameModel
    {
        public BindableProperty<bool> SceneLoaded { get; } = new BindableProperty<bool>();
        public BindableProperty<bool> SceneLoading { get; } = new BindableProperty<bool>();
        public BindableProperty<SceneID> LoadingTargetSceneID { get; } = new BindableProperty<SceneID>();


        protected override void OnInit()
        {
            SceneLoaded.Value = false;

        }
    }
}