using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace VectorEditor
{
    class Primitive
    {
        public string Name { get; set; }

        public int Thickness { get; set; }

        public string Color { get; set; }

        public int Points { get; set; }
  
        public SolidColorBrush Brush { get; set; }

        public Primitive()  { }

        public Primitive(string n, int t, SolidColorBrush b)
        {
            Name = n;
            Points = 0;
            Thickness = t;
            Brush = b;
        }

        public void AddPoint(int x, int y)   // метод добавления точек
        {
            Points += 1;
            GetListX.Add(x);
            GetListY.Add(y);
        }
        public void ReplacePoint(int xOld, int yOld, int xNew, int yNew)            // замена старой точки на новую
        {
            for (int i = 0; i < GetListX.Count; i++)
            {
                if (GetListX[i] == xOld &&              // если совпадают старые
                    GetListY[i] == yOld)
                {
                    GetListX[i] = xNew;                 // заменяю на новые
                    GetListY[i] = yNew;
                }
            }
        }
        public List<int> GetListX { get; } = new List<int>();
        public List<int> GetListY { get; } = new List<int>();
        public int MiddleX()                                    // средний Х
        {
            int sum = 0;
            foreach (var item in GetListX)
            {
                sum += item;
            }
            return sum / GetListX.Count;
        }
        public int MiddleY()                                    // средний Y
        {
            int sum = 0;
            foreach (var item in GetListY)
            {
                sum += item;
            }
            return sum / GetListY.Count;
        }
        public void MoveX(int m)
        {
            for (int i = 0; i < GetListX.Count; i++)
            {
                GetListX[i] = GetListX[i] + m;
            }
        }
        public void MoveY(int m)
        {
            for (int i = 0; i < GetListY.Count; i++)
            {
                GetListY[i] = GetListY[i] + m;
            }
        }
    }
}