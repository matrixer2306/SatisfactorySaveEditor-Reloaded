using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SSE_R
{
    public static class QOL
    {
        /// <summary>
        /// Compares an array of bytes to check if it corrosponds to a string
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Compare(byte[] a, string b)
        {
            string ArrayString = "";
            foreach(byte Byte in a)
            {
                ArrayString += (char)Byte;
            }
            if (ArrayString == b)
            {
                return true;
            }
            else { return false; }
        }
        /// <summary>
        /// Reads a string from the specified memorystream, advances position by 4 + the length of the string including a 0x00 trailbyte bytes
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static string ReadString(MemoryStream a)
        {
            string outputString = "";
            byte[] buffer = new byte[4];
            a.Read(buffer, 0, 4);
            int c = BitConverter.ToInt32(buffer, 0);
            byte[] stringBuffer = new byte[c];
            a.Read(stringBuffer, 0, c);
            foreach (byte Byte in stringBuffer)
            {
                outputString += (char)Byte;
            }
            outputString = outputString.Replace("\0", "");
            // if (outputString == "None")
            // {
            // 
            // }
            return outputString;
        }
        /// <summary>
        /// reads an int32 from the specified memorystream, advances position by 4 bytes
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int ReadInt32(MemoryStream a)
        {
            byte[] buffer = new byte[4];
            a.Read(buffer, 0, 4);
            return BitConverter.ToInt32(buffer, 0);
        }
        /// <summary>
        /// reads a float from the specified memorystream, advances position by 4 bytes
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static float ReadFloat(MemoryStream a)
        {
            byte[] buffer = new byte[4];
            a.Read(buffer, 0, 4);
            return BitConverter.ToSingle(buffer, 0);
        }
        /// <summary>
        /// reads an int64 from the specified memorystream, advances position by 8 bytes
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static long ReadLong(MemoryStream a)
        {
            byte[] buffer = new byte[8];
            a.Read(buffer, 0, 8);
            return BitConverter.ToInt64(buffer, 0);
        } 
        /// <summary>
        /// Reads typed data
        /// </summary>
        /// <param name="DecompressedBody"></param>
        /// <param name="elementType"></param>
        /// <exception cref="Exception"></exception>
        
        //DONE: updated, should now support structception
        public static void readTypedData(MemoryStream decompressedBody, string structElementType)
        {
            bool foundEnd = false;
            while (!foundEnd)
            {
                string name = QOL.ReadString(decompressedBody).Trim();
                if (name == "None")
                {
                    foundEnd = true;
                    break;
                }
                string type = QOL.ReadString(decompressedBody).Trim();
                //decompressedBody.Seek(17, SeekOrigin.Current);
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
                        foundEnd = true;
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
                            if (name == "None")
                            {
                                foundEnd = true;
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
        }
    }
}
