using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace AssignmentLogic.SerialcodeStorage
{
    public class FileSerialcodeStorage : IStoreSerialcodes
    {
        public string FileLocation { get; private set; }
        public const string FileName = "Serialcodes.dat";

        HashSet<int> CurrentCodes = new HashSet<int>();


        public FileSerialcodeStorage(string location, int[] codes, bool overrideExistingFile)
        {
            FileLocation = location + "\\" + FileName;

            if(!File.Exists(FileLocation) || overrideExistingFile)
            {
                using (File.Create(FileLocation)) { };
            }
            SetCodes(codes);
        }

        public ICollection<int> AllCodes()
        {
            return CurrentCodes;
        }

        public bool CheckCode(int code)
        {
            return CurrentCodes.Contains(code);
        }

        private HashSet<int> AllCodesFromFile()
        {
            HashSet<int> codes = new HashSet<int>();

            using (var reader = new BinaryReader(File.Open(FileLocation, FileMode.Open)))
            {
                var pos = 0;
                var length = (int)reader.BaseStream.Length;
                while (pos < length)
                {
                    codes.Add(reader.ReadInt32());
                    pos += sizeof(int);
                }
            }

            return codes;
        }

        public void SetCodes(params int[] codes)
        {
            using (var writer = new BinaryWriter(File.Open(FileLocation, FileMode.Create)))
            {
                foreach (int c in codes)
                    writer.Write(c);
            }

            CurrentCodes = AllCodesFromFile();
        }
    }
}
