namespace ToolGood.MazeGame.Windows
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.newMapButton = new System.Windows.Forms.Button();
            this.mapClear = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.mapHeight = new System.Windows.Forms.TextBox();
            this.mapWidth = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.algorithmCombo = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.goalRewardBox = new System.Windows.Forms.TextBox();
            this.explorationRateBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.wallRewardBox = new System.Windows.Forms.TextBox();
            this.learningRateBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.moveRewardBox = new System.Windows.Forms.TextBox();
            this.iterationsBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.showSolutionButton = new System.Windows.Forms.Button();
            this.iterationBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.stopButton = new System.Windows.Forms.Button();
            this.startLearningButton = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.roundRewardBox = new System.Windows.Forms.TextBox();
            this.cellWorld1 = new ToolGood.MazeGame.Controls.CellWorld();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cellWorld1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(360, 333);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "地图";
            // 
            // newMapButton
            // 
            this.newMapButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.newMapButton.Location = new System.Drawing.Point(235, 351);
            this.newMapButton.Name = "newMapButton";
            this.newMapButton.Size = new System.Drawing.Size(100, 23);
            this.newMapButton.TabIndex = 6;
            this.newMapButton.Text = "生成新的迷宫";
            this.newMapButton.UseVisualStyleBackColor = true;
            this.newMapButton.Click += new System.EventHandler(this.newMapButton_Click);
            // 
            // mapClear
            // 
            this.mapClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mapClear.Location = new System.Drawing.Point(202, 353);
            this.mapClear.Name = "mapClear";
            this.mapClear.Size = new System.Drawing.Size(27, 21);
            this.mapClear.TabIndex = 5;
            this.mapClear.Text = "7";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(142, 356);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "删墙(%)：";
            // 
            // mapHeight
            // 
            this.mapHeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mapHeight.Location = new System.Drawing.Point(51, 353);
            this.mapHeight.Name = "mapHeight";
            this.mapHeight.Size = new System.Drawing.Size(22, 21);
            this.mapHeight.TabIndex = 3;
            this.mapHeight.Text = "34";
            // 
            // mapWidth
            // 
            this.mapWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mapWidth.Location = new System.Drawing.Point(114, 353);
            this.mapWidth.Name = "mapWidth";
            this.mapWidth.Size = new System.Drawing.Size(22, 21);
            this.mapWidth.TabIndex = 2;
            this.mapWidth.Text = "30";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(79, 356);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "高：";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 356);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "长：";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.roundRewardBox);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.algorithmCombo);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.goalRewardBox);
            this.groupBox3.Controls.Add(this.explorationRateBox);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.wallRewardBox);
            this.groupBox3.Controls.Add(this.learningRateBox);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.moveRewardBox);
            this.groupBox3.Controls.Add(this.iterationsBox);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Location = new System.Drawing.Point(378, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(182, 235);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "设置";
            // 
            // algorithmCombo
            // 
            this.algorithmCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.algorithmCombo.FormattingEnabled = true;
            this.algorithmCombo.Items.AddRange(new object[] {
            "Q-Learning",
            "Sarsa",
            "Double-Q-Learning"});
            this.algorithmCombo.Location = new System.Drawing.Point(65, 24);
            this.algorithmCombo.Name = "algorithmCombo";
            this.algorithmCombo.Size = new System.Drawing.Size(111, 20);
            this.algorithmCombo.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 27);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 0;
            this.label10.Text = "学习算法：";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 51);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(89, 12);
            this.label11.TabIndex = 2;
            this.label11.Text = "初始勘探速度：";
            // 
            // goalRewardBox
            // 
            this.goalRewardBox.Location = new System.Drawing.Point(101, 179);
            this.goalRewardBox.Name = "goalRewardBox";
            this.goalRewardBox.Size = new System.Drawing.Size(60, 21);
            this.goalRewardBox.TabIndex = 13;
            this.goalRewardBox.Text = "1";
            // 
            // explorationRateBox
            // 
            this.explorationRateBox.Location = new System.Drawing.Point(101, 45);
            this.explorationRateBox.Name = "explorationRateBox";
            this.explorationRateBox.Size = new System.Drawing.Size(60, 21);
            this.explorationRateBox.TabIndex = 3;
            this.explorationRateBox.Text = "0.5";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(6, 182);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 14);
            this.label9.TabIndex = 12;
            this.label9.Text = "目标奖励：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "初始学习率：";
            // 
            // wallRewardBox
            // 
            this.wallRewardBox.Location = new System.Drawing.Point(101, 152);
            this.wallRewardBox.Name = "wallRewardBox";
            this.wallRewardBox.Size = new System.Drawing.Size(60, 21);
            this.wallRewardBox.TabIndex = 11;
            this.wallRewardBox.Text = "-1";
            // 
            // learningRateBox
            // 
            this.learningRateBox.Location = new System.Drawing.Point(101, 70);
            this.learningRateBox.Name = "learningRateBox";
            this.learningRateBox.Size = new System.Drawing.Size(60, 21);
            this.learningRateBox.TabIndex = 5;
            this.learningRateBox.Text = "0.5";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(6, 155);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 14);
            this.label8.TabIndex = 10;
            this.label8.Text = "墙奖：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 97);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "学习迭代：";
            // 
            // moveRewardBox
            // 
            this.moveRewardBox.Location = new System.Drawing.Point(101, 126);
            this.moveRewardBox.Name = "moveRewardBox";
            this.moveRewardBox.Size = new System.Drawing.Size(60, 21);
            this.moveRewardBox.TabIndex = 9;
            this.moveRewardBox.Text = "0";
            // 
            // iterationsBox
            // 
            this.iterationsBox.Location = new System.Drawing.Point(101, 95);
            this.iterationsBox.Name = "iterationsBox";
            this.iterationsBox.Size = new System.Drawing.Size(60, 21);
            this.iterationsBox.TabIndex = 7;
            this.iterationsBox.Text = "1000";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(6, 129);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 14);
            this.label7.TabIndex = 8;
            this.label7.Text = "移动奖励：";
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(6, 119);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(165, 2);
            this.label6.TabIndex = 14;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.showSolutionButton);
            this.groupBox2.Controls.Add(this.iterationBox);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.stopButton);
            this.groupBox2.Controls.Add(this.startLearningButton);
            this.groupBox2.Location = new System.Drawing.Point(378, 253);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(182, 132);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Learning";
            // 
            // showSolutionButton
            // 
            this.showSolutionButton.Enabled = false;
            this.showSolutionButton.Location = new System.Drawing.Point(10, 100);
            this.showSolutionButton.Name = "showSolutionButton";
            this.showSolutionButton.Size = new System.Drawing.Size(155, 21);
            this.showSolutionButton.TabIndex = 4;
            this.showSolutionButton.Text = "显示解决方案";
            this.showSolutionButton.UseVisualStyleBackColor = true;
            this.showSolutionButton.Click += new System.EventHandler(this.showSolutionButton_Click);
            // 
            // iterationBox
            // 
            this.iterationBox.Location = new System.Drawing.Point(65, 18);
            this.iterationBox.Name = "iterationBox";
            this.iterationBox.ReadOnly = true;
            this.iterationBox.Size = new System.Drawing.Size(100, 21);
            this.iterationBox.TabIndex = 1;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(10, 21);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 12);
            this.label12.TabIndex = 0;
            this.label12.Text = "迭代：";
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(10, 73);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(155, 21);
            this.stopButton.TabIndex = 3;
            this.stopButton.Text = "暂停";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startLearningButton
            // 
            this.startLearningButton.Enabled = false;
            this.startLearningButton.Location = new System.Drawing.Point(10, 46);
            this.startLearningButton.Name = "startLearningButton";
            this.startLearningButton.Size = new System.Drawing.Size(155, 21);
            this.startLearningButton.TabIndex = 2;
            this.startLearningButton.Text = "开始";
            this.startLearningButton.UseVisualStyleBackColor = true;
            this.startLearningButton.Click += new System.EventHandler(this.startLearningButton_Click);
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(6, 209);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(70, 14);
            this.label13.TabIndex = 15;
            this.label13.Text = "绕圈奖励：";
            // 
            // roundRewardBox
            // 
            this.roundRewardBox.Location = new System.Drawing.Point(101, 206);
            this.roundRewardBox.Name = "roundRewardBox";
            this.roundRewardBox.Size = new System.Drawing.Size(60, 21);
            this.roundRewardBox.TabIndex = 16;
            this.roundRewardBox.Text = "-0.1";
            // 
            // cellWorld1
            // 
            this.cellWorld1.Coloring = new System.Drawing.Color[] {
        System.Drawing.Color.White,
        System.Drawing.Color.Green,
        System.Drawing.Color.Black,
        System.Drawing.Color.Red,
        System.Drawing.Color.YellowGreen};
            this.cellWorld1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cellWorld1.Location = new System.Drawing.Point(3, 17);
            this.cellWorld1.Map = null;
            this.cellWorld1.Name = "cellWorld1";
            this.cellWorld1.Size = new System.Drawing.Size(354, 313);
            this.cellWorld1.TabIndex = 0;
            this.cellWorld1.Text = "cellWorld1";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(569, 390);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.newMapButton);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.mapClear);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.mapHeight);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mapWidth);
            this.Controls.Add(this.label2);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "迷宫游戏";
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private Controls.CellWorld cellWorld1;
        private System.Windows.Forms.Button newMapButton;
        private System.Windows.Forms.TextBox mapClear;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox mapHeight;
        private System.Windows.Forms.TextBox mapWidth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox algorithmCombo;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox goalRewardBox;
        private System.Windows.Forms.TextBox explorationRateBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox wallRewardBox;
        private System.Windows.Forms.TextBox learningRateBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox moveRewardBox;
        private System.Windows.Forms.TextBox iterationsBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button showSolutionButton;
        private System.Windows.Forms.TextBox iterationBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button startLearningButton;
        private System.Windows.Forms.TextBox roundRewardBox;
        private System.Windows.Forms.Label label13;
    }
}