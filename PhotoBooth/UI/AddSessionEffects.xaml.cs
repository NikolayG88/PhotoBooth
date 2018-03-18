﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PhotoBooth
{
    /// <summary>
    /// Interaction logic for AddSessionEffects.xaml
    /// </summary>
    public partial class AddSessionEffects : Page
    {
        public List<Photo> CurrentSessionPhotos;
        public AddSessionEffects(List<Photo> sessionPoses)
        {
            InitializeComponent();

            CurrentSessionPhotos = sessionPoses;
        }
    }
}