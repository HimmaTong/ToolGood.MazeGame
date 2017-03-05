using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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


        public MainWindow()
        {
            InitializeComponent();
        }

        private void newMapButton_Click(object sender, EventArgs e)
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
