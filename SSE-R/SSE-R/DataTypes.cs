using static SSE_R.Readers;
using static SSE_R.misc.LogFile;

namespace SSE_R
{
    public class DataTypes
    {
        public static void ArrayProperty(MemoryStream stream)
        {
            int arraySize = ReadInt32(stream);
            int index = ReadInt32(stream);
            string arrayType = ReadString(stream);
            stream.Seek(1, SeekOrigin.Current);
            int length = ReadInt32(stream);
            long structStartPos = 0;
            int size = 0;
            for (int i = 0; i < length; i++)
            {
                long position = stream.Position;
                switch (arrayType)
                {
                    case "ByteProperty\0":
                        int byteValue = stream.ReadByte(); break;
                    case "EnumProperty\0":
                    case "StrProperty\0":
                        string stringValue = ReadString(stream); break;
                    case "InterfaceProperty\0":
                    case "ObjectProperty\0":
                        string levelName = ReadString(stream); 
                        string pathName = ReadString(stream); break;
                    case "IntProperty\0":
                        int intValue = ReadInt32(stream); break;
                    case "Int64Property\0":
                        long longValue = readLong(stream); break;
                    case "StructProperty\0":
                        string name = ReadString(stream);
                        string type = ReadString(stream);
                        if (type != "StructProperty\0")
                        {
                            throw new Exception();
                        }
                        size = ReadInt32(stream);
                        stream.Seek(4, SeekOrigin.Current);
                        string elementType = ReadString(stream);
                        stream.Seek(17, SeekOrigin.Current);
                        structStartPos = stream.Position;
                        while (stream.Position < structStartPos + size)
                        {
                            string[] datatypes = { "Box\0", "FluidBox\0", "InventoryItem\0", "LinearColor\0", "Quat\0", "RailroadTrackPosition\0", "Vector\0" };
                            if (Array.IndexOf(datatypes, elementType) >= 0)
                            {
                                switch (elementType)
                                {
                                    case "Box\0": Box(stream); break;
                                    case "FluidBox\0": FluidBox(stream); break;
                                    case "InventoryItem\0": InventoryItem(stream); break;
                                    case "LinearColor\0": LinearColor(stream); break;
                                    case "Quat\0": Quat(stream); break;
                                    case "RailroadTrackPosition\0": RailRoadTrackPosition(stream); break;
                                    case "Vector\0": Vector(stream); break;
                                    default: throw new Exception();
                                }
                            }
                            else
                            {
                                ListReaders.TypedData(stream);
                            }
                        }
                        return;
                    default: throw new Exception();
                }
            }
            //string holder = ReadString(stream);
            //if (holder == "None" || holder == "None\0")
            //{
            //    return;
            //}
        }
        public static void BoolProperty(MemoryStream stream)
        {
            ReadInt32(stream);
            int index = ReadInt32(stream);
            int value = stream.ReadByte();
            stream.ReadByte();
        }
        public static void ByteProperty(MemoryStream stream)
        {
            int size = ReadInt32(stream);
            int index = ReadInt32(stream);
            string type = ReadString(stream);
            stream.ReadByte();
            if (type == "None\0")
            {
                int value = stream.ReadByte();
            }
            else
            {
                string value = ReadString(stream);
            }
        }
        public static void EnumProperty(MemoryStream stream)
        {
            int size = ReadInt32(stream);
            int index = ReadInt32(stream);
            string type = ReadString(stream);
            stream.ReadByte();
            string value = ReadString(stream);
        }
        public static void FloatProperty(MemoryStream stream)
        {
            int size = ReadInt32(stream);
            int index = ReadInt32(stream);
            stream.ReadByte();
            float value = readFloat(stream);
        }
        public static void IntProperty(MemoryStream stream)
        {
            int size = ReadInt32(stream);
            int index = ReadInt32(stream);
            stream.ReadByte();
            int value = ReadInt32(stream);
        }
        public static void Int8Property(MemoryStream stream)
        {
            int size = ReadInt32(stream);
            int index = ReadInt32(stream);
            stream.ReadByte();
            sbyte value = ReadInt8(stream);
        }
        public static void Int64Property(MemoryStream stream)
        {
            int size = ReadInt32(stream);
            int index = ReadInt32(stream);
            stream.ReadByte();
            long value = readLong(stream);
        }
        public static void MapProperty(MemoryStream stream)
        {
            int size = ReadInt32(stream);
            int index = ReadInt32(stream);
            string keyType = ReadString(stream);
            string valueType = ReadString(stream);
            stream.ReadByte();
            int modeType = ReadInt32(stream);
            int numElements = ReadInt32(stream);
            for (int i = 0; i < numElements; i++)
            {
                switch (keyType)
                {
                    case "ObjectProperty\0":
                        string levelName = ReadString(stream);
                        string pathName = ReadString(stream);
                        break;
                    case "IntProperty\0":
                        int intValue = ReadInt32(stream); break;
                    case "EnumProperty\0":
                        string stringValue = ReadString(stream); break;
                    default:
                        throw new Exception("unknown key type");
                }
                switch (valueType)
                {
                    case "ByteProperty\0":
                        int byteValue = stream.ReadByte();
                        break;
                    case "IntProperty\0":
                        int intValue = ReadInt32(stream);
                        break;
                    case "StructProperty\0":
                        ListReaders.ReadPropertyList(stream);
                        break;
                }
            }
        }
        public static void NameProperty(MemoryStream stream)
        {
            int size = ReadInt32(stream);
            int index = ReadInt32(stream);
            stream.ReadByte();
            string value = ReadString(stream);
        }
        public static void ObjectProperty(MemoryStream stream)
        {
            int size = ReadInt32(stream);
            int index = ReadInt32(stream);
            stream.ReadByte();
            string levelName = ReadString(stream);
            string pathName = ReadString(stream);
        }
        public static void SetProperty(MemoryStream stream)
        {
            int size = ReadInt32(stream);
            int index = ReadInt32(stream);
            string type = ReadString(stream);
            stream.Seek(5, SeekOrigin.Current);
            int length = ReadInt32(stream);
            for (int i = 0; i < length; i++)
            {
                switch (type)
                {
                    case "StructProperty\0":
                        float locationX = readFloat(stream);
                        float locationY = readFloat(stream);
                        float locationZ = readFloat(stream);
                        break;
                    default:
                        throw new Exception("unknown SetProperty type");
                }
            }
        }
        public static void StrProperty(MemoryStream stream)
        {
            int size = ReadInt32(stream);
            int index = ReadInt32(stream);
            stream.ReadByte();
            string value = ReadString(stream);
        }
        public static void StructProperty(MemoryStream stream)
        {
            int size = ReadInt32(stream);
            int index = ReadInt32(stream);
            string type = ReadString(stream);
            stream.Seek(17, SeekOrigin.Current);
            long dataStart = stream.Position;
            while (stream.Position - dataStart < size)
            {
                string[] datatypes = { "Box\0", "FluidBox\0", "InventoryItem\0", "LinearColor\0", "Quat\0", "RailroadTrackPosition\0", "Vector\0" };
                if (Array.IndexOf(datatypes, type) >= 0)
                {
                    switch (type)
                    {
                        case "Box\0": Box(stream); break;
                        case "FluidBox\0": FluidBox(stream); break;
                        case "InventoryItem\0": InventoryItem(stream); break;
                        case "LinearColor\0": LinearColor(stream); break;
                        case "Quat\0": Quat(stream); break;
                        case "RailroadTrackPosition\0": RailRoadTrackPosition(stream); break;
                        case "Vector\0": Vector(stream); break;
                        default: throw new Exception();
                    }
                }
                else
                {
                    ListReaders.TypedData(stream);
                }
            }
        }
        public static void TextProperty(MemoryStream stream)
        {
            int size = ReadInt32(stream);
            int index = ReadInt32(stream);
            stream.ReadByte();
            int flags = ReadInt32(stream);
            int historyType = stream.ReadByte();
            int isTextCultureVariant = ReadInt32(stream);
            string value = ReadString(stream);
        }

        //special data types for typed data start below
        public static void Box(MemoryStream stream)
        {
            float minX = readFloat(stream);
            float minY = readFloat(stream);
            float minZ = readFloat(stream);
            float maxX = readFloat(stream);
            float maxY = readFloat(stream);
            float maxZ = readFloat(stream);
            int isValid = stream.ReadByte();
        }
        public static void FluidBox(MemoryStream stream)
        {
            float value = readFloat(stream);
        }
        public static void InventoryItem(MemoryStream stream)
        {
            ReadInt32(stream);
            string itemType = ReadString(stream);
            string levelName = ReadString(stream);
            string pathName = ReadString(stream);
            string result = ReadString(stream);
            if (result == "NumItems\0")
            {
                stream.Seek(16, SeekOrigin.Current);
                IntProperty(stream);
            }
            else
            {
                stream.Seek(-result.Length, SeekOrigin.Current);
            }
        }
        public static void LinearColor(MemoryStream stream)
        {
            float r = readFloat(stream);
            float g = readFloat(stream);
            float b = readFloat(stream);
            float a = readFloat(stream);
        }
        public static void Quat(MemoryStream stream)
        {
            float x = readFloat(stream);
            float y = readFloat(stream);
            float z = readFloat(stream);
            float w = readFloat(stream);
        }
        public static void RailRoadTrackPosition(MemoryStream stream)
        {
            string levelName = ReadString(stream);
            string pathName = ReadString(stream);
            float offset = readFloat(stream);
            float forward = readFloat(stream);
        }
        public static void Vector(MemoryStream stream)
        {
            float x = readFloat(stream);
            float y = readFloat(stream);
            float z = readFloat(stream);
        }
    }
}
