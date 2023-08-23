using Windows.Media.Playback;
using static SSE_R.misc;
using static SSE_R.misc.LogFile;

namespace SSE_R
{
    public class Finder
    {
        public static List<SubLevel> FindSubLevels(MemoryStream decompressedBody)
        {
            byte[] offsetBytes = new byte[4];
            decompressedBody.Seek(4, SeekOrigin.Begin);
            decompressedBody.Read(offsetBytes, 0, 4);
            int amountOfOffsets = BitConverter.ToInt32(offsetBytes, 0);

            List<SubLevel> subLevels = new List<SubLevel>();
            byte[] buffer = decompressedBody.ToArray();
            byte[] target = new byte[] { 0x4C, 0x65, 0x76, 0x65, 0x6C, 0x20, 0x2F };
            int index = 0;

            while (index < buffer.Length && subLevels.Count < amountOfOffsets)
            {
                index = Array.IndexOf(buffer, target[0], index);
                if (index == -1 || index >= buffer.Length - 7) break;

                if (buffer[index + 1] == target[1] && buffer[index + 2] == target[2]
                    && buffer[index + 3] == target[3] && buffer[index + 4] == target[4]
                    && buffer[index + 5] == target[5] && buffer[index + 6] == target[6])
                {
                    decompressedBody.Position = index - 4;
                    long offset = decompressedBody.Position;
                    subLevels.Add(new SubLevel(offset, Readers.ReadString(decompressedBody)));
                }
                index++;
            }

            return subLevels;
        }
        public static SubLevel FindPersistentLevel(MemoryStream decompressedBody)
        {
            byte[] offsetBytes = new byte[4];
            decompressedBody.Seek(4, SeekOrigin.Begin);
            decompressedBody.Read(offsetBytes, 0, 4);
            byte[] buffer = decompressedBody.ToArray();
            byte[] target = new byte[]
            {
                0x01, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x2F, 0x53, 0x63, 0x72, 0x69, 0x70, 0x74, 0x2F,
                0x46, 0x61, 0x63, 0x74, 0x6F, 0x72, 0x79, 0x47, 0x61, 0x6D, 0x65, 0x2E, 0x46, 0x47, 0x57, 0x6F,
                0x72, 0x6C, 0x64, 0x53, 0x65, 0x74, 0x74, 0x69, 0x6E, 0x67, 0x73, 0x00, 0x11, 0x00, 0x00, 0x00,
                0x50, 0x65, 0x72, 0x73, 0x69, 0x73, 0x74, 0x65, 0x6E, 0x74, 0x5F, 0x4C, 0x65, 0x76, 0x65, 0x6C,
                0x00, 0x31, 0x00, 0x00, 0x00, 0x50, 0x65, 0x72, 0x73, 0x69, 0x73, 0x74, 0x65, 0x6E, 0x74, 0x5F,
                0x4C, 0x65, 0x76, 0x65, 0x6C, 0x3A, 0x50, 0x65, 0x72, 0x73, 0x69, 0x73, 0x74, 0x65, 0x6E, 0x74,
                0x4C, 0x65, 0x76, 0x65, 0x6C, 0x2E, 0x46, 0x47, 0x57, 0x6F, 0x72, 0x6C, 0x64, 0x53, 0x65, 0x74,
                0x74, 0x69, 0x6E, 0x67, 0x73
            };
            int index = 0;
            while (true)
            {
                index = Array.IndexOf(buffer, target[0], index);
                if (index == -1 || index >= buffer.Length - 7)
                {
                    break;
                }
                if (buffer[index + 1] == target[1] && buffer[index + 2] == target[2]
                && buffer[index + 3] == target[3] && buffer[index + 4] == target[4]
                && buffer[index + 5] == target[5] && buffer[index + 6] == target[6]
                && buffer[index + 7] == target[7] && buffer[index + 8] == target[8]
                && buffer[index + 9] == target[9] && buffer[index + 10] == target[10]
                && buffer[index + 11] == target[11] && buffer[index + 12] == target[12]
                && buffer[index + 13] == target[13] && buffer[index + 14] == target[14]
                && buffer[index + 15] == target[15] && buffer[index + 16] == target[16]
                && buffer[index + 17] == target[17] && buffer[index + 18] == target[18]
                && buffer[index + 19] == target[19] && buffer[index + 20] == target[20]
                && buffer[index + 21] == target[21] && buffer[index + 22] == target[22]
                && buffer[index + 23] == target[23] && buffer[index + 24] == target[24]
                && buffer[index + 25] == target[25] && buffer[index + 26] == target[26]
                && buffer[index + 27] == target[27] && buffer[index + 28] == target[28]
                && buffer[index + 29] == target[29] && buffer[index + 30] == target[30]
                && buffer[index + 31] == target[31] && buffer[index + 32] == target[32]
                && buffer[index + 33] == target[33] && buffer[index + 34] == target[34]
                && buffer[index + 35] == target[35] && buffer[index + 36] == target[36]
                && buffer[index + 37] == target[37] && buffer[index + 38] == target[38]
                && buffer[index + 39] == target[39] && buffer[index + 40] == target[40]
                && buffer[index + 41] == target[41] && buffer[index + 42] == target[42]
                && buffer[index + 43] == target[43] && buffer[index + 44] == target[44]
                && buffer[index + 45] == target[45] && buffer[index + 46] == target[46]
                && buffer[index + 47] == target[47] && buffer[index + 48] == target[48]
                && buffer[index + 49] == target[49] && buffer[index + 50] == target[50]
                && buffer[index + 51] == target[51] && buffer[index + 52] == target[52]
                && buffer[index + 53] == target[53] && buffer[index + 54] == target[54]
                && buffer[index + 55] == target[55] && buffer[index + 56] == target[56]
                && buffer[index + 57] == target[57] && buffer[index + 58] == target[58]
                && buffer[index + 59] == target[59] && buffer[index + 60] == target[60]
                && buffer[index + 61] == target[61]
                && buffer[index + 62] == target[62] && buffer[index + 63] == target[63]
                && buffer[index + 64] == target[64] && buffer[index + 65] == target[65]
                && buffer[index + 66] == target[66] && buffer[index + 67] == target[67]
                && buffer[index + 68] == target[68] && buffer[index + 69] == target[69]
                && buffer[index + 70] == target[70] && buffer[index + 71] == target[71]
                && buffer[index + 72] == target[72] && buffer[index + 73] == target[73]
                && buffer[index + 74] == target[74] && buffer[index + 75] == target[75]
                && buffer[index + 76] == target[76] && buffer[index + 77] == target[77]
                && buffer[index + 78] == target[78] && buffer[index + 79] == target[79]
                && buffer[index + 80] == target[80] && buffer[index + 81] == target[81]
                && buffer[index + 82] == target[82] && buffer[index + 83] == target[83]
                && buffer[index + 84] == target[84] && buffer[index + 85] == target[85]
                && buffer[index + 86] == target[86] && buffer[index + 87] == target[87]
                && buffer[index + 88] == target[88] && buffer[index + 89] == target[89]
                && buffer[index + 90] == target[90] && buffer[index + 91] == target[91]
                && buffer[index + 92] == target[92] && buffer[index + 93] == target[93]
                && buffer[index + 94] == target[94] && buffer[index + 95] == target[95]
                && buffer[index + 96] == target[96] && buffer[index + 97] == target[97]
                && buffer[index + 98] == target[98] && buffer[index + 99] == target[99]
                && buffer[index + 100] == target[100] && buffer[index + 101] == target[101]
                && buffer[index + 102] == target[102] && buffer[index + 103] == target[103]
                && buffer[index + 104] == target[104] && buffer[index + 105] == target[105]
                && buffer[index + 106] == target[106] && buffer[index + 107] == target[107]
                && buffer[index + 108] == target[108] && buffer[index + 109] == target[109]
                && buffer[index + 110] == target[110] && buffer[index + 111] == target[111]
                && buffer[index + 112] == target[112] && buffer[index + 113] == target[113]
                && buffer[index + 114] == target[114] && buffer[index + 115] == target[115]
                && buffer[index + 116] == target[116])
                {
                    decompressedBody.Position = index - 8;
                    long offset = decompressedBody.Position;
                    return new SubLevel(offset, "Persistent Level");
                }
                index++;
            }
            throw new Exception("couldnt find persistent level");
        }
    }
}
