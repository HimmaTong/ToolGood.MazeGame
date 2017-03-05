using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.MachineLearning
{
    /// <summary>
    /// 灵感来自DeepMind的 Deep Reinforcement Learning with Double Q-learning
    /// 因为每一次UpdateState时，Q总是被高估，
    /// 我的解决方案如下：
    /// qvalues拆成两个分别为qvalues、qvalues2，并且 q-values每一个值都大于q-values2
    /// 每次GetAction时，返回两者的平均值
    /// 每一次UpdateState时，获取下一次qvalues、qvalues2的平均值的最大值的action,取最小Q值
    /// 当UpdateState，Q值增加时，新值为较小值的更新值，qvalues2=qvalues，q-values=新值;
    /// 当UpdateState，Q值减少时，新值为较大值的更新值，qvalues=qvalues2，qvalues2=新值
    /// 当运行多次后qvalues与qvalues2会相等。
    /// 作者：ToolGood
    /// 时间：2017年3月5日14:12:28
    /// </summary>
    public class DoubleQLearning
    {
        #region private field
        private int states; // 可能状态的数量 amount of possible states
        private int actions;// 可能动作的数量 amount of possible actions
        private double[][] qvalues; // q-values
        private double[][] qvalues2; // q-values2, q-values每一个值都大于q-values2
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
        public DoubleQLearning(int states, int actions, IExplorationPolicy explorationPolicy) :
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
        public DoubleQLearning(int states, int actions, IExplorationPolicy explorationPolicy, bool randomize)
        {
            this.states = states;
            this.actions = actions;
            this.explorationPolicy = explorationPolicy;

            // create Q-array
            qvalues = new double[states][];
            for (int i = 0; i < states; i++) {
                qvalues[i] = new double[actions];
            }
            qvalues2 = new double[states][];
            for (int i = 0; i < states; i++) {
                qvalues2[i] = new double[actions];
            }

            // do randomization
            if (randomize) {
                Random rand = new Random();

                for (int i = 0; i < states; i++) {
                    for (int j = 0; j < actions; j++) {
                        var q1 = rand.NextDouble() / 10;
                        var q2 = rand.NextDouble() / 10;
                        if (q1 > q2) {
                            qvalues[i][j] = q1;
                            qvalues2[i][j] = q2;
                        } else {
                            qvalues[i][j] = q2;
                            qvalues2[i][j] = q1;
                        }
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
            double[] qs = new double[actions];
            for (int i = 0; i < actions; i++) {
                qs[i] = (qvalues[state][i] + qvalues2[state][i]) / 2;
            }
            return explorationPolicy.ChooseAction(qs);
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
            double[] nextActionEstimations1 = qvalues[nextState];
            double[] nextActionEstimations2 = qvalues2[nextState];

            // 从下一个州找到最大预期总结报酬 find maximum expected summary reward from the next state
            double maxNextExpectedReward = (nextActionEstimations1[0] + nextActionEstimations2[0]) / 2;
            double nextExpectedReward = nextActionEstimations2[0];

            for (int i = 1; i < actions; i++) {
                var expectedReward = (nextActionEstimations1[i] + nextActionEstimations2[i]) / 2;
                if (expectedReward > maxNextExpectedReward) {
                    maxNextExpectedReward = expectedReward;
                    nextExpectedReward = nextActionEstimations2[i];
                }
            }

            var actionEstimations1 = qvalues[previousState][action];
            var actionEstimations2 = qvalues2[previousState][action];
            var actionEstimations3 = actionEstimations1 * (1.0 - learningRate) + (learningRate * (reward + discountFactor * nextExpectedReward));
            var actionEstimations4 = actionEstimations2 * (1.0 - learningRate) + (learningRate * (reward + discountFactor * nextExpectedReward));

            List<double> list = new List<double>();
            list.Add(actionEstimations1);
            list.Add(actionEstimations2);
            list.Add(actionEstimations3);
            list.Add(actionEstimations4);
            list = list.OrderBy(q => q).ToList();
            qvalues[previousState][action] = list[2];
            qvalues2[previousState][action] = list[1];
        }
        #endregion


    }
}
