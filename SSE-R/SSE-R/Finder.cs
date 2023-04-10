using static SSE_R.misc;

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
    }
}
