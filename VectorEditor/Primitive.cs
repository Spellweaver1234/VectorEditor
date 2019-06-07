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
    [DataContract]
    public class Primitive
    {
        [DataMember]
        public string Name
        {
            get; set;
        }
        [DataMember]
        public int Thickness
        {
            get; set;
        }
        [DataMember]
        public byte Red
        {
            get; set;
        }
        [DataMember]
        public byte Green
        {
            get; set;
        }
        [DataMember]
        public byte Blue
        {
            get; set;
        }

        public Primitive()
        { }
    }
}