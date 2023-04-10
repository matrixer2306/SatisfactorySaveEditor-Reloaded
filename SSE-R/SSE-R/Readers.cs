using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SSE_R.Readers;
using static SSE_R.DataTypes;
using static SSE_R.misc.LogFile;
using Microsoft.VisualBasic.Logging;
using System.Net;
using System.Diagnostics;

namespace SSE_R
{
    internal class Readers
    {
        public static int ReadInt32(MemoryStream stream)
        {
            byte[] data = new byte[4];
            stream.Read(data, 0, 4);
            return BitConverter.ToInt32(data, 0);
        }
        public static string ReadString(MemoryStream stream)
        {
            string output = "";
            int length = ReadInt32(stream);
            byte[] data = new byte[length];
            stream.Read(data, 0, length);
            foreach(byte b in data)
            {
                output += (char)b;
            }
            return output;
        }
        public static float readFloat(MemoryStream stream)
        {
            byte[] data = new byte[4];
            stream.Read(data, 0, 4);
            return BitConverter.ToSingle(data, 0);
        }
        public static long readLong(MemoryStream stream)
        {
            byte[] data = new byte[8];
            stream.Read(data, 0, 8);
            return BitConverter.ToInt64(data, 0);
        }
    }
    internal class ListReaders
    {
        public static void ReadPropertyList(MemoryStream stream)
        {
            while(true)
            {
                string name = ReadString(stream);
                if (name == "None\0")
                {
                    break;
                }
                string type = ReadString(stream);
                if (type == "None\0")
                {
                    break;
                }
                
                switch (type)
                {
                    case "ArrayProperty\0":
                        ArrayProperty(stream); break;
                    case "BoolProperty\0":
                        BoolProperty(stream); break;
                    case "ByteProperty\0":
                        ByteProperty(stream); break;
                    case "EnumProperty\0":
                        EnumProperty(stream); break;
                    case "FloatProperty\0":
                        FloatProperty(stream); break;
                    case "IntProperty\0":
                        IntProperty(stream); break;
                    case "Int64Property\0":
                        Int64Property(stream); break;
                    case "MapProperty\0":
                        MapProperty(stream); break;
                    case "NameProperty\0":
                        NameProperty(stream); break;
                    case "ObjectProperty\0":
                        ObjectProperty(stream); break;
                    case "SetProperty\0":
                        SetProperty(stream); break;
                    case "StrProperty\0":
                        StrProperty(stream); break;
                    case "StructProperty\0":
                        StructProperty(stream); break;
                    case "TextProperty\0":
                        TextProperty(stream); break;
                    default:
                        try
                        {
                            throw new Exception($"UnhandledDataTypeExecption: {type} is unhandled");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "invalid data type", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly | MessageBoxOptions.ServiceNotification, false);
                            break;
                        }
                }
            }
        }
        public static void TypedData(MemoryStream stream)
        {
            while (true)
            {
                string name = ReadString(stream);
                if (name == "None\0")
                {
                    break;
                }
                string type = ReadString(stream);
                if (type == "None\0")
                {
                    break;
                }
                TypedData(stream, type);
            }
        }
        public static void TypedData(MemoryStream stream, string type)
        {
            //while (true)
            //{
            //
                string[] arrayTypes = { "ArrayProperty\0", "BoolProperty\0", "ByteProperty\0", "EnumProperty\0", "FloatProperty\0", "IntProperty\0",
                                        "Int64Property\0", "MapProperty\0", "NameProperty\0", "ObjectProperty\0", "SetProperty\0", "StrPropert\0",
                                        "StructProperty\0", "TextProperty\0"};
                if(Array.IndexOf(arrayTypes, type) >= 0)
                {
                    switch (type)
                    {
                        case "ArrayProperty\0":
                            ArrayProperty(stream); break;
                        case "BoolProperty\0":
                            BoolProperty(stream); break;
                        case "ByteProperty\0":
                            ByteProperty(stream); break;
                        case "EnumProperty\0":
                            EnumProperty(stream); break;
                        case "FloatProperty\0":
                            FloatProperty(stream); break;
                        case "IntProperty\0":
                            IntProperty(stream); break;
                        case "Int64Property\0":
                            Int64Property(stream); break;
                        case "MapProperty\0":
                            MapProperty(stream); break;
                        case "NameProperty\0":
                            NameProperty(stream); break;
                        case "ObjectProperty\0":
                            ObjectProperty(stream); break;
                        case "SetProperty\0":
                            SetProperty(stream); break;
                        case "StrProperty\0":
                            StrProperty(stream); break;
                        case "StructProperty\0":
                            StructProperty(stream); break;
                        case "TextProperty\0":
                            TextProperty(stream); break;
                    }
                }
                else
                {
                    stream.Seek(17, SeekOrigin.Current);
                    switch (type)
                    {
                        case "Box\0":
                            Box(stream); break;
                        case "FluidBox\0":
                            FluidBox(stream); break;
                        case "InventoryItem\0":
                            InventoryItem(stream); break;
                        case "LinearColor\0":
                            LinearColor(stream); break;
                        case "Quat\0":
                            Quat(stream); break;
                        case "RailroadTrackPosition\0":
                            RailRoadTrackPosition(stream); break;
                        case "Vector\0":
                            Vector(stream); break;
                        default: ReadPropertyList(stream); break;
                    }
                }
            //}
        }
    }
    internal class LevelReaders
    {
        public static void ReadLevel(MemoryStream stream, long offset, bool isSubLevel = true)
        {
            stream.Position = offset;
            if (isSubLevel) // read name if it is a sublevel
            {
                string subLevelName = ReadString(stream);
            }
            int oHeaderandCollectablesSize = ReadInt32(stream);
            int objectHeaderCount = ReadInt32(stream);
            int actorHeaderCount = 0;
            for (int i = 0; i < objectHeaderCount; i++) //object headers
            {
                int headerType = ReadInt32(stream);
                if (headerType == 1)
                {
                    actorHeaderCount++;
                    string typePath = ReadString(stream);
                    string rootObject = ReadString(stream);
                    string instanceName = ReadString(stream);
                    int needTransform = ReadInt32(stream);
                    float rotX = readFloat(stream);
                    float rotY = readFloat(stream);
                    float rotZ = readFloat(stream);
                    float rotW = readFloat(stream);
                    float posX = readFloat(stream);
                    float posY = readFloat(stream);
                    float posZ = readFloat(stream);
                    float scaleX = readFloat(stream);
                    float scaleY = readFloat(stream);
                    float scaleZ = readFloat(stream);
                    int wasPlaced = ReadInt32(stream);
                }
                else
                {
                    string typePath = ReadString(stream);
                    string rootObject = ReadString(stream);
                    string instanceName = ReadString(stream);
                    string parentActor = ReadString(stream);
                }
            }
            int collectablesCount = ReadInt32(stream); //collectables
            for (int i = 0; i < collectablesCount; i++)
            {
                string levelName = ReadString(stream);
                string pathName = ReadString(stream);
            }
            int objectsSize = ReadInt32(stream);
            int objectsCount = ReadInt32(stream);
            for (int i = 0; i < objectsCount; i++) //objects, starting with the actor objects
            {
                if (i < actorHeaderCount)
                {
                    int size = ReadInt32(stream);
                    long position = stream.Position;
                    string parentObjecctRoot = ReadString(stream);
                    string parentObjectName = ReadString(stream);
                    int componentCount = ReadInt32(stream);
                    for (int k = 0; k < componentCount; k++)
                    {
                        string levelName = ReadString(stream);
                        string pathName = ReadString(stream);
                    }
                    ListReaders.ReadPropertyList(stream);
                    stream.Seek(size - (stream.Position - position), SeekOrigin.Current);
                }
                else
                {
                    int size = ReadInt32(stream) ;
                    long position = stream.Position;
                    ListReaders.ReadPropertyList(stream);
                    stream.Seek(size - (stream.Position - position), SeekOrigin.Current);
                }
            }
        }
    }
}
