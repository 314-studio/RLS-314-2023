namespace FrameworkDesign
{
    public interface ICanGetUtility : IBelongToArchitecture
    {

    }

    public static class CanGetUTtilityExtension
    {
        public static T GetUtility<T>(this ICanGetUtility self) where T : class, IUtility
        {
            return self.GetArchitecture().GetUtility<T>();
        }
    }
}

