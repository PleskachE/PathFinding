﻿using GraphLibrary.Common.Constants;
using System.Windows;

namespace WpfVersion.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для PathFindParametresWindow.xaml
    /// </summary>
    public partial class PathFindWindow : Window
    {
        public PathFindWindow()
        {
            InitializeComponent();
            delayTimeSlider.Minimum = Range.DelayValueRange.LowerRange;
            delayTimeSlider.Maximum = Range.DelayValueRange.UpperRange;
        }
    }
}
