﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeleniumAdvance.PageObjects;
using SeleniumAdvance.Common;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using SeleniumAdvance.Ultilities;

namespace SeleniumAdvance.TestCases
{
    [TestClass]
    public class MPTestcases : TestBase
    {
        [TestMethod]
        public void TC011()
        {
            Console.WriteLine("DA_MP_TC011 - Verify that user is unable open more than 1 \"New Page\" dialog");

            //1. Navigate to Dashboard login page. Login with valid account
            //2. Go to Global Setting -> Add page. Try to go to Global Setting -> Add page again

            LoginPage loginPage = new LoginPage(driver);
            loginPage.Open().Login(Constant.Username, Constant.Password);

            MainPage mainPage = new MainPage(driver);
            mainPage.SelectGeneralSetting("Add Page");

            bool actualResult = mainPage.IsDashboardLockedByDialog();

            //VP: User cannot go to Global Setting -> Add page while "New Page" dialog appears.

            Assert.AreEqual(true, actualResult, "Dashboard is not locked by dialog!");
        }

        [TestMethod]
        public void TC012()
        {
            Console.WriteLine("DA_MP_TC012 - Verify that user is able to add additional pages besides \"Overview\" page successfully");

            string pageName = CommonMethods.GetUniqueString();

            //1. Navigate to Dashboard login page. Login with valid account
            //2. Go to Global Setting -> Add page. Enter Page Name field

            LoginPage loginPage = new LoginPage(driver);
            loginPage.Open().Login(Constant.Username, Constant.Password);

            MainPage mainPage = new MainPage(driver);

            mainPage.AddPage(pageName);

            //VP: New page is displayed besides "Overview" page
            bool isPageNextToPage = mainPage.IsPageNextToPage("Overview", pageName);
            Assert.AreEqual(true, isPageNextToPage, "The new page isn't displayed besides \"Overview\" page");

            //Post-condition: Delete newly added page
            mainPage.DeletePage(pageName);
        }

        [TestMethod]
        public void TC013()
        {
            Console.WriteLine("DA_MP_TC013 - Verify that the newly added main parent page is positioned at the location specified as set with \"Displayed After\" field of \"New Page\" form on the main page bar/\"Parent Page\" dropped down menu");

            string pageName1 = string.Concat("Page1", CommonMethods.GetUniqueString());
            string pageName2 = string.Concat("Page2", CommonMethods.GetUniqueString());

            //1. Navigate to Dashboard login page. Login with valid account
            //2. Go to Global Setting -> Add page. Enter Page Name field

            LoginPage loginPage = new LoginPage(driver);
            loginPage.Open().Login(Constant.Username, Constant.Password);

            MainPage mainPage = new MainPage(driver);

            mainPage.AddPage(pageName1);
            mainPage.AddPage(pageName: pageName2, displayAfer: pageName1);

            //VP: Page 1 is positioned besides the Page 2
            bool isPageNextToPage = mainPage.IsPageNextToPage(pageName1, pageName2);
            Assert.AreEqual(true, isPageNextToPage, "" + pageName2 + "isn't positioned besides" + pageName2);

            //Post-condition: Delete newly added page
            mainPage.DeletePage(pageName2);
            mainPage.DeletePage(pageName1);
        }

        [TestMethod]
        public void TC014()
        {
            Console.WriteLine("DA_MP_TC014 - Verify that \"Public\" pages can be visible and accessed by all users of working repository");

            string pageName = string.Concat("Page1", CommonMethods.GetUniqueString());

            //1.Navigate to Dashboard login page
            //2.Log in specific repository with valid account

            LoginPage loginPage = new LoginPage(driver);
            loginPage.Open().Login(Constant.Username, Constant.Password);

            //3.Go to Global Setting -> Add page
            //4.Enter Page Name field
            //5.Check Public checkbox
            //6.Click OK button

            MainPage mainPage = new MainPage(driver);
            mainPage.AddPage(pageName: pageName, publicCheckBox: true);

            //7.Click on Log out link
            //8.Log in with another valid account

            loginPage = mainPage.Logout();
            loginPage.Login(Constant.OtherUsername, Constant.OtherPassword);

            //VP: Check newly added page is visibled
            bool doesPageExist = mainPage.DoesPageExist(pageName);
            Assert.AreEqual(true, doesPageExist, "" + pageName + "isn't visibled");

            //Post-condition: Delete newly added page
            loginPage = mainPage.Logout();
            loginPage.Login(Constant.Username, Constant.Password);
            mainPage.DeletePage(pageName);
        }

        [TestMethod]
        public void TC015()
        {
            Console.WriteLine("DA_MP_TC015 - Verify that non \"Public\" pages can only be accessed and visible to their creators with condition that all parent pages above it are \"Public\"");

            string parentPageName = string.Concat("Parent", CommonMethods.GetUniqueString());
            string childPageName = string.Concat("Child", CommonMethods.GetUniqueString());

            //1. Navigate to Dashboard login page. Log in specific repository with valid account
            //2. Go to Global Setting -> Add page. Enter Page Name field. Check Public checkbox. Click OK button

            LoginPage loginPage = new LoginPage(driver);
            loginPage.Open().Login(Constant.Username, Constant.Password);

            MainPage mainPage = new MainPage(driver);
            mainPage.AddPage(pageName: parentPageName, publicCheckBox: true);

            //3. Go to Global Setting -> Add page. Enter Page Name field. Click on  Select Parent dropdown list
            //4. Select specific page. Click OK button. Click on Log out link. Log in with another valid account

            mainPage.AddPage(pageName: childPageName, parentPage: parentPageName);
            mainPage.Logout();

            loginPage.Login(Constant.OtherUsername, Constant.OtherPassword);

            //VP: Children is invisibled
            bool doesPageExist = mainPage.DoesPageExist(parentPageName + "->" + childPageName);
            Assert.AreEqual(false, doesPageExist, "" + childPageName + "is visibled");

            //Post-condition: Delete newly added page
            loginPage = mainPage.Logout();
            loginPage.Login(Constant.Username, Constant.Password);
            mainPage.DeletePage(childPageName);
            mainPage.DeletePage(parentPageName);
        }

        [TestMethod]
        public void TC016()
        {
            Console.WriteLine("DA_MP_TC016 - Verify that user is able to edit the \"Public\" setting of any page successfully");

            string pageName1 = string.Concat("Page1", CommonMethods.GetUniqueString());
            string pageName2 = string.Concat("Page2", CommonMethods.GetUniqueString());

            //1. Navigate to Dashboard login page. Log in specific repository with valid account
            //2. Go to Global Setting -> Add page. Enter Page Name. Click OK button

            LoginPage loginPage = new LoginPage(driver);
            loginPage.Open().Login(Constant.Username, Constant.Password);

            MainPage mainPage = new MainPage(driver);
            mainPage.AddPage(pageName: pageName1);

            //3. Go to Global Setting -> Add page.  Enter Page Name. Check Public checkbox. Click OK button
            //4. Click on "Test" page. Click on "Edit" link.

            mainPage.AddPage(pageName: pageName2, publicCheckBox: true);
            mainPage.GotoPage(pageName1);
            mainPage.SelectGeneralSetting("Edit");

            //VP: "Edit Page" pop up window is displayed
            bool doesPopupExist1 = mainPage.DoesPopupExist("Edit Page");
            Assert.AreEqual(true, doesPopupExist1, "Pop up window is not displayed");

            //5. Check Public checkbox. Click OK button
            //6. Click on "Another Test" page. Click on "Edit" link.

            mainPage.EditPageInfomation(publicCheckBox: true);
            mainPage.GotoPage(pageName2);
            mainPage.SelectGeneralSetting("Edit");

            //VP: "Edit Page" pop up window is displayed

            bool doesPopupExist2 = mainPage.DoesPopupExist("Edit Page");
            Assert.AreEqual(true, doesPopupExist2, "Pop up window is not displayed");

            //7. Uncheck Public checkbox. Click OK button
            //8. Click Log out link. Log in with another valid account

            mainPage.EditPageInfomation(publicCheckBox: false);
            mainPage.Logout();

            loginPage.Login(Constant.OtherUsername, Constant.OtherPassword);

            //VP: Check "Test" Page is visible and can be accessed. Check "Another Test" page is invisible.
            bool doesPageName1Exist = mainPage.DoesPageExist(pageName1);
            Assert.AreEqual(true, doesPageName1Exist, "" + pageName1 + " isn't visibled");

            bool doesPageName2Exist = mainPage.DoesPageExist(pageName2);
            Assert.AreEqual(false, doesPageName2Exist, "" + pageName2 + " is visibled");
           
            //Post-condition: Delete newly added page
            mainPage.DeletePage(pageName2);
            mainPage.DeletePage(pageName1);
        }

        [TestMethod]
        public void TC017()
        {
            Console.WriteLine("DA_MP_TC017 - Verify that user can remove any main parent page except \"Overview\" page successfully and the order of pages stays persistent as long as there is not children page under it");

            string parentPageName = string.Concat("Parent", CommonMethods.GetUniqueString());
            string childPageName = string.Concat("Child", CommonMethods.GetUniqueString());

            //1. Navigate to Dashboard login page
            //2. Log in specific repository with valid account
            //3. Add a new parent page
            //4. Add a children page of newly added page
            LoginPage loginPage = new LoginPage(driver);
            loginPage.Open().Login(Constant.Username, Constant.Password);

            MainPage mainPage = new MainPage(driver);
            mainPage.AddPage(pageName: parentPageName);
            mainPage.AddPage(pageName: childPageName, parentPage: parentPageName);

            //5. Click on parent page
            //6. Click "Delete" link
            mainPage.GotoPage(parentPageName);
            mainPage.SelectGeneralSetting("Delete");

            //7. VP: Check confirm message "Are you sure you want to remove this page?" appears
            string expectedMessage1 = "Are you sure you want to remove this page?";
            string actualMessage1 = mainPage.GetAlertMessage();
            Assert.AreEqual(expectedMessage1, actualMessage1, "Confirm message doesn't appear.");

            //8. Click OK button
            mainPage.GetAlertMessage(closeAlert: true);

            //9. VP: Check warning message "Can not delete page 'Test' since it has child page(s)" appears
            string expectedMessage2 = "Cannot delete page '" + parentPageName +  "' since it has child page(s).";
            string actualMessage2 = mainPage.GetAlertMessage().Trim();
            Assert.AreEqual(expectedMessage2, actualMessage2, "Warning message doesn't appear.");

            //10. Click OK button
            mainPage.GetAlertMessage(closeAlert: true);

            //11. Click on  children page
            //12. Click "Delete" link
            mainPage.GotoPage(parentPageName + "->" + childPageName);
            mainPage.SelectGeneralSetting("Delete");

            //13. VP: Check confirm message "Are you sure you want to remove this page?" appears
            string expectedMessage3 = "Are you sure you want to remove this page?";
            string actualMessage3 = mainPage.GetAlertMessage();
            Assert.AreEqual(expectedMessage3, actualMessage3, "Confirm message doesn't appear.");

            //14. Click OK button
            mainPage.GetAlertMessage(closeAlert: true);

            //15. VP: Check children page is deleted
            bool doesChildPageExist = mainPage.DoesPageExist(parentPageName + "->" + childPageName);
            Assert.AreEqual(false, doesChildPageExist, "Child page isn't deleted");
            
            //16. Click on Parent page
            //17. Click "Delete" link
            mainPage.GotoPage(parentPageName);
            mainPage.SelectGeneralSetting("Delete");

            //18. VP: Check confirm message "Are you sure you want to remove this page?" appears
            string expectedMessage4 = "Are you sure you want to remove this page?";
            string actualMessage4 = mainPage.GetAlertMessage();
            Assert.AreEqual(expectedMessage4, actualMessage4, "Confirm message doesn't appear.");

            //19. Click OK button
            mainPage.GetAlertMessage(closeAlert: true);

            //20. VP: Check parent page is deleted
            bool doesParentPageExist = mainPage.DoesPageExist(parentPageName);
            Assert.AreEqual(false, doesParentPageExist, "Parent page isn't deleted");
        }
            
    }
}