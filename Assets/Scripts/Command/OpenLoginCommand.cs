using Event;
using QFramework;

namespace Command
{
    public class OpenLoginCommand : AbstractCommand {
        protected override void OnExecute() {
            this.SendEvent(new OpenLoginEvent());
        }
    }
}
