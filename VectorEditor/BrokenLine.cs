using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VectorEditor
{
    [DataContract]
    class BrokenLine : Primitive
    {
        [DataMember]
        public int Points
        {
            get; set;
        }
        [DataMember]
        public List<int> GetListX
        {
            get; set;
        } = new List<int>();
        [DataMember]
        public List<int> GetListY
        {
            get; set;
        } = new List<int>();

        public BrokenLine(string n, int t, byte r, byte g, byte b)
        {
            Name = n;
            Points = 0;
            Thickness = t;
            Red = r;
            Green = g;
            Blue = b;
        }
        // добавление точки
        public void AddPoint(int x, int y)
        {
            Points += 1;
            GetListX.Add(x);
            GetListY.Add(y);
        }
        // замена точки - Трансформация
        public void ReplacePoint(int xOld, int yOld, int xNew, int yNew)
        {
            for (int i = 0; i < GetListX.Count; i++)
            {
                // если совпадение старых
                if (GetListX[i] == xOld && 
                    GetListY[i] == yOld)
                {
                    GetListX[i] = xNew;   
                    GetListY[i] = yNew;
                }
            }
        }

        // центр по Х
        public int MiddleX()
        {
            int sum = 0;
            foreach (var item in GetListX)
            {
                sum += item;
            }
            return sum / GetListX.Count;
        }
        // центр по Y
        public int MiddleY()
        {
            int sum = 0;
            foreach (var item in GetListY)
            {
                sum += item;
            }
            return sum / GetListY.Count;
        }
        // передвинуть фигуру по Х
        public void MoveX(int m)
        {
            for (int i = 0; i < GetListX.Count; i++)
            {
                GetListX[i] = GetListX[i] + m;
            }
        }
        // передвинуть фигуру по Y
        public void MoveY(int m)
        {
            for (int i = 0; i < GetListY.Count; i++)
            {
                GetListY[i] = GetListY[i] + m;
            }
        }
        public int[] CheckArea(int x,int y)
        {
            int radius = 10;
            for (int i = 0; i < GetListX.Count; i++)
            {
                if (x > GetListX[i] - radius && x < GetListX[i] + radius &&
                    y > GetListY[i] - radius && y < GetListY[i] + radius)
                {
                    return new int[] { GetListX[i], GetListY[i] };
                }
            }
            return null;
        }
    }
}
