

namespace ToolGood.MachineLearning
{
    using System;

    /// <summary>
    /// 玻尔兹曼分布勘探政策。
    /// Boltzmann distribution exploration policy.
    /// </summary>
    /// 
    /// <remarks><para>The class implements exploration policy base on Boltzmann distribution.
    /// Acording to the policy, action <b>a</b> at state <b>s</b> is selected with the next probability:</para>
    /// <code lang="none">
    ///                   exp( Q( s, a ) / t )
    /// p( s, a ) = -----------------------------
    ///              SUM( exp( Q( s, b ) / t ) )
    ///               b
    /// </code>
    /// <para>where <b>Q(s, a)</b> is action's <b>a</b> estimation (usefulness) at state <b>s</b> and
    /// <b>t</b> is <see cref="Temperature"/>.</para>
    /// </remarks>
    /// 
    /// <seealso cref="RouletteWheelExploration"/>
    /// <seealso cref="EpsilonGreedyExploration"/>
    /// <seealso cref="TabuSearchExploration"/>
    /// 
    public class BoltzmannExploration : IExplorationPolicy
    {
        #region private field
        // 玻尔兹曼分布的温度参数 termperature parameter of Boltzmann distribution
        private double temperature;

        // random number generator
        private Random rand = new Random();
        #endregion

        #region public property
        /// <summary>
        /// 玻尔兹曼分布的温度参数 >0.
        /// Termperature parameter of Boltzmann distribution, >0.
        /// </summary>
        /// 
        /// <remarks><para>
        /// 该属性设置探索和贪婪行为之间的平衡。 如果温度低，那么政策往往更贪婪。
        /// The property sets the balance between exploration and greedy actions.
        /// If temperature is low, then the policy tends to be more greedy.</para></remarks>
        /// 
        public double Temperature
        {
            get { return temperature; }
            set { temperature = Math.Max(0, value); }
        }
        #endregion

        #region Initializes class
        /// <summary> 
        /// Initializes a new instance of the <see cref="BoltzmannExploration"/> class.
        /// </summary>
        /// 
        /// <param name="temperature">玻尔兹曼分布的温度参数。 Termperature parameter of Boltzmann distribution.</param>
        /// 
        public BoltzmannExploration(double temperature)
        {
            Temperature = temperature;
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
        /// 该方法根据所提供的估计来选择动作。 估计可以是任何种类的估计，其估计动作的有用性（预期表单奖励，打折报酬等）。
        /// 
        /// The method chooses an action depending on the provided estimates. The
        /// estimates can be any sort of estimate, which values usefulness of the action
        /// (expected summary reward, discounted reward, etc).</remarks>
        /// 
        public int ChooseAction(double[] actionEstimates)
        {
            // 动作计数 actions count
            int actionsCount = actionEstimates.Length;
            // 动作概率 action probabilities
            double[] actionProbabilities = new double[actionsCount];
            // 动作总和 actions sum
            double sum = 0, probabilitiesSum = 0;

            for (int i = 0; i < actionsCount; i++) {
                double actionProbability = Math.Exp(actionEstimates[i] / temperature);

                actionProbabilities[i] = actionProbability;
                probabilitiesSum += actionProbability;
            }

            if ((double.IsInfinity(probabilitiesSum)) || (probabilitiesSum == 0)) {
                // 在无限或零的情况下贪婪选择 do greedy selection in the case of infinity or zero
                double maxReward = actionEstimates[0];
                int greedyAction = 0;

                for (int i = 1; i < actionsCount; i++) {
                    if (actionEstimates[i] > maxReward) {
                        maxReward = actionEstimates[i];
                        greedyAction = i;
                    }
                }
                return greedyAction;
            }

            // 获取随机数，这决定了选择哪个动作 get random number, which determines which action to choose
            double actionRandomNumber = rand.NextDouble();

            for (int i = 0; i < actionsCount; i++) {
                sum += actionProbabilities[i] / probabilitiesSum;
                if (actionRandomNumber <= sum)
                    return i;
            }

            return actionsCount - 1;
        } 
        #endregion
    }
}
