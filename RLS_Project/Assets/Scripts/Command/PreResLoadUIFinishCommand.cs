using QFramework;

public class PreResLoadUIFinishCommand : AbstractCommand {
    protected override void OnExecute() {
        this.SendEvent(new PreResLoadUIFinishEvent());
    }
}
