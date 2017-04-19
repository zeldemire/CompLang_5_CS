using System;
using System.Reflection;
using System.Text;

namespace Serializer
{
    public class MySerializer
    {
        public static string Serialize(Object obj)
        {
            Type t = obj.GetType();
            StringBuilder sb = new StringBuilder();
            FieldInfo[] fieldInfos = t.GetFields(BindingFlags.Public | BindingFlags.Instance);

            for (int i = 0; i < fieldInfos.Length; i++)
            {
                sb.Append(fieldInfos[i].GetValue(obj).GetType());
                sb.Append(" ");
                sb.Append(fieldInfos[i].Name);
                sb.Append(" ");
                sb.Append(fieldInfos[i].GetValue(obj));
                sb.Append(" ");
            }
            return sb.ToString();
        }

        public static T Deserialize<T>(string str)
        {
            Type t = typeof(T);
            
            str = str.Trim();
            
            String[] parts = str.Split(' ');
            ConstructorInfo ctx2 = t.GetConstructor(new Type[]{});

            T obj = (T) ctx2?.Invoke(new object[] {});

            for (int i = 0; i < parts.Length; i += 3)
            {
                FieldInfo fieldInfo = t.GetField(parts[i + 1]);

                switch (parts[i])
                {
                    case "System.Int32":
                        fieldInfo.SetValue(obj, Convert.ToInt32(parts[i + 2]));
                        break;
                    case "System.Double":
                        fieldInfo.SetValue(obj, Convert.ToDouble(parts[i + 2]));
                        break;
                    case "System.Boolean":
                        fieldInfo.SetValue(obj, Convert.ToBoolean(parts[i + 2]));
                        break;
                }
            }
            return obj;
        }
    }

    public class Point
    {
        public bool x;
        public double y;
        public Point()
        {
            x = false;
            y = 0.0;
        }

        public Point(bool X, double Y)
        {
            x = X;
            y = Y;
        }

        public override String ToString()
        {
            return $"({x},{y})";
        }
    }

    public class Test
    {
        public static void Main(String[] args)
        {
            Point p1 = new Point(true, 1.9);
            String str1 = MySerializer.Serialize(p1);
            Console.WriteLine(str1);
            Point newPt = MySerializer.Deserialize<Point>(str1);
            Console.WriteLine(newPt);
        }
    }
}