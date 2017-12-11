using System;
using System.Collections.Generic;
using System.Text;

namespace AssignmentLogic.SerialcodeStorage
{
    public interface IStoreSerialcodes
    {
        void SetCodes(params int[] codes);

        bool CheckCode(int code);

        ICollection<int> AllCodes();
    }
}
