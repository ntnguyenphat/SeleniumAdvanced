﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeleniumAdvance.PageObjects;
using SeleniumAdvance.Common;

namespace SeleniumAdvance.TestCases
{
    [TestClass]
    public class DA_LOGIN_TC001:PageBase
    {
        [TestMethod]
        public void TC01()
        {
            Console.WriteLine("TC01 - Verify that user can login specific repository successfully via Dashboard login page with correct credentials.");
            //1. Navigate to Dashboard login page
            HomePage homePage = new HomePage();
            homePage.Open();

            //2. Enter valid username and password
            //3. Click on "Login" button
            //VP: Verify that Dashboard Mainpage appears
            LoginPage loginPage = new LoginPage();
            loginPage.Login(Constant.Username, Constant.Password);
            Assert.AreEqual(true, LnkAccount.Displayed);

            //3. Enter valid Email and Password
            //4. Click on "Login" button
            //VP: User is logged into Railway. Welcome user message is displayed.
            //string actualMsg = loginPage.Login(Constant.Email, Constant.Password).GetWelcomeMessage();
            //string expectedMsg = "Welcome seltrain2015@gmail.com";
            //Assert.AreEqual(expectedMsg, actualMsg);
        }
    }
}
