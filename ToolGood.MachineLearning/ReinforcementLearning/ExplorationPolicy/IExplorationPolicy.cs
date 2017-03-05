

namespace ToolGood.MachineLearning
{
    using System;

    /// <summary>
    /// 探索政策接口。
    /// Exploration policy interface.
    /// </summary>
    /// 
    /// <remarks>
    /// 该界面描述了勘探策略，用于强化学习以探究状态空间。
    /// The interface describes exploration policies, which are used in Reinforcement
    /// Learning to explore state space.</remarks>
    /// 
    public interface IExplorationPolicy
    {
        /// <summary>
        /// 选择操作。
        /// Choose an action.
        /// </summary>
        /// 
        /// <param name="actionEstimates">Action estimates.</param>
        /// 
        /// <returns>Returns selected action.</returns>
        /// 
        /// <remarks>
        /// 该方法根据所提供的估计估计来选择动作可以是任何种类的估计，其估计动作的有用性（预期部分报酬，贴现报酬等）。
        /// The method chooses an action depending on the provided estimates. The
        /// estimates can be any sort of estimate, which values usefulness of the action
        /// (expected summary reward, discounted reward, etc).</remarks>
        /// 
        int ChooseAction( double[] actionEstimates );
    }
}
