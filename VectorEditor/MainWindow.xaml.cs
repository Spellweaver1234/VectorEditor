using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;
using Newtonsoft.Json;

namespace VectorEditor
{

    public partial class MainWindow : Window
    {
        bool isDrawing = false;
        string mode = "Waiting";                                               
        List<BrokenLine> brokenLines = new List<BrokenLine>(); 
        int[] xy = null;

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
                    string name = "";
                    if (cd.NameLine.Trim().Length == 0) name = "без названия";
                    else name = cd.NameLine;
                    int thickness = cd.Thickness;
                    byte red = (byte)cd.slider1.Value;
                    byte green = (byte)cd.slider2.Value;
                    byte blue = (byte)cd.slider3.Value;
                    // новая линия название и толщина
                    brokenLines.Add(new BrokenLine(name, thickness, red, green, blue));
                    listB_elements.Items.Add(brokenLines[brokenLines.Count - 1].Name);
                }
            }
            else
            {                                   
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
                {   // удаление из листа
                    listB_elements.Items.RemoveAt(l);
                    // удаление объекта
                    brokenLines.RemoveAt(l);
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
                MessageBox.Show("Включен режим рисования");
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
                else
                {
                    MessageBox.Show("Для перемещения выберите элемент из списка");
                }
            }
            else
            {
                MessageBox.Show("Включен режим рисования");
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int x, y;
            // -140 панели настрек слева
            x = (int)(e.GetPosition(null).X - 140);                 
            y = (int)(e.GetPosition(null).Y);

            // в области рисования
            if (x > 0 && y > 0)
            {   // режим рисования
                if (isDrawing == true)                              
                {
                    if (mode == "NewBrokenLine")                    
                    {
                        int k = brokenLines.Count - 1;
                        brokenLines[k].AddPoint(x, y);              
                        Draw();                                     
                    }
                    if (mode == "NewCircle")                        
                    {

                    }
                    if (mode == "NewRectangle")                     
                    {

                    }
                }
                else
                {
                    if (mode == "Move" && listB_elements.SelectedIndex > -1)
                    {
                        int l = listB_elements.SelectedIndex;
                        int m_X = brokenLines[l].MiddleX();
                        int m_Y = brokenLines[l].MiddleY();

                        brokenLines[l].MoveX(x - m_X);
                        brokenLines[l].MoveY(y - m_Y);
                        Draw();
                    }
                    if (mode == "Transformation" &&
                        listB_elements.SelectedIndex > -1)
                    {
                        int l = listB_elements.SelectedIndex;
                        if (xy != null)
                        {
                            brokenLines[l].ReplacePoint(xy[0], xy[1], x, y);
                            xy = null;
                        }
                        else
                        {
                            // захват точки рядом 
                            xy = brokenLines[l].CheckArea(x, y);
                        }
                        Draw();
                    }
                }
            }
        }
        // кнопка очистка всего
        private void btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            Reset();
            Draw();
        }
        // метод отрисовки всех линий и всех примитивов
        private void Draw()
        {
            myCanvas.Children.Clear();
            foreach (BrokenLine prim in brokenLines)
            {
                Brush br = new SolidColorBrush(Color.FromRgb(prim.Red, prim.Green, prim.Blue));

                if (prim.Points > 1)
                {
                    for (int i = 0; i < prim.Points-1; i++)
                    {
                        Line line = new Line();
                        line.X1 = prim.GetListX[i];
                        line.Y1 = prim.GetListY[i];
                        line.X2 = prim.GetListX[i + 1];
                        line.Y2 = prim.GetListY[i + 1];

                        line.Stroke = br;
                        line.StrokeThickness = prim.Thickness;
                        myCanvas.Children.Add(line);
                    }
                }
            }
        }
        // обновление информации о примитиве
        private void listB_elements_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listB_elements.SelectedIndex>-1)
            {
                int l = listB_elements.SelectedIndex;
                tBoxName.Text = brokenLines[l].Name;
                sliderThickness.Value = brokenLines[l].Thickness;
                slider1.Value = brokenLines[l].Red;
                slider2.Value = brokenLines[l].Green;
                slider3.Value = brokenLines[l].Blue;
                labNumber.Content = "Точек = " + brokenLines[l].Points;
            }
        }
        // кнопка обновления 
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (listB_elements.SelectedIndex > -1)
            {
                int l = listB_elements.SelectedIndex;
                brokenLines[l].Name = tBoxName.Text;
                listB_elements.Items[l] = tBoxName.Text;
                brokenLines[l].Thickness = (int)sliderThickness.Value;
                brokenLines[l].Red = (byte)slider1.Value;
                brokenLines[l].Green = (byte)slider2.Value;
                brokenLines[l].Blue = (byte)slider3.Value;
                Draw();
            }
            else
            {
                MessageBox.Show("Элемент не выбран");
            }
        }
        // очистка всего
        private void Reset()
        {
            // канвас - линии
            myCanvas.Children.Clear();
            // массив 
            brokenLines.Clear();
            // лист
            listB_elements.Items.Clear();
            // контролы 
            tBoxName.Text = "";
            sliderThickness.Value = 2;
            slider1.Value = 0;
            slider2.Value = 0;
            slider3.Value = 0;
            labNumber.Content = "Точек = 0";
        }
        // загрузка из файла
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Reset();
                string path = openFileDialog.FileName;
                // чтение
                string buff;
                using (StreamReader sr = new StreamReader(path))
                {
                    buff = sr.ReadToEnd();
                }
                // десериализация
                brokenLines = JsonConvert.DeserializeObject<List<BrokenLine>>(buff);
                
                MessageBox.Show("Загружено");
                // заполнение листа примитивов
                foreach (var item in brokenLines)
                {
                    listB_elements.Items.Add(item.Name);
                }
                Draw();
            }
        }
        // сохранение в файл
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)                // 
            {
                string path = saveFileDialog.FileName + ".json";
                // сериализация
                string json = JsonConvert.SerializeObject(brokenLines, Formatting.Indented);
                // запись
                using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.UTF8))
                {
                    sw.WriteLine(json);
                }
                MessageBox.Show("Сохранено");
            }
        }
        // слайдеры
        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (listB_elements.SelectedIndex > -1)
            {
                int l = listB_elements.SelectedIndex;
                brokenLines[l].Red = (byte)slider1.Value;
                Draw();
            }
        }
        private void slider2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (listB_elements.SelectedIndex > -1)
            {
                int l = listB_elements.SelectedIndex;
                brokenLines[l].Green = (byte)slider2.Value;
                Draw();
            }
        }
        private void slider3_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (listB_elements.SelectedIndex > -1)
            {
                int l = listB_elements.SelectedIndex;
                brokenLines[l].Blue = (byte)slider3.Value;
                Draw();
            }
        }
        private void sliderThickness_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (listB_elements.SelectedIndex > -1)
            {
                int l = listB_elements.SelectedIndex;
                brokenLines[l].Thickness = (byte)sliderThickness.Value;
                Draw();
            }
        }
    }
}