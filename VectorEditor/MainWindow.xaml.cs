using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Serialization;

namespace VectorEditor
{

    public partial class MainWindow : Window
    {
        bool isDrawing = false;
        string mode = "Waiting";
        int id = 0;                                             // ID фигуры
        List<Primitive> primitives = new List<Primitive>();     // лист с фигурами
        Point pt;                                               // точка для выбора

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_NewBrokenLine_Click(object sender, RoutedEventArgs e)
        {
            if (isDrawing == false)
            {
                CreateDialog cd = new CreateDialog();
                if (cd.ShowDialog() == true)
                {
                    isDrawing = true;
                    mode = "NewBrokenLine";
                    lab_mode.Content = "Линия с изломами";
                    btn_NewBrokenLine.Content = "Включено";
                    string name = cd.NameLine;
                    int thickness = cd.Thickness;
                    var br = Brushes.Black;
                    switch(cd.Color.ToString())
                    {
                        case "Чёрный": br = Brushes.Black; break;
                        case "Красный": br = Brushes.Red; break;
                        case "Зелёный": br = Brushes.Green; break;
                    }
                    if (name == "") name = "без названия";
                    primitives.Add(new Primitive(name, thickness, br));       // новая линия название и толщина
                    listB_elements.Items.Add(primitives[primitives.Count - 1].Name);
                }
            }
            else
            {
                id += 1;                                     // счётчик фигур +1
                isDrawing = false;
                mode = "Waiting";
                lab_mode.Content = "ожидание...";
                btn_NewBrokenLine.Content = "Линия с изломами";
            }
        }

        private void btn_DeleteElement_Click(object sender, RoutedEventArgs e)
        {
            if (isDrawing == false)
            {
                int l = listB_elements.SelectedIndex;
                if (l > -1)
                {
                    listB_elements.Items.RemoveAt(l);       // удаление из листа
                    primitives.RemoveAt(l);                 // удаление объекта
                    Draw();
                }
                else
                { MessageBox.Show("Для удаления элемента необходимо его выбрать в списке доступных элементов"); }
            }
            else
            {
                MessageBox.Show("Включен режим рисования...");
            }
        }

        private void btn_Transformation_Click(object sender, RoutedEventArgs e)
        {
            if (isDrawing == false)
            {
                int l = listB_elements.SelectedIndex;
                if (l > -1)
                {
                    mode = "Transformation";
                    lab_mode.Content = "Трансформация";
                }
                else
                { MessageBox.Show("Для Трансформации необходимо выбрать элемент из списка и включить режим Трансформации"); }
            }
            else
            {
                MessageBox.Show("Включен режим рисования...");
            }
        }

        private void btn_Move_Click(object sender, RoutedEventArgs e)
        {
            if (isDrawing == false)
            {
                if (listB_elements.SelectedIndex > -1)
                {
                    mode = "Move";
                    lab_mode.Content = "Перемещение";

                }
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int x, y;
            x = (int)(e.GetPosition(null).X - 140);             // -140 панели настрек слева
            y = (int)(e.GetPosition(null).Y);
            
            if (x > 0 && y > 0)                                 // в области рисования
            {
                if (isDrawing == true)                          // режим рисования
                {
                    if (mode == "NewBrokenLine")                // рисование ломаной линии
                    {
                        int k = primitives.Count - 1;
                        primitives[k].AddPoint(x, y);           // сохраняем точку
                        Draw();                                     // рисуем 
                    }
                    if (mode == "NewCircle")            // круг
                    {

                    }
                    if (mode == "NewRectangle")             // прямоуголник
                    {

                    }
                }
                else
                {
                    if (mode == "Move" && listB_elements.SelectedIndex > -1)
                    {
                        int t = listB_elements.SelectedIndex;
                        int m_X = primitives[t].MiddleX();
                        int m_Y = primitives[t].MiddleY();

                        primitives[t].MoveX(x - m_X);
                        primitives[t].MoveY(y - m_Y);
                        Draw();
                    }
                    if (mode == "Transformation" &&
                        listB_elements.SelectedIndex > -1 &&
                        pt != null)
                    {
                        int t = listB_elements.SelectedIndex;
                        primitives[t].ReplacePoint((int)pt.X, (int)pt.Y, x, y);
                        Draw();
                    }
                }
            }
        }
        private void btn_Clear_Click(object sender, RoutedEventArgs e)
        {

            Reset();
            Draw();
        }
        private void myCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (mode == "Transformation" &&
                isDrawing == false &&
                listB_elements.SelectedIndex > -1)
            {
                int l = listB_elements.SelectedIndex;
                Point point = e.GetPosition((UIElement)sender);              // точка фигуры
                for (int i = 0; i < primitives[l].GetListX.Count; i++)
                {
                    if (primitives[l].GetListX[i] == point.X &&
                        primitives[l].GetListY[i] == point.Y)               // если такая точка есть в массиве точек этого примитива
                    {
                        pt = point;
                        lab_helper.Content = pt.X + " " + pt.Y;
                        MessageBox.Show("Выберите новое место для точки", "Точка выбрана");
                        break;
                    }
                }
            }
        }

        private void Draw()                                     // метод отрисовки всех линий и всех примитивов
        {
            myCanvas.Children.Clear();
            foreach (Primitive prim in primitives)
            {
                if (prim.Points > 1)
                {
                    for (int i = 0; i < prim.Points-1; i++)
                    {
                        Line line = new Line();
                        line.X1 = prim.GetListX[i];
                        line.Y1 = prim.GetListY[i];
                        line.X2 = prim.GetListX[i + 1];
                        line.Y2 = prim.GetListY[i + 1];
                        line.Stroke = prim.Brush;// Brushes.LightSteelBlue;
                        line.StrokeThickness = prim.Thickness;
                        myCanvas.Children.Add(line);
                    }
                }
            }
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void listB_elements_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listB_elements.SelectedIndex>-1)
            {
                int l = listB_elements.SelectedIndex;
                tBoxName.Text = primitives[l].Name;
                sliderThickness.Value = primitives[l].Thickness;
                var br = primitives[l].Brush;
                if  (br == Brushes.Black)
                    comboBox.Text = "Чёрный";
                if (br == Brushes.Red)
                    comboBox.Text = "Красный";
                if (br == Brushes.Green)
                    comboBox.Text = "Зелёный";
                labNumber.Content = "Точек = " + primitives[l].Points;
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (listB_elements.SelectedIndex > -1)
            {
                int l = listB_elements.SelectedIndex;
                primitives[l].Name = tBoxName.Text;
                listB_elements.Items[l] = tBoxName.Text;
                primitives[l].Thickness = (int)sliderThickness.Value;
                var br = Brushes.Black;
                switch (comboBox.Text)
                {
                    case "Чёрный": br = Brushes.Black; break;
                    case "Красный": br = Brushes.Red; break;
                    case "Зелёный": br = Brushes.Green; break;
                }
                primitives[l].Brush = br;
                Draw();
            }
            else
            { MessageBox.Show("Элемент не выбран"); }
        }

        private void Reset()
        {
            primitives.Clear();
            listB_elements.Items.Clear();
            tBoxName.Text = "";
            sliderThickness.Value = 2;
            comboBox.Text = "";
            labNumber.Content = "Точек = 0";
        }
        
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {

            }

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)                // 
            {

                XmlSerializer formatter = new XmlSerializer(typeof(List<Primitive>));

                using (FileStream fs = new FileStream("people.xml", FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, primitives);
                }

                //using (FileStream fs = new FileStream("people.xml", FileMode.OpenOrCreate))
                //{
                //    Primitive[] newpeople = (Primitive[])formatter.Deserialize(fs);

                //    foreach ( p in newpeople)
                //    {
                //        Console.WriteLine("Имя: {0} --- Возраст: {1}", p.Name, p.Age);
                //    }
                //}
            }
        }

    }
}