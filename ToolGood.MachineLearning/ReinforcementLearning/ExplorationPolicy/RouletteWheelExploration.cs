
namespace ToolGood.MachineLearning
{
    using System;

    /// <summary>
    /// 轮盘轮勘探政策。
    /// Roulette wheel exploration policy.
    /// </summary>
    /// 
    /// <remarks><para>The class implements roulette whell exploration policy. Acording to the policy,
    /// action <b>a</b> at state <b>s</b> is selected with the next probability:</para>
    /// <code lang="none">
    ///                   Q( s, a )
    /// p( s, a ) = ------------------
    ///              SUM( Q( s, b ) )
    ///               b
    /// </code>
    /// <para>where <b>Q(s, a)</b> is action's <b>a</b> estimation (usefulness) at state <b>s</b>.</para>
    /// 
    /// <para><note>The exploration policy may be applied only in cases, when action estimates (usefulness)
    /// are represented with positive value greater then 0.</note></para>
    /// </remarks>
    /// 
    /// <seealso cref="BoltzmannExploration"/>
    /// <seealso cref="EpsilonGreedyExploration"/>
    /// <seealso cref="TabuSearchExploration"/>
    /// 
    public class RouletteWheelExploration : IExplorationPolicy
    {
        #region private field
        // random number generator
        private Random rand = new Random();

        #endregion

        #region Initializes class
        /// <summary>
        /// Initializes a new instance of the <see cref="RouletteWheelExploration"/> class.
        /// </summary>
        /// 
        public RouletteWheelExploration() { }
        #endregion

        #region public function
        /// <summary>
        /// 选择操作
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
        public int ChooseAction(double[] actionEstimates)
        {
            // 动作计数 actions count
            int actionsCount = actionEstimates.Length;
            // 动作合 actions sum
            double sum = 0, estimateSum = 0;

            for (int i = 0; i < actionsCount; i++) {
                estimateSum += actionEstimates[i];
            }

            // 获取随机数，这决定了选择哪个动作 get random number, which determines which action to choose
            double actionRandomNumber = rand.NextDouble();

            for (int i = 0; i < actionsCount; i++) {
                sum += actionEstimates[i] / estimateSum;
                if (actionRandomNumber <= sum)
                    return i;
            }

            return actionsCount - 1;
        } 
        #endregion
    }
}
