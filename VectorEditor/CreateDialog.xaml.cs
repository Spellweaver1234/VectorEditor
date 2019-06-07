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
using System.Windows.Shapes;

namespace VectorEditor
{
    /// <summary>
    /// Логика взаимодействия для CreateDialog.xaml
    /// </summary>
    public partial class CreateDialog : Window
    {
        public CreateDialog()
        {
            InitializeComponent();
            slider1.Value = 200;
            slider2.Value = 50;
            slider3.Value = 200;
        }
        public int Thickness
        {
            get
            {
                return (int)slider.Value;
            }
        }
        public string NameLine
        {
            get
            {
                return textBox.Text;
            }
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        private void ShowColor(byte R, byte G, byte B)
        {
            rectangle.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));
        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ShowColor((byte)slider1.Value, (byte)slider2.Value, (byte)slider3.Value);
        }

        private void slider2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ShowColor((byte)slider1.Value, (byte)slider2.Value, (byte)slider3.Value);
        }

        private void slider3_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ShowColor((byte)slider1.Value, (byte)slider2.Value, (byte)slider3.Value);
        }
    }
}
