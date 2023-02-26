using QFramework;

public class RLSGameArchitecture : Architecture<RLSGameArchitecture>
{
    protected override void Init()
    {
        RegisterSystem<IConfigSystem>(new ConfigSystem());
    }
}
