﻿using OpenQA.Selenium;
using SeleniumAdvance.Common;
using SeleniumAdvance.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeleniumAdvance.Ultilities;
using OpenQA.Selenium.Support.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace SeleniumAdvance.PageObjects
{
    public class ManagePagePage : GeneralPage
    {
        private IWebDriver _driverManagePagePage;

        #region Locators

        static readonly By _txtNewPagePageName = By.XPath("//div[@id='div_popup']//input[@class='page_txt_name']");
        static readonly By _btnNewPageOK = By.XPath("//div[@id='div_popup']//input[contains(@onclick,'doAddPage')]");
        static readonly By _cmbNewPageDisplayAfter = By.XPath("//div[@id='div_popup']//select[@id='afterpage']");
        static readonly By _cmbParentPage = By.XPath("//div[@id='div_popup']//select[@id='parent']");
        static readonly By _cbmNumberOfColumns = By.XPath("//div[@id='div_popup']//select[@id='columnnumber']");
        static readonly By _chbPublic = By.XPath("//input[@id='ispublic']");
        static readonly By _dlgPopupHeader = By.XPath("//div[@id='div_popup']//td[@class='ptc']/h2");
        static string _lnkNewPage = "//a[.='{0}']";

        #endregion

        #region Elements

        public IWebElement TxtNewPagePageName
        {
            get { return _driverManagePagePage.FindElement(_txtNewPagePageName); }
        }

        public IWebElement BtnNewPageOK
        {
            get { return _driverManagePagePage.FindElement(_btnNewPageOK); }
        }

        public IWebElement CmbNewPageDisplayAfter
        {
            get { return _driverManagePagePage.FindElement(_cmbNewPageDisplayAfter); }
        }

        public IWebElement CmbParentPage
        {
            get { return _driverManagePagePage.FindElement(_cmbParentPage); }
        }

        public IWebElement CmbNumberOfColumns
        {
            get { return _driverManagePagePage.FindElement(_cbmNumberOfColumns); }
        }

        public IWebElement ChbPublic
        {
            get { return _driverManagePagePage.FindElement(_dlgPopupHeader); }
        }

        public IWebElement DlgPopupHeader
        {
            get { return _driverManagePagePage.FindElement(_chbPublic); }
        }

        #endregion

        #region Methods

        public ManagePagePage(IWebDriver driver)
            : base(driver)
        {
            this._driverManagePagePage = driver;
        }

        //public void AddPage(string pageName)
        //{
        //    GeneralPage generalPage = new GeneralPage(_driverManagePagePage);
        //    generalPage.SelectGeneralSetting("Add Page");

        //    TxtNewPagePageName.SendKeys(pageName);
        //    BtnNewPageOK.Click();

        //    WebDriverWait wait = new WebDriverWait(_driverManagePagePage, TimeSpan.FromSeconds(10));
        //    wait.Until(ExpectedConditions.ElementExists(By.XPath(string.Format(_lnkNewPage, pageName))));
        //}

        //public void AddPage(string pageName, string displayAfter)
        //{
        //    this.SelectGeneralSetting("Add Page");

        //    TxtNewPagePageName.SendKeys(pageName);
        //    CmbNewPageDisplayAfter.SelectItem(displayAfter);
        //    BtnNewPageOK.Click();

        //    WebDriverWait wait = new WebDriverWait(_driverManagePagePage, TimeSpan.FromSeconds(10));
        //    wait.Until(ExpectedConditions.ElementExists(By.XPath(string.Format(_lnkNewPage, pageName))));
        //}

        public void AddPage(string pageName, string parentPage = null, int numberOfColumn = 0, string displayAfer = null, bool publicCheckBox = false)
        {
            this.SelectGeneralSetting("Add Page");
            TxtNewPagePageName.SendKeys(pageName);

            if (parentPage != null)
            {
                CmbParentPage.SelectItem(parentPage);
            }

            if (numberOfColumn != 0)
            {
                CmbNewPageDisplayAfter.SelectItem(numberOfColumn.ToString());
            }

            if (displayAfer != null)
            {
                CmbNewPageDisplayAfter.SelectItem(displayAfer);
            }

            if (publicCheckBox != false)
            {
                ChbPublic.Check();
            }

            BtnNewPageOK.Click();

            WebDriverWait wait = new WebDriverWait(_driverManagePagePage, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementExists(By.XPath(string.Format(_lnkNewPage, pageName))));
        }

        public void EditPageInfomation(string pageName = null, string parentPage = null, int numberOfColumn = 0, string displayAfer = null, bool publicCheckBox = false)
        {
            TxtNewPagePageName.SendKeys(pageName);

            if (parentPage != null)
            {
                CmbParentPage.SelectItem(parentPage);
            }

            if (numberOfColumn != 0)
            {
                CmbNewPageDisplayAfter.SelectItem(numberOfColumn.ToString());
            }

            if (displayAfer != null)
            {
                CmbNewPageDisplayAfter.SelectItem(displayAfer);
            }

            if (publicCheckBox != false)
            {
                ChbPublic.Check();
            }

            BtnNewPageOK.Click();

            WebDriverWait wait = new WebDriverWait(_driverManagePagePage, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementExists(By.XPath(string.Format(_lnkNewPage, pageName))));
        }

        public void SelectPage(string path)
        {
            By parent = By.XPath("//a[.='" + path + "']");
            IWebElement lnkParent = _driverManagePagePage.FindElement(parent);
            lnkParent.Click();
        }

        #endregion

        #region Verify

        public void CheckPageNextToPage(string currentPage, string nextPage)
        {
            By next = By.XPath("//a[.='" + currentPage + "']/following::a[1]");
            string nextValue = _driverManagePagePage.FindElement(next).Text;
            Assert.AreEqual(nextPage, nextValue, "\nExpected: " + nextPage + "\nActual: " + nextValue);
        }

        public void CheckPageNotExist(string parentPage, string childPage = null)
        {
            By page = By.XPath("//a[.='" + parentPage + "']");

            if (childPage != null)
            {
                IWebElement lnkParent = _driverManagePagePage.FindElement(page);
                lnkParent.MouseTo(_driverManagePagePage);
                page = By.XPath("//a[.='" + childPage + "']");

            }

            bool isExist = this.isElementExist(page);
            Assert.AreEqual(false, isExist, "\nPage is exist");
        }

        public void CheckPageVisible(string pageName)
        {
            By page = By.XPath("//a[.='" + pageName + "']");
            IWebElement lnkPage = _driverManagePagePage.FindElement(page);

            Assert.AreEqual(true, lnkPage.Displayed, "Page: " + pageName + " is invisible and can not be accessed");
        }

        public void CheckPopupHeader(string headerName)
        {
            Thread.Sleep(1000);

            Assert.AreEqual(headerName, DlgPopupHeader.Text, "\nExpected: " + headerName + "\nActual: " + DlgPopupHeader.Text);
        }

        #endregion
    }
}
