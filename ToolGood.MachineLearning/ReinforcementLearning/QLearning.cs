
namespace ToolGood.MachineLearning
{
    using System;

    /// <summary>
    /// QLearning学习算法。
	/// QLearning learning algorithm.
	/// </summary>
    /// 
    /// <remarks>
    /// 类提供了Q学习算法的实现，即政策的时间差控制。
    /// The class provides implementation of Q-Learning algorithm, known as
    /// off-policy Temporal Difference control.
    /// </remarks>
    /// 
    /// <seealso cref="Sarsa"/>
    /// 
	public class QLearning
	{
        #region private field
        private int states; // 可能状态的数量 amount of possible states
        private int actions;// 可能动作的数量 amount of possible actions
        private double[][] qvalues; // q-values
        private IExplorationPolicy explorationPolicy;     // 探索策略 exploration policy
        private double discountFactor = 0.95;// 折扣因子 discount factor 
        // 学习率 learning rate
        private double learningRate = 0.25;
        #endregion

        #region public property
        /// <summary>
        /// 可能状态的数量
        /// Amount of possible states.
        /// </summary>
        /// 
        public int StatesCount
        {
            get { return states; }
        }

        /// <summary>
        /// 可能动作的数量
        /// Amount of possible actions.
        /// </summary>
        /// 
        public int ActionsCount
        {
            get { return actions; }
        }

        /// <summary>
        /// 探索策略
        /// Exploration policy.
        /// </summary>
        /// 
        /// <remarks>
        /// 策略，用于选择操作。
        /// Policy, which is used to select actions.</remarks>
        /// 
        public IExplorationPolicy ExplorationPolicy
        {
            get { return explorationPolicy; }
            set { explorationPolicy = value; }
        }

        /// <summary>
        /// 学习率[0, 1].
        /// Learning rate, [0, 1].
        /// </summary>
        /// 
        /// <remarks>The value determines the amount of updates Q-function receives
        /// during learning. The greater the value, the more updates the function receives.
        /// The lower the value, the less updates it receives.</remarks>
        /// 
		public double LearningRate
        {
            get { return learningRate; }
            set { learningRate = Math.Max(0.0, Math.Min(1.0, value)); }
        }

        /// <summary>
        /// 贴现因子 [0, 1].
        /// Discount factor, [0, 1].
        /// </summary>
        /// 
        /// <remarks>
        /// 预期汇总奖励的折现系数。 该值用作预期报酬的乘数。 
        /// 因此，如果值设置为1，则预期汇总奖励不会被折现。 
        /// 如果值越来越小，则预期奖励的较小量被用于动作估计更新。
        /// Discount factor for the expected summary reward. The value serves as
        /// multiplier for the expected reward. So if the value is set to 1,
        /// then the expected summary reward is not discounted. If the value is getting
        /// smaller, then smaller amount of the expected reward is used for actions'
        /// estimates update.</remarks>
        /// 
        public double DiscountFactor
        {
            get { return discountFactor; }
            set { discountFactor = Math.Max(0.0, Math.Min(1.0, value)); }
        }
        #endregion

        #region Initializes class
        /// <summary>
        /// Initializes a new instance of the <see cref="QLearning"/> class.
        /// </summary>
        /// 
        /// <param name="states">可能状态的数量 Amount of possible states.</param>
        /// <param name="actions">可能动作的数量 Amount of possible actions.</param>
        /// <param name="explorationPolicy">探索策略 Exploration policy.</param>
        /// 
        /// <remarks>
        /// 在使用此构造函数的情况下，动作估计值是随机的。
        /// Action estimates are randomized in the case of this constructor
        /// is used.</remarks>
        /// 
        public QLearning(int states, int actions, IExplorationPolicy explorationPolicy) :
            this(states, actions, explorationPolicy, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QLearning"/> class.
        /// </summary>
        /// 
        /// <param name="states">可能状态的数量 Amount of possible states.</param>
        /// <param name="actions">可能动作的数量 Amount of possible actions.</param>
        /// <param name="explorationPolicy">探索策略 Exploration policy.</param>
        /// <param name="randomize">是否随机化动作估计。 Randomize action estimates or not.</param>
        /// 
        /// <remarks>
        /// randomize参数指定初始动作估计值是否应该用小值随机化。 当使用贪婪勘探策略时，动作值的随机化可能是有用的。 在这种情况下，随机化确保不总是选择相同类型的动作。
        /// 随机参数指定初始动作估计应该随机值较小或不。随机值的动作值可能是有用的，当贪婪的勘探政策。在这种情况下随机化确保相同类型的操作不总是被选择的。
        /// The <b>randomize</b> parameter specifies if initial action estimates should be randomized
        /// with small values or not. Randomization of action values may be useful, when greedy exploration
        /// policies are used. In this case randomization ensures that actions of the same type are not chosen always.</remarks>
        /// 
        public QLearning(int states, int actions, IExplorationPolicy explorationPolicy, bool randomize)
        {
            this.states = states;
            this.actions = actions;
            this.explorationPolicy = explorationPolicy;

            // create Q-array
            qvalues = new double[states][];
            for (int i = 0; i < states; i++) {
                qvalues[i] = new double[actions];
            }

            // do randomization
            if (randomize) {
                Random rand = new Random();

                for (int i = 0; i < states; i++) {
                    for (int j = 0; j < actions; j++) {
                        qvalues[i][j] = rand.NextDouble() / 10;
                    }
                }
            }
        }
        #endregion

        #region public function
        /// <summary>
        /// 从指定状态获取下一个动作。
        /// Get next action from the specified state.
        /// </summary>
        /// 
        /// <param name="state">要获取操作的当前状态。
        /// Current state to get an action for.</param>
        /// 
        /// <returns>
        /// 返回状态的动作
        /// Returns the action for the state.</returns>
        /// 
        /// <remarks>
        /// 该方法根据当前返回一个动作
        /// The method returns an action according to current
        /// <see cref="ExplorationPolicy">exploration policy</see>.</remarks>
        /// 
        public int GetAction(int state)
        {
            double[] nextActionEstimations = qvalues[state];
            double maxNextExpectedReward = nextActionEstimations[0];

            for (int i = 1; i < actions; i++) {
                if (nextActionEstimations[i] > maxNextExpectedReward)
                    maxNextExpectedReward = nextActionEstimations[i];
            }

            return explorationPolicy.ChooseAction(qvalues[state]);
        }

        /// <summary>
        /// 更新先前状态对操作对的Q函数值。
        /// 更新之前的状态-动作对Q函数的值。
        /// Update Q-function's value for the previous state-action pair.
        /// </summary>
        /// 
        /// <param name="previousState">上一个状态。Previous state.</param>
        /// <param name="action">动作，从前一个状态转到下一个状态。 Action, which leads from previous to the next state.</param>
        /// <param name="reward">奖励值，通过从以前的状态采取指定的操作来接收。
        /// Reward value, received by taking specified action from previous state.</param>
        /// <param name="nextState">下一个状态。Next state.</param>
        /// 
		public void UpdateState(int previousState, int action, double reward, int nextState)
        {
            // 下一个状态的动作估计 next state's action estimations
            double[] nextActionEstimations = qvalues[nextState];
            // 从下一个州找到最大预期总结报酬 find maximum expected summary reward from the next state
            double maxNextExpectedReward = nextActionEstimations[0];

            for (int i = 1; i < actions; i++) {
                if (nextActionEstimations[i] > maxNextExpectedReward)
                    maxNextExpectedReward = nextActionEstimations[i];
            }

            // 前一个状态的动作估计 previous state's action estimations
            double[] previousActionEstimations = qvalues[previousState];
            // 更新的状态汇总回报预期 update expexted summary reward of the previous state
            previousActionEstimations[action] *= (1.0 - learningRate);
            previousActionEstimations[action] += (learningRate * (reward + discountFactor * maxNextExpectedReward));
        } 
        #endregion
    }
}