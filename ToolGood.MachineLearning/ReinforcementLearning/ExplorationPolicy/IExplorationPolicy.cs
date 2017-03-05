

namespace ToolGood.MachineLearning
{
    using System;

    /// <summary>
    /// ̽�����߽ӿڡ�
    /// Exploration policy interface.
    /// </summary>
    /// 
    /// <remarks>
    /// �ý��������˿�̽���ԣ�����ǿ��ѧϰ��̽��״̬�ռ䡣
    /// The interface describes exploration policies, which are used in Reinforcement
    /// Learning to explore state space.</remarks>
    /// 
    public interface IExplorationPolicy
    {
        /// <summary>
        /// ѡ�������
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
        int ChooseAction( double[] actionEstimates );
    }
}
