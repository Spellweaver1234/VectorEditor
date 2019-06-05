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
            comboBox.SelectedIndex = 0;
        }
        public int Thickness { get { return (int)slider.Value; } }
        public string NameLine { get { return textBox.Text; } }
        public string Color
        {
            get
            {
                ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
                return selectedItem.Content.ToString();
            }
        }
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
