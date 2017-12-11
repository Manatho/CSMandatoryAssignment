using System;
using System.ComponentModel.DataAnnotations;
using AssignmentLogic.SubmissionStorage;
using AssignmentLogic.SerialcodeStorage;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AssignmentLogic
{
    public class GiveAwayController : IGiveAway
    {
        IStoreSubmissions submissionStorage;
        IStoreSerialcodes codeStorage;

        public GiveAwayController(string fileLocation)
        {

            submissionStorage = new FileSubmissionStorage(fileLocation);

            int[] codes = new int[100];
            for (int i = 0; i < codes.Length; i++)
                codes[i] = i;

            codeStorage = new FileSerialcodeStorage(fileLocation, codes ,false);
        }


        public Submission DrawWinner()
        {
            ICollection<Submission> submissions = submissionStorage.FindAll();
            int winnerNumber = new Random().Next(submissions.Count);
            return submissions.ElementAt(winnerNumber);
        }

        public SubmitStates Submit(Submission submission)
        {
            SubmitStates temp;
            //Validates submission
            temp = ValidateSubmission(submission);
            if (temp != SubmitStates.Success)
                return temp;

            //Check for dublicates in storage
            temp = submissionStorage.Store(submission);
            if (temp != SubmitStates.Success)
                return temp;

            return temp;
        }

        public ICollection<Submission> AllSubmissions()
        {
            return submissionStorage.FindAll();
        }

        private SubmitStates ValidateSubmission(Submission submission)
        {
            if (!codeStorage.CheckCode(submission.SerialNumber))
            {
                return SubmitStates.InvalidCode;
            }

            EmailAddressAttribute emailValidator = new EmailAddressAttribute();
            PhoneAttribute phoneValidator = new PhoneAttribute();


            if (submission.FirstName == "" || submission.SurName == "")
                return SubmitStates.InvalidInformation;

            if (!emailValidator.IsValid(submission.EmailAdress))
                return SubmitStates.InvalidInformation;

            if (!phoneValidator.IsValid(submission.PhoneNumber))
                return SubmitStates.InvalidInformation;

            if( submission.Birthday.Equals(default(DateTime)))
                return SubmitStates.InvalidInformation;

            return SubmitStates.Success;
        }


    }
}
