using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLScriptParser.Parsers;

namespace WLScriptParser.Models.Repositories
{
    public sealed class ScriptRepository
    {
        private static ScriptRepository _repository = null;
        public Script ScriptLeft { get; set; }
        public Script ScriptRight { get; set; }

        public static ScriptRepository Repository
        {
            get
            {
                if(_repository == null)
                {
                    throw new Exception("Repository not yet created");
                }
                return _repository;
            }
        }

        private ScriptRepository(string filePathLeft, string filePathRight)
        {
            ScriptLeft = ScriptTransactionParser.Parse(filePathLeft);
            ScriptRight = ScriptTransactionParser.Parse(filePathRight);
        }

        public static void Create(string filePathLeft, string filePathRight)
        {
            _repository = new ScriptRepository(filePathLeft, filePathRight);
        }

    }
}
