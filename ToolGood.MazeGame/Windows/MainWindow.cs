using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ToolGood.MachineLearning;
using ToolGood.MazeGame.Codes;

namespace ToolGood.MazeGame.Windows
{
    public partial class MainWindow : Form
    {
        private int[,] map = null;
        private int[,] mapToDisplay = null;

        // 代理启动和停止位置 agent' start and stop position
        private int _agentStartX = 1;
        private int _agentStartY = 1;
        private int _agentStopX;
        private int _agentStopY;
        //学习环境 learning settings
        private int learningIterations = 1000;//迭代次数
        private double explorationRate = 0.5;//探测率
        private double learningRate = 0.5;// 学习率

        private double moveReward = 0;//
        private double wallReward = -1;
        private double goalReward = 1;
        private double roundReward = 0.05;

        // 停止后台作业的标志 flag to stop background job
        private volatile bool needToStop = false;
        // 工作线程 worker thread
        private Thread workerThread = null;


        private QLearning qLearning = null;
        private DoubleQLearning doubleQLearning = null;
        private Sarsa sarsa = null;


        public MainWindow()
        {
            InitializeComponent();
            this.algorithmCombo.SelectedIndex = 2;
        }

        #region 按钮事件
        private void newMapButton_Click(object sender, EventArgs e)
        {
            CreateNewMap();
            startLearningButton.Enabled = true;

        }
        private void startLearningButton_Click(object sender, EventArgs e)
        {
            // get settings
            GetSettings();
            ShowSettings();

            iterationBox.Text = string.Empty;

            // destroy algorithms
            qLearning = null;
            sarsa = null;
            doubleQLearning = null;

            if (algorithmCombo.SelectedIndex == 0) {
                // create new QLearning algorithm's instance
                qLearning = new QLearning(map.Length, 4, new TabuSearchExploration(4, new EpsilonGreedyExploration(explorationRate)));
                workerThread = new Thread(new ThreadStart(QLearningThread));
            } else if (algorithmCombo.SelectedIndex == 1) {
                // create new Sarsa algorithm's instance
                sarsa = new Sarsa(map.Length, 4, new TabuSearchExploration(4, new EpsilonGreedyExploration(explorationRate)));
                workerThread = new Thread(new ThreadStart(SarsaThread));
            } else {
                doubleQLearning = new DoubleQLearning(map.Length, 4, new TabuSearchExploration(4, new EpsilonGreedyExploration(explorationRate)));
                workerThread = new Thread(new ThreadStart(DoubleQLearningThread));
            }

            // disable all settings controls except "Stop" button
            EnableControls(false);

            // run worker thread
            needToStop = false;
            workerThread.Start();




            stopButton.Enabled = true;
            showSolutionButton.Enabled = true;
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            if (workerThread != null) {
                // stop worker thread
                needToStop = true;
                while (!workerThread.Join(100))
                    Application.DoEvents();
                workerThread = null;
            }
        }

        private void showSolutionButton_Click(object sender, EventArgs e)
        {
            // check if learning algorithm was run before
            if ((qLearning == null) && (sarsa == null) && (doubleQLearning == null))
                return;

            // disable all settings controls except "Stop" button
            EnableControls(false);

            // run worker thread
            needToStop = false;
            workerThread = new Thread(new ThreadStart(ShowSolutionThread));
            workerThread.Start();
        }

        #endregion

        #region 显示机器行走
        private void ShowSolutionThread()
        {
            // 将探索率设置为0，因此代理仅使用他学到的内容 set exploration rate to 0, so agent uses only what he learnt
            TabuSearchExploration tabuPolicy = null;
            EpsilonGreedyExploration exploratioPolicy = null;

            if (qLearning != null) {
                tabuPolicy = (TabuSearchExploration)qLearning.ExplorationPolicy;

            } else if (sarsa != null) {
                tabuPolicy = (TabuSearchExploration)sarsa.ExplorationPolicy;

            } else {
                tabuPolicy = (TabuSearchExploration)doubleQLearning.ExplorationPolicy;
            }

            exploratioPolicy = (EpsilonGreedyExploration)tabuPolicy.BasePolicy;

            exploratioPolicy.Epsilon = 0;
            tabuPolicy.ResetTabuList();

            // 代理的当前坐标 curent coordinates of the agent
            int agentCurrentX = _agentStartX, agentCurrentY = _agentStartY;

            // pripate地图显示 pripate the map to display
            Array.Copy(map, mapToDisplay, map.GetLength(0) * map.GetLength(1));
            mapToDisplay[_agentStartX, _agentStartY] = 2;
            mapToDisplay[_agentStopX, _agentStopY] = 3;

            while (!needToStop) {
                // 显示地图 dispay the map
                this.cellWorld1.Map = mapToDisplay;
                // sleep for a while
                Thread.Sleep(200);

                // 检查我们是否已经到达终点 check if we have reached the end point
                if ((agentCurrentX == _agentStopX) && (agentCurrentY == _agentStopY)) {
                    // 恢复地图 restore the map
                    mapToDisplay[_agentStartX, _agentStartY] = 2;
                    mapToDisplay[_agentStopX, _agentStopY] = 3;

                    agentCurrentX = _agentStartX;
                    agentCurrentY = _agentStartY;

                    this.cellWorld1.Map = mapToDisplay;
                    Thread.Sleep(200);
                }

                // 从当前位置删除代理 remove agent from current position
                mapToDisplay[agentCurrentX, agentCurrentY] = 0;

                // 获取代理的当前状态 get agent's current state
                int currentState = GetStateNumber(agentCurrentX, agentCurrentY);
                // 获取此状态的操作 get the action for this state
                int action = GetAction(currentState);// (qLearning != null) ? qLearning.GetAction(currentState) : sarsa.GetAction(currentState);
                // 更新代理的当前位置并得到他的奖励 update agent's current position and get his reward
                double reward = UpdateAgentPosition(ref agentCurrentX, ref agentCurrentY, action);

                // 把代理放到新的位置 put agent to the new position
                mapToDisplay[agentCurrentX, agentCurrentY] = 2;
            }

            // 启用设置控件 enable settings controls
            EnableControls(true);
        }
        private int GetAction(int currentState)
        {
            if (qLearning != null) {
                return qLearning.GetAction(currentState);
            }
            if (sarsa != null) {
                return sarsa.GetAction(currentState);
            }
            if (doubleQLearning != null) {
                return doubleQLearning.GetAction(currentState);
            }

            return doubleQLearning.GetAction(currentState);
        }


        #endregion

        #region 后台机器学习
        private void QLearningThread()
        {
            int iteration = 0;
            // 当前坐标的代理 curent coordinates of the agent
            int agentCurrentX, agentCurrentY;
            // 探索策略 exploration policy
            TabuSearchExploration tabuPolicy = (TabuSearchExploration)qLearning.ExplorationPolicy;
            EpsilonGreedyExploration explorationPolicy = (EpsilonGreedyExploration)tabuPolicy.BasePolicy;

            // loop
            while ((!needToStop) && (iteration < learningIterations)) {
                OldAction.Clear();
                // 为这个迭代设置勘探速率 set exploration rate for this iteration
                explorationPolicy.Epsilon = explorationRate - ((double)iteration / learningIterations) * explorationRate;
                // 为迭代设置学习率 set learning rate for this iteration
                qLearning.LearningRate = learningRate - ((double)iteration / learningIterations) * learningRate;
                // 清除tabu列表 clear tabu list
                tabuPolicy.ResetTabuList();

                //复位代理的坐标到起始位置 reset agent's coordinates to the starting position
                agentCurrentX = _agentStartX;
                agentCurrentY = _agentStartY;

                // 代理执行的步骤以达到目标 steps performed by agent to get to the goal
                int steps = 0;

                while ((!needToStop) && ((agentCurrentX != _agentStopX) || (agentCurrentY != _agentStopY))) {
                    steps++;

                    // 获取代理的当前状态 get agent's current state
                    int currentState = GetStateNumber(agentCurrentX, agentCurrentY);
                    // 获取此状态的操作 get the action for this state
                    int action = qLearning.GetAction(currentState);
                    tabuPolicy.ResetTabuList();

                    // 更新代理的当前位置并得到他的奖励 update agent's current position and get his reward
                    double reward = UpdateAgentPosition(ref agentCurrentX, ref agentCurrentY, action);
                    // 获取代理的下一个状态 get agent's next state
                    int nextState = GetStateNumber(agentCurrentX, agentCurrentY);
                    // 做学习代理 - 更新他的Q函数 do learning of the agent - update his Q-function
                    qLearning.UpdateState(currentState, action, reward, nextState);
                    OldAction.Add(Tuple.Create(currentState, action, reward, nextState));

                    // 设置tabu动作 set tabu action
                    tabuPolicy.SetTabuAction((action + 2) % 4, 1);
                }
                for (int i = OldAction.Count - 1; i >= 0; i--) {
                    var a = OldAction[i];
                    qLearning.UpdateState(a.Item1, a.Item2, a.Item3, a.Item4);
                }

                System.Diagnostics.Debug.WriteLine(steps);

                iteration++;

                // 显示当前迭代 show current iteration
                SetText(iterationBox, iteration.ToString());
            }

            // 启用设置控件 enable settings controls
            EnableControls(true);
        }
        private void DoubleQLearningThread()
        {
            miniSteps = int.MaxValue;
            MiniOldAction.Clear();
            int iteration = 0;
            // 当前坐标的代理 curent coordinates of the agent
            int agentCurrentX, agentCurrentY;
            // 探索策略 exploration policy
            TabuSearchExploration tabuPolicy = (TabuSearchExploration)doubleQLearning.ExplorationPolicy;
            EpsilonGreedyExploration explorationPolicy = (EpsilonGreedyExploration)tabuPolicy.BasePolicy;

            // loop
            while ((!needToStop) && (iteration < learningIterations)) {
                OldAction.Clear();
                // 为这个迭代设置勘探速率 set exploration rate for this iteration
                explorationPolicy.Epsilon = explorationRate - ((double)iteration / learningIterations) * explorationRate;
                // 为迭代设置学习率 set learning rate for this iteration
                doubleQLearning.LearningRate = learningRate - ((double)iteration / learningIterations) * learningRate;
                // 清除tabu列表 clear tabu list
                tabuPolicy.ResetTabuList();

                //复位代理的坐标到起始位置 reset agent's coordinates to the starting position
                agentCurrentX = _agentStartX;
                agentCurrentY = _agentStartY;

                // 代理执行的步骤以达到目标 steps performed by agent to get to the goal
                int steps = 0;

                while ((!needToStop) && ((agentCurrentX != _agentStopX) || (agentCurrentY != _agentStopY))) {
                    steps++;

                    // 获取代理的当前状态 get agent's current state
                    int currentState = GetStateNumber(agentCurrentX, agentCurrentY);
                    // 获取此状态的操作 get the action for this state
                    int action = doubleQLearning.GetAction(currentState);
                    tabuPolicy.ResetTabuList();

                    // 更新代理的当前位置并得到他的奖励 update agent's current position and get his reward
                    double reward = UpdateAgentPosition(ref agentCurrentX, ref agentCurrentY, action);
                    // 获取代理的下一个状态 get agent's next state
                    int nextState = GetStateNumber(agentCurrentX, agentCurrentY);
                    // 做学习代理 - 更新他的Q函数 do learning of the agent - update his Q-function
                    doubleQLearning.UpdateState(currentState, action, reward, nextState);

                    var tup = Tuple.Create(currentState, action, reward, nextState);
                    if (OldAction.Contains(tup)==false) {
                        OldAction.Add(tup);
                    }

                    // 设置tabu动作 set tabu action
                    tabuPolicy.SetTabuAction((action + 2) % 4, 1);
                }
                for (int i = OldAction.Count - 1; i >= 0; i--) {
                    var a = OldAction[i];
                    doubleQLearning.UpdateState(a.Item1, a.Item2, a.Item3, a.Item4);
                }
                if (steps<miniSteps) {
                    miniSteps = steps;
                    MiniOldAction.Clear();
                    for (int i = 0; i < OldAction.Count; i++) {
                        MiniOldAction.Add(OldAction[i]);
                    }
                } else {
                    for (int i = MiniOldAction.Count - 1; i >= 0; i--) {
                        var a = MiniOldAction[i];
                        doubleQLearning.UpdateState(a.Item1, a.Item2, a.Item3, a.Item4);
                    }
                }


                System.Diagnostics.Debug.WriteLine(steps);

                iteration++;

                // 显示当前迭代 show current iteration
                SetText(iterationBox, iteration.ToString());
            }

            // 启用设置控件 enable settings controls
            EnableControls(true);
        }
        private void SarsaThread()
        {
            int iteration = 0;
            // 当前坐标的代理 curent coordinates of the agent
            int agentCurrentX, agentCurrentY;
            // 探索策略 exploration policy
            TabuSearchExploration tabuPolicy = (TabuSearchExploration)sarsa.ExplorationPolicy;
            EpsilonGreedyExploration explorationPolicy = (EpsilonGreedyExploration)tabuPolicy.BasePolicy;

            // loop
            while ((!needToStop) && (iteration < learningIterations)) {
                // 为这个迭代设置勘探速率 set exploration rate for this iteration
                explorationPolicy.Epsilon = explorationRate - ((double)iteration / learningIterations) * explorationRate;
                // 为迭代设置学习率 set learning rate for this iteration
                sarsa.LearningRate = learningRate - ((double)iteration / learningIterations) * learningRate;
                // 清除tabu列表 clear tabu list
                tabuPolicy.ResetTabuList();

                // 复位代理的坐标到起始位置 reset agent's coordinates to the starting position
                agentCurrentX = _agentStartX;
                agentCurrentY = _agentStartY;

                // 代理执行的步骤以达到目标 steps performed by agent to get to the goal
                int steps = 1;
                // 以前的状态和动作 previous state and action
                int previousState = GetStateNumber(agentCurrentX, agentCurrentY);
                int previousAction = sarsa.GetAction(previousState);
                // 更新代理的当前位置并得到他的奖励 update agent's current position and get his reward
                double reward = UpdateAgentPosition(ref agentCurrentX, ref agentCurrentY, previousAction);

                while ((!needToStop) && ((agentCurrentX != _agentStopX) || (agentCurrentY != _agentStopY))) {
                    steps++;

                    // 设置禁忌动作 set tabu action
                    tabuPolicy.SetTabuAction((previousAction + 2) % 4, 1);

                    // 获取代理的下一个状态 get agent's next state
                    int nextState = GetStateNumber(agentCurrentX, agentCurrentY);
                    // 获取代理的下一个动作 get agent's next action
                    int nextAction = sarsa.GetAction(nextState);
                    // 做学习代理 - 更新他的Q函数 do learning of the agent - update his Q-function
                    sarsa.UpdateState(previousState, previousAction, reward, nextState, nextAction);

                    // 更新代理的新位置并得到他的奖励 update agent's new position and get his reward
                    reward = UpdateAgentPosition(ref agentCurrentX, ref agentCurrentY, nextAction);

                    previousState = nextState;
                    previousAction = nextAction;
                }

                if (!needToStop) {
                    // 如果达到终端状态，则更新Q函数 update Q-function if terminal state was reached
                    sarsa.UpdateState(previousState, previousAction, reward);
                }

                System.Diagnostics.Debug.WriteLine(steps);

                iteration++;

                // show current iteration
                SetText(iterationBox, iteration.ToString());
            }

            // enable settings controls
            EnableControls(true);
        }
        #endregion

        #region 控件操作
        private delegate void SetTextCallback(System.Windows.Forms.Control control, string text);
        private void SetText(System.Windows.Forms.Control control, string text)
        {
            if (control.InvokeRequired) {
                SetTextCallback d = new SetTextCallback(SetText);
                Invoke(d, new object[] { control, text });
            } else {
                control.Text = text;
            }
        }

        private delegate void EnableCallback(bool enable);
        private void EnableControls(bool enable)
        {
            if (InvokeRequired) {
                EnableCallback d = new EnableCallback(EnableControls);
                Invoke(d, new object[] { enable });
            } else {

                algorithmCombo.Enabled = enable;
                explorationRateBox.Enabled = enable;
                learningRateBox.Enabled = enable;
                iterationsBox.Enabled = enable;

                moveRewardBox.Enabled = enable;
                wallRewardBox.Enabled = enable;
                goalRewardBox.Enabled = enable;
                roundRewardBox.Enabled = enable;

                startLearningButton.Enabled = enable;
                showSolutionButton.Enabled = enable;
                stopButton.Enabled = !enable;
            }
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        private void GetSettings()
        {
            // explortion rate
            try {
                explorationRate = Math.Max(0.0, Math.Min(1.0, double.Parse(explorationRateBox.Text)));
            } catch {
                explorationRate = 0.5;
            }
            // learning rate
            try {
                learningRate = Math.Max(0.0, Math.Min(1.0, double.Parse(learningRateBox.Text)));
            } catch {
                learningRate = 0.5;
            }
            // learning iterations
            try {
                learningIterations = Math.Max(10, Math.Min(100000, int.Parse(iterationsBox.Text)));
            } catch {
                learningIterations = 100;
            }

            // move reward
            try {
                moveReward = Math.Max(-100, Math.Min(100, double.Parse(moveRewardBox.Text)));
            } catch {
                moveReward = 0;
            }
            // wall reward
            try {
                wallReward = Math.Max(-100, Math.Min(100, double.Parse(wallRewardBox.Text)));
            } catch {
                wallReward = -1;
            }
            // goal reward
            try {
                goalReward = Math.Max(-100, Math.Min(100, double.Parse(goalRewardBox.Text)));
            } catch {
                goalReward = 1;
            }
            try {
                roundReward = Math.Max(-100, Math.Min(100, double.Parse(roundRewardBox.Text)));
            } catch {
                roundReward = -0.1;
            }
        }
        /// <summary>
        /// 设置显示参数
        /// </summary>
        private void ShowSettings()
        {
            explorationRateBox.Text = explorationRate.ToString();
            learningRateBox.Text = learningRate.ToString();
            iterationsBox.Text = learningIterations.ToString();

            moveRewardBox.Text = moveReward.ToString();
            wallRewardBox.Text = wallReward.ToString();
            goalRewardBox.Text = goalReward.ToString();
            roundRewardBox.Text = roundReward.ToString();
        }
        #endregion
        private int miniSteps = int.MaxValue;
        private List<Tuple<int, int, double, int>> MiniOldAction = new List<Tuple<int, int, double, int>>();

        private List<Tuple<int, int, double, int>> OldAction = new List<Tuple<int, int, double, int>>();

        private double UpdateAgentPosition(ref int currentX, ref int currentY, int action)
        {
            //OldAction.Add(Tuple.Create(currentX, currentY, action));

            // 默认奖励等于移动奖励 default reward is equal to moving reward
            double reward = moveReward;
            // 移动方向 moving direction
            int dx = 0, dy = 0;

            switch (action) {
                case 0:         // go to north (up)
                    dy = -1;
                    break;
                case 1:         // go to east (right)
                    dx = 1;
                    break;
                case 2:         // go to south (down)
                    dy = 1;
                    break;
                case 3:         // go to west (left)
                    dx = -1;
                    break;
            }

            int newX = currentX + dx;
            int newY = currentY + dy;

            //检查新代理的坐标 check new agent's coordinates
            if (map[newX, newY] == 1) {
                // 我们发现了一堵墙或者在世界之外 we found a wall or got outside of the world
                reward = wallReward;
            } else {
                currentX = newX;
                currentY = newY;

                // 检查我们是否找到了目标 check if we found the goal
                if ((currentX == _agentStopX) && (currentY == _agentStopY))
                    reward = goalReward;
            }

            return reward;
        }



        // 在指定位置从代理的受体获取状态数 Get state number from agent's receptors in the specified position
        private int GetStateNumber(int x, int y)
        {
            //int c1 = (map[y - 1, x - 1] != 0) ? 1 : 0;
            int c2 = (map[x - 1, y] != 0) ? 1 : 0;
            //int c3 = (map[y - 1, x + 1] != 0) ? 1 : 0;
            int c4 = (map[x, y + 1] != 0) ? 1 : 0;
            //int c5 = (map[y + 1, x + 1] != 0) ? 1 : 0;
            int c6 = (map[x + 1, y] != 0) ? 1 : 0;
            //int c7 = (map[y + 1, x - 1] != 0) ? 1 : 0;
            int c8 = (map[x, y - 1] != 0) ? 1 : 0;
            int t1 = Math.Abs(_agentStopX - x);
            int t2 = Math.Abs(_agentStopY - y);

            var t = x * map.GetLength(1) + y;
            //t = t * 32 + c8;
            ////t = t * 2 + c7;
            //t = t * 2 + c6;
            ////t = t * 2 + c5;
            //t = t * 2 + c4;
            ////t = t * 2 + c3;
            //t = t * 2 + c2;
            ////t = t * 2 + c1;
            return t;

            //return c1 |
            //    (c2 << 1) |
            //    (c3 << 2) |
            //    (c4 << 3) |
            //    (c5 << 4) |
            //    (c6 << 5) |
            //    (c7 << 6) |
            //    (c8 << 7) |
            //        (c9 << 9) |
        }

        private void CreateNewMap()
        {
            var width = int.Parse(mapWidth.Text) / 2;
            var height = int.Parse(mapHeight.Text) / 2;
            int meshWidth = width * 2 + 1;
            int meshHeight = height * 2 + 1;

            Maze maze = new Maze(width, height);
            map = maze.GetIntArray();
            mapToDisplay = new int[meshWidth, meshHeight];

            var well = meshWidth * meshHeight * double.Parse(mapClear.Text) * 0.01;
            Random rand = new Random();
            while (well > 0) {
                var w = rand.Next(meshWidth - 2);
                var h = rand.Next(meshHeight - 2);
                if (map[w + 1, h + 1] == 1) {
                    map[w + 1, h + 1] = 0;
                    well--;
                }
            }

            Array.Copy(map, mapToDisplay, meshWidth * meshHeight);
            mapToDisplay[1, 1] = 2;
            do {
                _agentStopX = rand.Next(meshWidth - 2) + 1;
                _agentStopY = rand.Next(meshHeight - 2) + 1;
            } while (GetMapCanGo(_agentStopX, _agentStopY) == false || (_agentStopX < (5 / 3) * width && _agentStopY < (5 / 3) * height));
            mapToDisplay[_agentStopX, _agentStopY] = 3;

            this.cellWorld1.Map = mapToDisplay;
        }

        private bool GetMapCanGo(int x, int y)
        {
            if (map[x, y] == 1) return false;
            if (map[x + 1, y] == 1 && map[x - 1, y] == 1 && map[x, y + 1] == 1 && map[x, y - 1] == 1) return false;
            return true;
        }


    }
}
