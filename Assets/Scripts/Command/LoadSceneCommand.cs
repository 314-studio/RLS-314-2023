using Model;
using QFramework;
using UnityEngine.SceneManagement;
using Utility;

namespace Command
{
    public class LoadSceneCommand : AbstractCommand
    {
        private SceneID mSceneID;
        public LoadSceneCommand(SceneID sceneID)
        {
            mSceneID = sceneID;
        }

        protected override void OnExecute()
        {
            if (this.GetModel<IGameModel>().SceneLoading)
            {
                return;
            }
            this.GetModel<IGameModel>().LoadingTargetSceneID.Value = mSceneID;
            SceneManager.LoadScene((int)SceneID.Loading);
        }
    }
}
