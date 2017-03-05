
namespace ToolGood.MachineLearning
{
    using System;

    /// <summary>
    /// QLearningѧϰ�㷨��
	/// QLearning learning algorithm.
	/// </summary>
    /// 
    /// <remarks>
    /// ���ṩ��Qѧϰ�㷨��ʵ�֣������ߵ�ʱ�����ơ�
    /// The class provides implementation of Q-Learning algorithm, known as
    /// off-policy Temporal Difference control.
    /// </remarks>
    /// 
    /// <seealso cref="Sarsa"/>
    /// 
	public class QLearning
	{
        #region private field
        private int states; // ����״̬������ amount of possible states
        private int actions;// ���ܶ��������� amount of possible actions
        private double[][] qvalues; // q-values
        private IExplorationPolicy explorationPolicy;     // ̽������ exploration policy
        private double discountFactor = 0.95;// �ۿ����� discount factor 
        // ѧϰ�� learning rate
        private double learningRate = 0.25;
        #endregion

        #region public property
        /// <summary>
        /// ����״̬������
        /// Amount of possible states.
        /// </summary>
        /// 
        public int StatesCount
        {
            get { return states; }
        }

        /// <summary>
        /// ���ܶ���������
        /// Amount of possible actions.
        /// </summary>
        /// 
        public int ActionsCount
        {
            get { return actions; }
        }

        /// <summary>
        /// ̽������
        /// Exploration policy.
        /// </summary>
        /// 
        /// <remarks>
        /// ���ԣ�����ѡ�������
        /// Policy, which is used to select actions.</remarks>
        /// 
        public IExplorationPolicy ExplorationPolicy
        {
            get { return explorationPolicy; }
            set { explorationPolicy = value; }
        }

        /// <summary>
        /// ѧϰ��[0, 1].
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
        /// �������� [0, 1].
        /// Discount factor, [0, 1].
        /// </summary>
        /// 
        /// <remarks>
        /// Ԥ�ڻ��ܽ���������ϵ���� ��ֵ����Ԥ�ڱ���ĳ����� 
        /// ��ˣ����ֵ����Ϊ1����Ԥ�ڻ��ܽ������ᱻ���֡� 
        /// ���ֵԽ��ԽС����Ԥ�ڽ����Ľ�С�������ڶ������Ƹ��¡�
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
        /// <param name="states">����״̬������ Amount of possible states.</param>
        /// <param name="actions">���ܶ��������� Amount of possible actions.</param>
        /// <param name="explorationPolicy">̽������ Exploration policy.</param>
        /// 
        /// <remarks>
        /// ��ʹ�ô˹��캯��������£���������ֵ������ġ�
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
        /// <param name="states">����״̬������ Amount of possible states.</param>
        /// <param name="actions">���ܶ��������� Amount of possible actions.</param>
        /// <param name="explorationPolicy">̽������ Exploration policy.</param>
        /// <param name="randomize">�Ƿ�������������ơ� Randomize action estimates or not.</param>
        /// 
        /// <remarks>
        /// randomize����ָ����ʼ��������ֵ�Ƿ�Ӧ����Сֵ������� ��ʹ��̰����̽����ʱ������ֵ����������������õġ� ����������£������ȷ��������ѡ����ͬ���͵Ķ�����
        /// �������ָ����ʼ��������Ӧ�����ֵ��С�򲻡����ֵ�Ķ���ֵ���������õģ���̰���Ŀ�̽���ߡ�����������������ȷ����ͬ���͵Ĳ��������Ǳ�ѡ��ġ�
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
        /// ��ָ��״̬��ȡ��һ��������
        /// Get next action from the specified state.
        /// </summary>
        /// 
        /// <param name="state">Ҫ��ȡ�����ĵ�ǰ״̬��
        /// Current state to get an action for.</param>
        /// 
        /// <returns>
        /// ����״̬�Ķ���
        /// Returns the action for the state.</returns>
        /// 
        /// <remarks>
        /// �÷������ݵ�ǰ����һ������
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
        /// ������ǰ״̬�Բ����Ե�Q����ֵ��
        /// ����֮ǰ��״̬-������Q������ֵ��
        /// Update Q-function's value for the previous state-action pair.
        /// </summary>
        /// 
        /// <param name="previousState">��һ��״̬��Previous state.</param>
        /// <param name="action">��������ǰһ��״̬ת����һ��״̬�� Action, which leads from previous to the next state.</param>
        /// <param name="reward">����ֵ��ͨ������ǰ��״̬��ȡָ���Ĳ��������ա�
        /// Reward value, received by taking specified action from previous state.</param>
        /// <param name="nextState">��һ��״̬��Next state.</param>
        /// 
		public void UpdateState(int previousState, int action, double reward, int nextState)
        {
            // ��һ��״̬�Ķ������� next state's action estimations
            double[] nextActionEstimations = qvalues[nextState];
            // ����һ�����ҵ����Ԥ���ܽᱨ�� find maximum expected summary reward from the next state
            double maxNextExpectedReward = nextActionEstimations[0];

            for (int i = 1; i < actions; i++) {
                if (nextActionEstimations[i] > maxNextExpectedReward)
                    maxNextExpectedReward = nextActionEstimations[i];
            }

            // ǰһ��״̬�Ķ������� previous state's action estimations
            double[] previousActionEstimations = qvalues[previousState];
            // ���µ�״̬���ܻر�Ԥ�� update expexted summary reward of the previous state
            previousActionEstimations[action] *= (1.0 - learningRate);
            previousActionEstimations[action] += (learningRate * (reward + discountFactor * maxNextExpectedReward));
        } 
        #endregion
    }
}