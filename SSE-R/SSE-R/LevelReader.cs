using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSE_R;

namespace SSE_R
{
    public class LevelReader
    {
        public List<int> GetOffsets(MemoryStream decompressedBody)
        {
            byte[] offsetBytes = new byte[4];
            decompressedBody.Seek(4, SeekOrigin.Begin);
            decompressedBody.Read(offsetBytes, 0, 4);
            int amountOfOffsets = BitConverter.ToInt32(offsetBytes, 0); 
            List<int> offsets = new List<int>();
            Debug.WriteLine("initiating read");
            decompressedBody.Position = 0;
            byte[] buffer = new byte[decompressedBody.Length];
            decompressedBody.Read(buffer, 0, buffer.Length);
            byte[] target = new byte[] { 0x4C, 0x65, 0x76, 0x65, 0x6C, 0x20, 0x2F }; // "level /" string
            int index = 0;
            while (index < buffer.Length)
            {
                index = Array.IndexOf(buffer, target[0], index);
                if (index == -1)
                {
                    break;
                }
                if (buffer[index + 1] == target[1] && index < buffer.Length - 7)
                {
                    if (buffer[index + 2] == target[2]) 
                    {
                        if (buffer[index + 3] == target[3])
                        {
                            if (buffer[index + 4] == target[4])
                            {
                                if (buffer[index + 5] == target[5])
                                {
                                    if (buffer[index + 6] == target[6])
                                    {
                                        offsets.Add(index-4);
                                        Debug.WriteLine("\nfound an offset\n");
                                        index += 6;
                                    }
                                }
                            }
                        }
                    }
                }
                if (buffer[index] == target[0])
                {
                    index++;
                }
                if (offsets.Count == amountOfOffsets)
                {
                    break;
                }
                
                Debug.WriteLine($"position is {index} of {buffer.Length}. Found {offsets.Count} offsets so far");
            }
            Debug.WriteLine($"should have found {amountOfOffsets} offsets");
            return offsets;
        }
        public void readLevel(MemoryStream DecompressedBody, long offset, bool isSublevel=true)
        {
            DecompressedBody.Position = offset;
            if (isSublevel)
            {
                string sublevelname = QOL.ReadString(DecompressedBody);
            }
            else { QOL.ReadInt32(DecompressedBody); }
            int OHaCS = QOL.ReadInt32(DecompressedBody);
            int objectHeaderCount = QOL.ReadInt32(DecompressedBody);
            int actorCount = 0;
            if (objectHeaderCount > 0)
            {
                for (int i = 0; i < objectHeaderCount; i++)
                {
                    int headerType = QOL.ReadInt32(DecompressedBody);
                    if (headerType == 0) // 0 is objectHeader
                    {
                        string typePath = QOL.ReadString(DecompressedBody);
                        string rootObject = QOL.ReadString(DecompressedBody);
                        string instanceName = QOL.ReadString(DecompressedBody);
                        string parentActorName = QOL.ReadString(DecompressedBody);
                    }
                    if (headerType == 1) // 1 is actorHeader
                    {
                        string typePath = QOL.ReadString(DecompressedBody);
                        string rootObject = QOL.ReadString(DecompressedBody);
                        string instanceName = QOL.ReadString(DecompressedBody);
                        int needTransform = QOL.ReadInt32(DecompressedBody);
                        float Xrotation = QOL.ReadFloat(DecompressedBody);
                        float Yrotation = QOL.ReadFloat(DecompressedBody);
                        float Zrotation = QOL.ReadFloat(DecompressedBody);
                        float Wrotation = QOL.ReadFloat(DecompressedBody);
                        float Xposition = QOL.ReadFloat(DecompressedBody);
                        float Yposition = QOL.ReadFloat(DecompressedBody);
                        float Zposition = QOL.ReadFloat(DecompressedBody);
                        float Xscale = QOL.ReadFloat(DecompressedBody);
                        float Yscale = QOL.ReadFloat(DecompressedBody);
                        float Zscale = QOL.ReadFloat(DecompressedBody);
                        int wasPlaced = QOL.ReadInt32(DecompressedBody);            // use unclear
                        actorCount++;
                    }
                    Debug.WriteLine($"object header {i + 1} of {objectHeaderCount} at {DecompressedBody.Position}");
                }
            }
            Debug.WriteLine($"Last header ended at {DecompressedBody.Position}");
            int collectablesCount = QOL.ReadInt32(DecompressedBody);
            if (collectablesCount > 0)
            {
                for (int i = 0; i < collectablesCount; i++)
                {
                    string levelName = QOL.ReadString(DecompressedBody);
                    string pathName = QOL.ReadString(DecompressedBody);
                }
            }
            int objectSize = QOL.ReadInt32(DecompressedBody);
            int objectCount = QOL.ReadInt32(DecompressedBody);
            if (objectCount > 0)
            {
                for (int i = 0; i < actorCount; i++)
                {
                    Debug.WriteLine($"Starting actor {i+1} of {actorCount} at {DecompressedBody.Position}; line 126 LevelReader.cs");
                    int size = QOL.ReadInt32(DecompressedBody);
                    string parentObjectRoot = QOL.ReadString(DecompressedBody);
                    string parentObjectName = QOL.ReadString(DecompressedBody);
                    int componentCount = QOL.ReadInt32(DecompressedBody);
                    for (int j  = 0; j < componentCount; j++)
                    {
                        string levelName = QOL.ReadString(DecompressedBody);
                        string pathName = QOL.ReadString(DecompressedBody);
                    }
                    bool foundListEnd = false;
                    while (!foundListEnd)
                    {
                        Debug.WriteLine($"position is {DecompressedBody.Position}; line 139 LevelReader.cs");
                        string propertyName = QOL.ReadString(DecompressedBody).Trim();
                        if (propertyName == "None")
                        {
                            foundListEnd = true;
                            QOL.ReadInt32(DecompressedBody);
                            break;
                        }
                        string propertyType = QOL.ReadString(DecompressedBody).Trim();
                        Debug.WriteLine($"Property starts at {DecompressedBody.Position}, type is {propertyType}; line 148 LevelReader.cs");
                        switch (propertyType)
                        {
                            case "ArrayProperty":
                                PropertyTypes.Array(DecompressedBody); break;
                            case "BoolProperty":
                                PropertyTypes.BoolProperty(DecompressedBody); break;
                            case "ByteProperty":
                                PropertyTypes.ByteProperty(DecompressedBody); break;
                            case "EnumProperty":
                                PropertyTypes.EnumProperty(DecompressedBody); break;
                            case "FloatProperty":
                                PropertyTypes.FloatProperty(DecompressedBody); break;
                            case "IntProperty":
                                PropertyTypes.IntProperty(DecompressedBody); break;
                            case "Int64Property":
                                PropertyTypes.Int64Property(DecompressedBody); break;
                            case "MapProperty":
                                PropertyTypes.MapProperty(DecompressedBody); break;
                            case "NameProperty":
                                PropertyTypes.NameProperty(DecompressedBody); break;
                            case "ObjectProperty":
                                PropertyTypes.ObjectProperty(DecompressedBody); break;
                            case "SetProperty":
                                PropertyTypes.SetProperty(DecompressedBody); break;
                            case "StrProperty":
                                PropertyTypes.StrProperty(DecompressedBody); break;
                            case "StructProperty":
                                PropertyTypes.StructProperty(DecompressedBody); break;
                            case "TextProperty":
                                PropertyTypes.TextProperty(DecompressedBody); break;
                            case "None":
                                foundListEnd = true;
                                break;
                        }
                    }
                }
                for (int i = 0; i < (objectCount - actorCount); i++)
                {
                    Debug.WriteLine($"starting component {i + 1} of {(objectCount - actorCount)} at {DecompressedBody.Position}; Line 187 LevelReader.cs");
                    int size = QOL.ReadInt32(DecompressedBody);
                    bool foundListEnd = false;
                    while (!foundListEnd)
                    {
                        string propertyName = QOL.ReadString(DecompressedBody).Trim();
                        if (propertyName == "None")
                        {
                            foundListEnd = true;
                            QOL.ReadInt32(DecompressedBody);
                            break;
                        }
                        string propertyType = QOL.ReadString(DecompressedBody).Trim();
                        switch (propertyType)
                        {
                            case "ArrayProperty":
                                PropertyTypes.Array(DecompressedBody); break;
                            case "BoolProperty":
                                PropertyTypes.BoolProperty(DecompressedBody); break;
                            case "ByteProperty":
                                PropertyTypes.ByteProperty(DecompressedBody); break;
                            case "EnumProperty":
                                PropertyTypes.EnumProperty(DecompressedBody); break;
                            case "FloatProperty":
                                PropertyTypes.FloatProperty(DecompressedBody); break;
                            case "IntProperty":
                                PropertyTypes.IntProperty(DecompressedBody); break;
                            case "Int64Property":
                                PropertyTypes.Int64Property(DecompressedBody); break;
                            case "MapProperty":
                                PropertyTypes.MapProperty(DecompressedBody); break;
                            case "NameProperty":
                                PropertyTypes.NameProperty(DecompressedBody); break;
                            case "ObjectProperty":
                                PropertyTypes.ObjectProperty(DecompressedBody); break;
                            case "SetProperty":
                                PropertyTypes.SetProperty(DecompressedBody); break;
                            case "StrProperty":
                                PropertyTypes.StrProperty(DecompressedBody); break;
                            case "StructProperty":
                                PropertyTypes.StructProperty(DecompressedBody); break;
                            case "TextPropert":
                                PropertyTypes.TextProperty(DecompressedBody); break;
                            case "None":
                                foundListEnd = true;
                                break;
                        }
                    }
                }
            }
        }
        public void readLevel(MemoryStream DecompressedBody, int offset)
        {
            long longOffset = (long)offset;
            readLevel(DecompressedBody, longOffset);
        }
    }
}
