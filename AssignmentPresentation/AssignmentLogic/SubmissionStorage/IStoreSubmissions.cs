using System;
using System.Collections.Generic;
using System.Text;

namespace AssignmentLogic.SubmissionStorage
{
    public interface IStoreSubmissions
    {
        SubmitStates Store(params Submission[] submissions);

        Submission Find(int serialNumber);
        ICollection<Submission> FindAll();

        bool RemoveAll();
        bool Remove(int serialNumber);
    }
}
