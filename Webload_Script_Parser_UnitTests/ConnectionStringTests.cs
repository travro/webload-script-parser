﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Webload_Script_Parser_WPF.Models;

namespace Webload_Script_Parser_UnitTests
{
    [TestClass]
    public class ConnectionStringTests
    {

        

        [TestMethod]
        public void AttributesRepository_TestNames_IsNotNull()
        {
            Assert.IsNotNull(AttributesRepository.Repository.TestNames);  
        }
    }
}
