
namespace ToolGood.MachineLearning
{
    using System;

    /// <summary>
    /// Tabu搜索策略。
    /// Tabu search exploration policy.
    /// </summary>
    /// 
    /// <remarks>
    /// 类实现简单的tabu搜索探索策略，允许将某些操作设置为tabu，用于指定的迭代次数。
    /// 从non-tabu行动的实际探索和选择是由基地探索政策完成的
    /// The class implements simple tabu search exploration policy,
    /// allowing to set certain actions as tabu for a specified amount of
    /// iterations. The actual exploration and choosing from non-tabu actions
    /// is done by <see cref="BasePolicy">base exploration policy</see>.</remarks>
    /// 
    /// <seealso cref="BoltzmannExploration"/>
    /// <seealso cref="EpsilonGreedyExploration"/>
    /// <seealso cref="RouletteWheelExploration"/>
    /// 
    public class TabuSearchExploration : IExplorationPolicy
    {
        #region private field
        // 动作总数 total actions count
        private int actions;
        // tabu动作列表 list of tabu actions
        private int[] tabuActions = null;
        // 基础勘探政策 base exploration policy
        private IExplorationPolicy basePolicy;
        #endregion

        #region public property
        /// <summary>
        /// 基本勘探策略。
        /// Base exploration policy.
        /// </summary>
        /// 
        /// <remarks>Base exploration policy is the policy, which is used
        /// to choose from non-tabu actions.</remarks>
        /// 
        public IExplorationPolicy BasePolicy
        {
            get { return basePolicy; }
            set { basePolicy = value; }
        }
        #endregion

        #region Initializes class
        /// <summary>
        /// Initializes a new instance of the <see cref="TabuSearchExploration"/> class.
        /// </summary>
        /// 
        /// <param name="actions">Total actions count.</param>
        /// <param name="basePolicy">Base exploration policy.</param>
        /// 
        public TabuSearchExploration(int actions, IExplorationPolicy basePolicy)
        {
            this.actions = actions;
            this.basePolicy = basePolicy;

            // create tabu list
            tabuActions = new int[actions];
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
        /// 该操作仅从非禁忌操作中选择
        /// The method chooses an action depending on the provided estimates. The
        /// estimates can be any sort of estimate, which values usefulness of the action
        /// (expected summary reward, discounted reward, etc). The action is choosed from
        /// non-tabu actions only.</remarks>
        /// 
        public int ChooseAction(double[] actionEstimates)
        {
            // 获得non-tabu行动的金额 get amount of non-tabu actions
            int nonTabuActions = actions;
            for (int i = 0; i < actions; i++) {
                if (tabuActions[i] != 0) {
                    nonTabuActions--;
                }
            }

            // 允许的操作 allowed actions
            double[] allowedActionEstimates = new double[nonTabuActions];
            int[] allowedActionMap = new int[nonTabuActions];

            for (int i = 0, j = 0; i < actions; i++) {
                if (tabuActions[i] == 0) {
                    // 允许操作 allowed action
                    allowedActionEstimates[j] = actionEstimates[i];
                    allowedActionMap[j] = i;
                    j++;
                } else {
                    // 减少禁忌行动的禁忌时间 decrease tabu time of tabu action
                    tabuActions[i]--;
                }
            }

            return allowedActionMap[basePolicy.ChooseAction(allowedActionEstimates)]; ;
        }

        /// <summary>
        /// 重置禁忌列表。
        /// Reset tabu list.
        /// </summary>
        /// 
        /// <remarks>Clears tabu list making all actions allowed.</remarks>
        /// 
        public void ResetTabuList()
        {
            Array.Clear(tabuActions, 0, actions);
        }

        /// <summary>
        /// 设置禁忌列表。
        /// Set tabu action.
        /// </summary>
        /// 
        /// <param name="action">设置tabu的操作。Action to set tabu for.</param>
        /// <param name="tabuTime">迭代中的Tabu时间。Tabu time in iterations.</param>
        /// 
        public void SetTabuAction(int action, int tabuTime)
        {
            tabuActions[action] = tabuTime;
        } 
        #endregion
    }
}
