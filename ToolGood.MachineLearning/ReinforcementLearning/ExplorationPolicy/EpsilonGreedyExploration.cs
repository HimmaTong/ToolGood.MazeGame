

namespace ToolGood.MachineLearning
{
    using System;

    /// <summary>
    /// Epsilon贪婪勘探政策。
    /// Epsilon greedy exploration policy.
    /// </summary>
    /// 
    /// <remarks><para>The class implements epsilon greedy exploration policy. Acording to the policy,
    /// the best action is chosen with probability <b>1-epsilon</b>. Otherwise,
    /// with probability <b>epsilon</b>, any other action, except the best one, is
    /// chosen randomly.</para>
    /// 
    /// <para>According to the policy, the epsilon value is known also as exploration rate.</para>
    /// </remarks>
    /// 
    /// <seealso cref="RouletteWheelExploration"/>
    /// <seealso cref="BoltzmannExploration"/>
    /// <seealso cref="TabuSearchExploration"/>
    /// 
    public class EpsilonGreedyExploration : IExplorationPolicy
    {
        #region private field
        // 勘探速率 exploration rate
        private double epsilon;

        // random number generator
        private Random rand = new Random();
        #endregion

        #region public property
        /// <summary>
        /// 勘探速率 [0, 1].
        /// Epsilon value (exploration rate), [0, 1].
        /// </summary>
        /// 
        /// <remarks><para>
        /// 该值确定策略驱动的探索量。 如果值很高，那么策略驱动更多的探索 - 选择随机动作，排除最好的。
        /// 如果值很低，那么策略更贪婪 - 选择到目前为止的动作。
        /// The value determines the amount of exploration driven by the policy.
        /// If the value is high, then the policy drives more to exploration - choosing random
        /// action, which excludes the best one. If the value is low, then the policy is more
        /// greedy - choosing the beat so far action.
        /// </para></remarks>
        /// 
        public double Epsilon
        {
            get { return epsilon; }
            set { epsilon = Math.Max(0.0, Math.Min(1.0, value)); }
        }
        #endregion

        #region Initializes class
        /// <summary>
        /// Initializes a new instance of the <see cref="EpsilonGreedyExploration"/> class.
        /// </summary>
        /// 
        /// <param name="epsilon">Epsilon value (exploration rate).</param>
        /// 
        public EpsilonGreedyExploration(double epsilon)
        {
            Epsilon = epsilon;
        }
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
            if (actionEstimates.Length == 1) return 0;

            // 动作计数 actions count
            int actionsCount = actionEstimates.Length;

            // 找到最好的动作（贪婪） find the best action (greedy)
            double maxReward = actionEstimates[0];
            int greedyAction = 0;

            for (int i = 1; i < actionsCount; i++) {
                if (actionEstimates[i] > maxReward) {
                    maxReward = actionEstimates[i];
                    greedyAction = i;
                }
            }

            // 尝试做探索 try to do exploration
            if (rand.NextDouble() < epsilon) {
                int randomAction = rand.Next(actionsCount - 1);

                if (randomAction >= greedyAction)
                    randomAction++;

                return randomAction;
            }

            return greedyAction;
        } 
        #endregion
    }
}
