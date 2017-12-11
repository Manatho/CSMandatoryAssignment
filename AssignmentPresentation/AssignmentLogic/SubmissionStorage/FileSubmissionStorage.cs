using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace AssignmentLogic.SubmissionStorage
{
    public class FileSubmissionStorage : IStoreSubmissions
    {
        public string FileLocation { get; private set; }
        public const string FileName = "Submissions.dat";

        private HashSet<int> existingSerialNumbers;

        public FileSubmissionStorage(string location)
        {
            FileLocation = location+"\\"+FileName;
            existingSerialNumbers = new HashSet<int>();

            if (!File.Exists(FileLocation))
                using (File.Create(FileLocation)) { };

            foreach (Submission s in FindAll())
                existingSerialNumbers.Add(s.SerialNumber);
        }


        public SubmitStates Store(params Submission[] submissions)
        {
            if (submissions.Distinct().Count() != submissions.Length)
                return SubmitStates.Duplicate;

            foreach (Submission s in submissions)
                if (SerialAlreadyExists(s))
                    return SubmitStates.Duplicate;

            using (FileStream stream = new FileStream(FileLocation, FileMode.Append))
            {
                IFormatter formatter = new BinaryFormatter();
                foreach (Submission s in submissions)
                {
                    formatter.Serialize(stream, s);
                    existingSerialNumbers.Add(s.SerialNumber);
                }
            }
            
            return SubmitStates.Success;
        }


        public ICollection<Submission> FindAll()
        {
            return AllSubmissionFromFile();
        }

        public Submission Find(int SerialNumber)
        {
            List<Submission> submissions = AllSubmissionFromFile();

            foreach (Submission s in submissions)
                if (s.SerialNumber == SerialNumber)
                    return s;
            
            return null;
        }


        public bool RemoveAll()
        {
            using (File.Create(FileLocation)) { };
            existingSerialNumbers.Clear();
            return true;
        }

        public bool Remove(int serialNumber)
        {
            List<Submission> submissions = AllSubmissionFromFile();
            if (submissions.RemoveAll(s => s.SerialNumber == serialNumber) == 1)
            {
                RemoveAll();
                using (File.Create(FileLocation)) { };
                Store(submissions.ToArray());
                return true;
            }
            return false;

            
        }

        private List<Submission> AllSubmissionFromFile()
        {
            List<Submission> submissions = new List<Submission>();

            using (var stream = new FileStream(FileLocation, FileMode.Open))
            {
                IFormatter bFormatter = new BinaryFormatter();
                while (stream.Position != stream.Length)
                    submissions.Add((Submission)bFormatter.Deserialize(stream));
                
            }

            return submissions;
        }

        private bool SerialAlreadyExists(Submission submission)
        {
            return existingSerialNumbers.Contains(submission.SerialNumber);
        }





    }
}
