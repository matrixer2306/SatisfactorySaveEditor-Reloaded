using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Popups;

namespace SSE_R
{
    public class LevelSorter
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
            byte[] target = new byte[] { 0x4C, 0x65, 0x76, 0x65, 0x6C, 0x20, 0x2F };
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
                                        offsets.Add(index);
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
    }
}
