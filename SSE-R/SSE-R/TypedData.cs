using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SSE_R;

namespace SSE_R
{
    public class TypedData
    {
        public static void Box(MemoryStream decompressedBody)
        {
            float minX = QOL.ReadFloat(decompressedBody);
            float minY = QOL.ReadFloat(decompressedBody);
            float minZ = QOL.ReadFloat(decompressedBody);
            float maxX = QOL.ReadFloat(decompressedBody);
            float maxY = QOL.ReadFloat(decompressedBody);
            float maxZ = QOL.ReadFloat(decompressedBody);
            int isValid = decompressedBody.ReadByte();
        }
        public static void FluidBox(MemoryStream decompressedBody)
        {
            float value = QOL.ReadFloat(decompressedBody);
        }
        public static void InventoryItem(MemoryStream decompressedBody, bool isPayload=false)
        {
            QOL.ReadInt32(decompressedBody);
            string itemType = QOL.ReadString(decompressedBody);
            string levelName = QOL.ReadString(decompressedBody);
            string pathName = QOL.ReadString(decompressedBody);
            if (isPayload)
            {
                string propertyName = QOL.ReadString(decompressedBody);
                string propertyType = QOL.ReadString(decompressedBody);
                PropertyTypes.IntProperty(decompressedBody);
            }
        }
        public static void LinearColor(MemoryStream decompressedBody)
        {
            float r = QOL.ReadFloat(decompressedBody);
            float g = QOL.ReadFloat(decompressedBody);
            float b = QOL.ReadFloat(decompressedBody);
            float a = QOL.ReadFloat(decompressedBody);
        }
        public static void Quat(MemoryStream decompressedBody)
        {
            float x = QOL.ReadFloat(decompressedBody);
            float y = QOL.ReadFloat(decompressedBody);
            float z = QOL.ReadFloat(decompressedBody);
            float w = QOL.ReadFloat(decompressedBody);
        }
        public static void RailRoadTrackPosition(MemoryStream decompressedBody)
        {
            string levelName = QOL.ReadString(decompressedBody);
            string pathName = QOL.ReadString(decompressedBody);
            float offset = QOL.ReadFloat(decompressedBody);
            float forward = QOL.ReadFloat(decompressedBody);
        }
        public static void Vector(MemoryStream decompressedBody)
        {
            float x = QOL.ReadFloat(decompressedBody);
            float y = QOL.ReadFloat(decompressedBody);
            float z = QOL.ReadFloat(decompressedBody);
        }
    }
    public class PropertyTypes
    {
        public static void Array(MemoryStream decompressedBody)
        {
            int size = QOL.ReadInt32(decompressedBody);
            int index = QOL.ReadInt32(decompressedBody);
            string elementType = QOL.ReadString(decompressedBody).Trim();
            decompressedBody.ReadByte();
            int numElements = QOL.ReadInt32(decompressedBody);
            for (int i = 0; i < numElements; i++)
            {
                switch(elementType)
                {
                    case "ByteProperty":
                        int byteValue = decompressedBody.ReadByte();
                        break;
                    case "EnumProperty":
                        string enumValue = QOL.ReadString(decompressedBody);
                        break;
                    case "InterfaceProperty":
                    case "ObjectProperty":
                        string levelName = QOL.ReadString(decompressedBody);
                        string pathName = QOL.ReadString(decompressedBody);
                        break;
                    case "IntProperty":
                        int intValue = QOL.ReadInt32(decompressedBody);
                        break;
                    case "StructProperty":
                        string name = QOL.ReadString(decompressedBody);
                        string type = QOL.ReadString(decompressedBody);
                        int structSize = QOL.ReadInt32(decompressedBody);
                        QOL.ReadInt32(decompressedBody);
                        string structElementType = QOL.ReadString(decompressedBody).Trim();
                        decompressedBody.Seek(17, SeekOrigin.Current);
                        QOL.readTypedData(decompressedBody, structElementType);
                        break;
                }
            }
        }
        public static void BoolProperty(MemoryStream decompressedBody)
        {
            QOL.ReadInt32(decompressedBody);
            int index = QOL.ReadInt32(decompressedBody);
            int value = decompressedBody.ReadByte();
            decompressedBody.ReadByte();
        }
        public static void ByteProperty(MemoryStream decompressedBody)
        {
            int size = QOL.ReadInt32(decompressedBody);
            int index = QOL.ReadInt32(decompressedBody);
            string type = QOL.ReadString(decompressedBody).Trim();
            decompressedBody.ReadByte();
            if (type == "None")
            {
                int value = decompressedBody.ReadByte();
            }
            else
            {
                string value = QOL.ReadString(decompressedBody);
            }
        }
        public static void EnumProperty(MemoryStream decompressedBody)
        {
            int size = QOL.ReadInt32(decompressedBody);
            int index = QOL.ReadInt32(decompressedBody);
            string type = QOL.ReadString(decompressedBody);
            decompressedBody.ReadByte();
            string value = QOL.ReadString(decompressedBody);
        }
        public static void FloatProperty(MemoryStream decompressedBody)
        {
            int size = QOL.ReadInt32(decompressedBody);
            int index = QOL.ReadInt32(decompressedBody);
            decompressedBody.ReadByte();
            float value = QOL.ReadFloat(decompressedBody);
        }
        public static void IntProperty(MemoryStream decompressedBody)
        {
            int size = QOL.ReadInt32(decompressedBody);
            int index = QOL.ReadInt32(decompressedBody);
            decompressedBody.ReadByte();
            int value = QOL.ReadInt32(decompressedBody);
        }
        public static void Int64Property(MemoryStream decompressedBody)
        {
            int size = QOL.ReadInt32(decompressedBody);
            int index = QOL.ReadInt32(decompressedBody);
            decompressedBody.ReadByte();
            long value = QOL.ReadLong(decompressedBody);
        }
        public static void MapProperty(MemoryStream decompressedBody)
        {
            int size = QOL.ReadInt32(decompressedBody);
            int index = QOL.ReadInt32(decompressedBody);
            string keyType = QOL.ReadString(decompressedBody).Trim();
            string valueType = QOL.ReadString(decompressedBody).Trim();
            decompressedBody.ReadByte();
            int modeType = QOL.ReadInt32(decompressedBody);
            int numElements = QOL.ReadInt32(decompressedBody);
            var value = QOL.ReadInt32(decompressedBody);
            for (int i = 0; i < numElements; i++)
            {
                switch (keyType)
                {
                    case "ObjectProperty": 
                        string levelName = QOL.ReadString(decompressedBody);
                        string pathName = QOL.ReadString(decompressedBody);
                        break;
                    default: throw new Exception("unknown key type; line 172 TypedData.cs");
                }
                switch (valueType)
                {
                    case "ByteProperty":
                        value = decompressedBody.ReadByte();
                        break;
                    case "IntProperty":
                        value = QOL.ReadInt32(decompressedBody);
                        break;
                    case "StructProperty":
                        PropertyTypes.StructProperty(decompressedBody);
                        break;
                }
            }
        }
        public static void NameProperty(MemoryStream decompressedBody)
        {
            int size = QOL.ReadInt32(decompressedBody);
            int index = QOL.ReadInt32(decompressedBody);
            decompressedBody.ReadByte();
            string value = QOL.ReadString(decompressedBody);
        }
        public static void ObjectProperty(MemoryStream decompressedBody)
        {
            int size = QOL.ReadInt32(decompressedBody);
            int index = QOL.ReadInt32(decompressedBody);
            decompressedBody.ReadByte();
            string levelName = QOL.ReadString(decompressedBody);
            string pathName = QOL.ReadString(decompressedBody);
        }
        public static void SetProperty(MemoryStream decompressedBody)
        {
            int size = QOL.ReadInt32(decompressedBody);
            int index = QOL.ReadInt32(decompressedBody);
            string type = QOL.ReadString(decompressedBody).Trim();
            decompressedBody.Seek(17, SeekOrigin.Current);
            int length = QOL.ReadInt32(decompressedBody);
            for (int i = 0; i <  length; i++)
            {
                switch (type)
                {
                    case "StructProperty":
                        float x = QOL.ReadFloat(decompressedBody);
                        float y = QOL.ReadFloat(decompressedBody);
                        float z = QOL.ReadFloat(decompressedBody);
                        break;
                    default: throw new Exception("unknown type; line 221 TypedData.cs");
                }
            }
        }
        public static void StrProperty(MemoryStream decompressedBody)
        {
            int size = QOL.ReadInt32(decompressedBody);
            int index = QOL.ReadInt32(decompressedBody);
            decompressedBody.ReadByte();
            string value = QOL.ReadString(decompressedBody);
        }
        public static void StructProperty(MemoryStream decompressedBody)
        {
            int size = QOL.ReadInt32(decompressedBody);
            int index = QOL.ReadInt32(decompressedBody);
            string type = QOL.ReadString(decompressedBody).Trim();
            decompressedBody.Seek(17, SeekOrigin.Current);
            //QOL.readTypedData(decompressedBody, type);
            switch (type)
            {
                case "Box":
                    TypedData.Box(decompressedBody);
                    break;
                case "FluidBox":
                    TypedData.FluidBox(decompressedBody);
                    break;
                case "InventoryItem":
                    TypedData.InventoryItem(decompressedBody);
                    break;
                case "LinearColor":
                    TypedData.LinearColor(decompressedBody);
                    break;
                case "Quat":
                    TypedData.Quat(decompressedBody);
                    break;
                case "RailRoadTrackPosition":
                    TypedData.RailRoadTrackPosition(decompressedBody);
                    break;
                case "Vector":
                    TypedData.Vector(decompressedBody);
                    break;
                case "None":
                    break;
                case "ArrayProperty":
                    PropertyTypes.Array(decompressedBody); break;
                case "BoolProperty":
                    PropertyTypes.BoolProperty(decompressedBody); break;
                case "ByteProperty":
                    PropertyTypes.ByteProperty(decompressedBody); break;
                case "EnumProperty":
                    PropertyTypes.EnumProperty(decompressedBody); break;
                case "FloatProperty":
                    PropertyTypes.FloatProperty(decompressedBody); break;
                case "IntProperty":
                    PropertyTypes.IntProperty(decompressedBody); break;
                case "Int64Property":
                    PropertyTypes.Int64Property(decompressedBody); break;
                case "MapProperty":
                    PropertyTypes.MapProperty(decompressedBody); break;
                case "NameProperty":
                    PropertyTypes.NameProperty(decompressedBody); break;
                case "ObjectProperty":
                    PropertyTypes.ObjectProperty(decompressedBody); break;
                case "SetProperty":
                    PropertyTypes.SetProperty(decompressedBody); break;
                case "StrProperty":
                    PropertyTypes.StrProperty(decompressedBody); break;
                case "StructProperty":
                    PropertyTypes.StructProperty(decompressedBody); break;
                case "TextProperty":
                    PropertyTypes.TextProperty(decompressedBody); break;
                default:
                    bool foundListEnd = false;
                    while (!foundListEnd)
                    {
                        string propertyName = QOL.ReadString(decompressedBody).Trim();
                        if (propertyName == "None")
                        {
                            foundListEnd = true;
                            break;
                        }
                        string propertyType = QOL.ReadString(decompressedBody).Trim();
                        switch (propertyType)
                        {
                            case "ArrayProperty":
                                PropertyTypes.Array(decompressedBody); break;
                            case "BoolProperty":
                                PropertyTypes.BoolProperty(decompressedBody); break;
                            case "ByteProperty":
                                PropertyTypes.ByteProperty(decompressedBody); break;
                            case "EnumProperty":
                                PropertyTypes.EnumProperty(decompressedBody); break;
                            case "FloatProperty":
                                PropertyTypes.FloatProperty(decompressedBody); break;
                            case "IntProperty":
                                PropertyTypes.IntProperty(decompressedBody); break;
                            case "Int64Property":
                                PropertyTypes.Int64Property(decompressedBody); break;
                            case "MapProperty":
                                PropertyTypes.MapProperty(decompressedBody); break;
                            case "NameProperty":
                                PropertyTypes.NameProperty(decompressedBody); break;
                            case "ObjectProperty":
                                PropertyTypes.ObjectProperty(decompressedBody); break;
                            case "SetProperty":
                                PropertyTypes.SetProperty(decompressedBody); break;
                            case "StrProperty":
                                PropertyTypes.StrProperty(decompressedBody); break;
                            case "StructProperty":
                                PropertyTypes.StructProperty(decompressedBody); break;
                            case "TextProperty":
                                PropertyTypes.TextProperty(decompressedBody); break;
                            case "None":
                                foundListEnd = true;
                                break;
                        }
                    }
                    break;
            }
        }
        public static void TextProperty(MemoryStream decompressedBody)
        {
            int size = QOL.ReadInt32(decompressedBody);
            int index = QOL.ReadInt32(decompressedBody);
            decompressedBody.ReadByte();
            int flags = QOL.ReadInt32(decompressedBody);
            int historyType = decompressedBody.ReadByte();
            int isTextCultureVariant = QOL.ReadInt32(decompressedBody);
            string value = QOL.ReadString(decompressedBody);
        }
    }
}
