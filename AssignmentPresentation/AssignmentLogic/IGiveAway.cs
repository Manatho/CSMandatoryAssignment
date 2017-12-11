using System;
using System.Collections.Generic;
using System.Text;

namespace AssignmentLogic
{
    public enum SubmitStates
    {
        Success,
        Duplicate,
        InvalidCode,
        InvalidInformation
    }

    public interface IGiveAway
    {
        SubmitStates Submit(Submission submission);
        Submission DrawWinner();

        ICollection<Submission> AllSubmissions();
    }
}
