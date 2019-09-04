using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLScriptParser.Models;

namespace WLScriptParser.Utilities
{
    public static class ScriptTransactionsComparer
    {
        public static bool CompareCount(Script scriptLeft, Script scriptRight)
        {
            return scriptLeft.Transactions.Count == scriptRight.Transactions.Count;
        }

        public static bool CompareEach(Script scriptLeft, Script scriptRight)
        {
            Transaction[] scriptLeftTrans = scriptLeft.Transactions.ToArray();
            Transaction[] scriptRightTrans = scriptRight.Transactions.ToArray();

            if (scriptLeftTrans.Length != scriptRightTrans.Length) return false;

            for (int i = 0; i < scriptLeftTrans.Length; i++)
            {
                if(scriptLeftTrans[i].Name != scriptRightTrans[i].Name)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool CompareAll(Script scriptLeft, Script scriptRight)
        {
            return (CompareCount(scriptLeft, scriptRight) && CompareEach(scriptLeft, scriptRight))?  true: false;
        }
    }
}
