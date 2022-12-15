using Controller;
using Model;
using System;
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
using System.Windows.Threading;

namespace WPF_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeController();
        }

        private void InitializeController()
        {
            Competition comp = new Competition();
            Data.Initialize(comp);
            Track data = Data.NextRace();
            Data.CurrentRace.DriversChanged += OnDriverChanged;

        }

        public void OnDriverChanged(object? sender, DriversChangedEventArgs e)
        {
            this.ImageDraw.Dispatcher.BeginInvoke(
            DispatcherPriority.Render,
            new Action(() =>
            {
                this.ImageDraw.Source = null;
                this.ImageDraw.Source = WPFVisualization.DrawTrack(e.Track);
            }));
        }
    }
}
