
namespace BTree
{
    public enum BTResult
    {
        Failed = 0,
        Executing = 1,
        Success = 2,
    }
    public enum BTStatus
    {
        Ready = 1,
        Running = 2,
        Finish = 3,
    }
    public enum BTParallelCondition
    {
        OR = 1,
        AND = 2,
    }

}
