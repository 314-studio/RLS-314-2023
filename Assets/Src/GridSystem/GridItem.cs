namespace Src.GridSystem
{
    public class GridItem : IGridItem
    {
        public string GetClassName()
        {
            return this.GetType().Name;
        }
    }
}