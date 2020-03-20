﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SearchAlgorythms.Algorythms;
using SearchAlgorythms.Algorythms.GraphCreateAlgorythm;
using SearchAlgorythms.Graph;

namespace SearchAlgorythms.GraphLoader
{
    public class ButtonGraphLoader : IGraphLoader
    {
        private readonly int buttonWidth;
        private readonly int buttonHeight;
        private readonly int placeBetweenButtons;
        private IGraph graph;

        public ButtonGraphLoader(int buttonWidth,
            int buttonHeight, int placeBetweenButtons)
        {
            this.buttonWidth = buttonWidth;
            this.buttonHeight = buttonHeight;
            this.placeBetweenButtons = placeBetweenButtons;
        }

        public IGraph GetGraph()
        {
            IGraphTopInfo[,] info = null;
            OpenFileDialog open = new OpenFileDialog();
            BinaryFormatter f = new BinaryFormatter();
            if (open.ShowDialog() == DialogResult.OK)
                using (var stream = new FileStream(open.FileName, FileMode.Open))
                {
                    try
                    {
                        info = (IGraphTopInfo[,])f.Deserialize(stream);
                        Initialise(info);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return null;
                    }
                }
            return graph;
        }

        private void Initialise(IGraphTopInfo[,] info)
        {
            OnInfoButtonGraphCreater creator =
                new OnInfoButtonGraphCreater(info, buttonWidth, buttonHeight, placeBetweenButtons);
            if (info == null)
                return;
            graph = new ButtonGraph(creator.GetGraph());
            NeigbourSetter setter = new NeigbourSetter(graph.GetArray());
            setter.SetNeighbours();
        }

    }
}
