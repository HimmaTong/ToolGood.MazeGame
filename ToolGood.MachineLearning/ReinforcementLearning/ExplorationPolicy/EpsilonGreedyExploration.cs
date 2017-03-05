

namespace ToolGood.MachineLearning
{
    using System;

    /// <summary>
    /// Epsilon̰����̽���ߡ�
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
        // ��̽���� exploration rate
        private double epsilon;

        // random number generator
        private Random rand = new Random();
        #endregion

        #region public property
        /// <summary>
        /// ��̽���� [0, 1].
        /// Epsilon value (exploration rate), [0, 1].
        /// </summary>
        /// 
        /// <remarks><para>
        /// ��ֵȷ������������̽������ ���ֵ�ܸߣ���ô�������������̽�� - ѡ������������ų���õġ�
        /// ���ֵ�ܵͣ���ô���Ը�̰�� - ѡ��ĿǰΪֹ�Ķ�����
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
        /// ѡ�����
        /// Choose an action.
        /// </summary>
        /// 
        /// <param name="actionEstimates">Action estimates.</param>
        /// 
        /// <returns>Returns selected action.</returns>
        /// 
        /// <remarks>
        /// �÷����������ṩ�Ĺ��ƹ�����ѡ�����������κ�����Ĺ��ƣ�����ƶ����������ԣ�Ԥ�ڲ��ֱ��꣬���ֱ���ȣ���
        /// The method chooses an action depending on the provided estimates. The
        /// estimates can be any sort of estimate, which values usefulness of the action
        /// (expected summary reward, discounted reward, etc).</remarks>
        /// 
        public int ChooseAction(double[] actionEstimates)
        {
            if (actionEstimates.Length == 1) return 0;

            // �������� actions count
            int actionsCount = actionEstimates.Length;

            // �ҵ���õĶ�����̰���� find the best action (greedy)
            double maxReward = actionEstimates[0];
            int greedyAction = 0;

            for (int i = 1; i < actionsCount; i++) {
                if (actionEstimates[i] > maxReward) {
                    maxReward = actionEstimates[i];
                    greedyAction = i;
                }
            }

            // ������̽�� try to do exploration
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
