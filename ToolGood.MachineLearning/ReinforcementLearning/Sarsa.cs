
namespace ToolGood.MachineLearning
{
    using System;

    /// <summary>
    /// Sarsa学习算法。
    /// Sarsa learning algorithm.
    /// </summary>
    /// 
    /// <remarks>
    /// 该类提供了Sarse算法的实现，称为策略时间差控制。
    /// The class provides implementation of Sarse algorithm, known as
    /// on-policy Temporal Difference control.</remarks>
    /// 
    /// <seealso cref="QLearning"/>
    /// 
    public class Sarsa
    {
        #region private field
        // 可能状态的数量 amount of possible states
        private int states;
        // 可能动作的数量 amount of possible actions
        private int actions;
        // q-values
        private double[][] qvalues;
        // 探索策略 exploration policy
        private IExplorationPolicy explorationPolicy;

        // 折扣因子 discount factor 
        // 贴现因子(discount factor)，又称折现系数、折现参数，即把将来的现金流量折算成现值的介于0－1之间的一个数，在数值上可以理解为贴现率，是1个份额经过一段时间后所等同的现在份额。
        private double discountFactor = 0.95;
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
        /// Initializes a new instance of the <see cref="Sarsa"/> class.
        /// </summary>
        /// 
        /// <param name="states">Amount of possible states.</param>
        /// <param name="actions">Amount of possible actions.</param>
        /// <param name="explorationPolicy">Exploration policy.</param>
        /// 
        /// <remarks>Action estimates are randomized in the case of this constructor
        /// is used.</remarks>
        /// 
        public Sarsa(int states, int actions, IExplorationPolicy explorationPolicy) :
            this(states, actions, explorationPolicy, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sarsa"/> class.
        /// </summary>
        /// 
        /// <param name="states">Amount of possible states.</param>
        /// <param name="actions">Amount of possible actions.</param>
        /// <param name="explorationPolicy">Exploration policy.</param>
        /// <param name="randomize">Randomize action estimates or not.</param>
        /// 
        /// <remarks>The <b>randomize</b> parameter specifies if initial action estimates should be randomized
        /// with small values or not. Randomization of action values may be useful, when greedy exploration
        /// policies are used. In this case randomization ensures that actions of the same type are not chosen always.</remarks>
        /// 
        public Sarsa(int states, int actions, IExplorationPolicy explorationPolicy, bool randomize)
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
            return explorationPolicy.ChooseAction(qvalues[state]);
        }

        /// <summary>
        /// 更新先前状态对操作对的Q函数值。
        /// 更新之前的状态-动作对Q函数的值。
        /// Update Q-function's value for the previous state-action pair.
        /// </summary>
        /// 
        /// <param name="previousState">目前的状态。Curren state.</param>
        /// <param name="previousAction">行动，从上一个到下一个状态。Action, which lead from previous to the next state.</param>
        /// <param name="reward">通过从上一个状态采取指定动作接收的奖励值。Reward value, received by taking specified action from previous state.</param>
        /// <param name="nextState">下一个状态。Next state.</param>
        /// <param name="nextAction">下一个动作。Next action.</param>
        /// 
        /// <remarks>
        /// 如果下一个状态是非终端更新Q函数的值为案例，以前的状态-动作对。
        /// 在下一个状态为非终端的情况下，更新先前状态 - 动作对的Q函数值。
        /// Updates Q-function's value for the previous state-action pair in
        /// the case if the next state is non terminal.</remarks>
        /// 
        public void UpdateState(int previousState, int previousAction, double reward, int nextState, int nextAction)
        {
            // 上一个状态的动作估计 previous state's action estimations
            double[] previousActionEstimations = qvalues[previousState];
            // 更新前一状态的预期总结报酬 update expexted summary reward of the previous state
            previousActionEstimations[previousAction] *= (1.0 - learningRate);
            previousActionEstimations[previousAction] += (learningRate * (reward + discountFactor *
                                                           qvalues[nextState][nextAction]));

        }

        /// <summary>
        /// 更新之前的状态-动作对Q函数的值。
        /// Update Q-function's value for the previous state-action pair.
        /// </summary>
        /// 
        /// <param name="previousState">当前状态。 Curren state.</param>
        /// <param name="previousAction">行动，从上一个到下一个状态。Action, which lead from previous to the next state.</param>
        /// <param name="reward">通过从上一个状态采取指定动作接收的奖励值。Reward value, received by taking specified action from previous state.</param>
        /// 
        /// <remarks>
        /// 在下一个状态为终端的情况下，更新先前状态 - 动作对的Q函数值。
        /// Updates Q-function's value for the previous state-action pair in
        /// the case if the next state is terminal.</remarks>
        /// 
        public void UpdateState(int previousState, int previousAction, double reward)
        {
            // 上一个状态的动作估计 previous state's action estimations
            double[] previousActionEstimations = qvalues[previousState];
            // 更新前一状态的预期总结报酬 update expexted summary reward of the previous state
            previousActionEstimations[previousAction] *= (1.0 - learningRate);
            previousActionEstimations[previousAction] += (learningRate * reward);
        } 
        #endregion
    }
}
