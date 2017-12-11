using System;
using System.ComponentModel.DataAnnotations;

namespace AssignmentLogic
{
    [Serializable]
    public class Submission
    {
        //DataAnnotations have been used due to difficulties with
        //other approaches, however given more time it would be
        //appropriate to avoid GUI specific details in the domain object

        [Required]
        [Display(Name= "First Name")]
        public string FirstName { get;  set; }

        [Required]
        [Display(Name = "Sur Name")]
        public string SurName { get;  set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAdress { get;  set; }

        [Required]
        [Display(Name = "Phone Number"), DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get;  set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Birthday { get;  set; }

        [Display(Name = "Serial Number")]
        public int SerialNumber { get;  set; }

        public Submission()
        {

        }

        public Submission(string firstName, string surName, string emailAdress, string phoneNumber, DateTime birthday, int serialNumber)
        {
            FirstName = firstName;
            SurName = surName;
            EmailAdress = emailAdress;
            PhoneNumber = phoneNumber;
            Birthday = birthday;
            SerialNumber = serialNumber;
        }

        public override string ToString()
        {
            return SerialNumber + " | " + FirstName + " " + SurName + " | " + EmailAdress + " | " + PhoneNumber + " | " + Birthday;
        }

        //Basic Hashcode and Equals methods
        public override int GetHashCode()
        {
            int hash = 5;
            hash = 83 * hash + FirstName.GetHashCode();
            hash = 83 * hash + SurName.GetHashCode();
            hash = 83 * hash + EmailAdress.GetHashCode();
            hash = 83 * hash + PhoneNumber.GetHashCode();
            hash = 83 * hash + Birthday.GetHashCode();
            hash = 83 * hash + SerialNumber;
            return hash;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Submission s = obj as Submission;

            return s.FirstName == FirstName &&
                s.SurName == SurName &&
                s.EmailAdress == EmailAdress &&
                s.PhoneNumber == PhoneNumber &&
                s.Birthday == Birthday &&
                s.SerialNumber == SerialNumber;
            
               
            
        }
    }
}
