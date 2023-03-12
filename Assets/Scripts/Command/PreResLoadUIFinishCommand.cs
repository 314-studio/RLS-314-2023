using Event;
using QFramework;

namespace Command
{
    public class PreResLoadUIFinishCommand : AbstractCommand {
        protected override void OnExecute() {
            this.SendEvent(new PreResLoadUIFinishEvent());
        }
    }
}
