using static SSE_R.Readers;

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
            for (int i = 0; i < length; i++)
            {
                switch (arrayType)
                {
                    case "ByteProperty":
                        int byteValue = stream.ReadByte(); break;
                    case "EnumProperty":
                        string stringValue = ReadString(stream); break;
                    case "InterfaceProperty":
                    case "ObjectProperty":
                        string levelName = ReadString(stream); string pathName = ReadString(stream); break;
                    case "IntProperty":
                        int intValue = ReadInt32(stream); break;
                    case "StructProperty":
                        string name = ReadString(stream);
                        string type = ReadString(stream);
                        int size = ReadInt32(stream);
                        stream.Seek(4, SeekOrigin.Current);
                        string elementType = ReadString(stream);
                        ReadInt32(stream);
                        ReadInt32(stream);
                        ReadInt32(stream);
                        ReadInt32(stream);
                        stream.ReadByte();
                        ListReaders.TypedData(stream);
                        break;
                }
            }
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
            if (type == "None")
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
                    case "ObjectProperty":
                        string levelName = ReadString(stream);
                        string pathName = ReadString(stream);
                        break;
                    default:
                        throw new Exception("unknown key type");
                }
                switch (valueType)
                {
                    case "ByteProperty":
                        int byteValue = stream.ReadByte();
                        break;
                    case "IntProperty":
                        int intValue = stream.ReadByte();
                        break;
                    case "StructProperty":
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
                    case "StructProperty":
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
            if(type == "InventoryItem\0")
            {
                InventoryItem(stream);
            }
            else
            {
                ListReaders.TypedData(stream);
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
