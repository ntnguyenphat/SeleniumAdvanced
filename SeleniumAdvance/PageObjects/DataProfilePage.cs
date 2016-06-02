﻿using OpenQA.Selenium;
using SeleniumAdvance.Common;
using SeleniumAdvance.DataObjects;
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
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

namespace SeleniumAdvance.PageObjects
{
    public class DataProfilePage : GeneralPage
    {
        private IWebDriver _driverDataProfile;

        #region Locators

        static readonly By _lnkAddNew = By.XPath("//a[.='Add New']");
        static readonly By _txtName = By.XPath("//input[@id='txtProfileName']");
        static readonly By _btnNext = By.XPath("//input[@value='Next']");
        static readonly By _btnFinish = By.XPath("//input[@value='Finish']");
        static readonly By _btnCancel = By.XPath("//input[@value='Cancel']");

        #endregion

        #region Elements

        public IWebElement LnkAddNew
        {
            get { return MyFindElement(_lnkAddNew); }
        }
        public IWebElement TxtName
        {
            get { return MyFindElement(_txtName); }
        }
        public IWebElement BtnNext
        {
            get { return MyFindElement(_btnNext); }
        }
        public IWebElement BtnFinish
        {
            get { return MyFindElement(_btnFinish); }
        }
        public IWebElement BtnCancel
        {
            get { return MyFindElement(_btnCancel); }
        }

        #endregion

        #region Methods

        public DataProfilePage(IWebDriver driver):base(driver)
        {
            this._driverDataProfile = driver;
        }


        /// <summary>
        /// Wait for adding profile.
        /// </summary>
        /// <param name="profileName">Name of the profile.</param>
        /// <Author>Phat</Author>
        /// <Startdate>23/05/2016</Startdate>
        public void WaitForAddingProfile(string profileName)
        {
            By panel = By.XPath("//a[.='" + profileName + "']");
            WebDriverWait wait = new WebDriverWait(_driverDataProfile, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementExists(panel));
            wait.Until(ExpectedConditions.ElementToBeClickable(_lnkAddNew));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[@href='#Administer']")));
        }


        /// <summary>
        /// Click Edit Profile link
        /// </summary>
        /// <param name="profileName">Name of the profile.</param>
        /// <Author>Phat</Author>
        /// <Startdate>23/05/2016</Startdate>
        /// <returns></returns>
        public DataProfilePage ClickEditProfile(string profileName)
        {
            By xpath = By.XPath("//a[.='" + profileName + "']/ancestor::tr//a[.='Edit']");
            MyFindElement(xpath).Click();
            WebDriverWait wait = new WebDriverWait(_driverDataProfile, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementExists(_txtName));
            return this;
        }


        /// <summary>
        /// Click Delete Profile link
        /// </summary>
        /// <param name="profileName">Name of the profile.</param>
        /// <Author>Phat</Author>
        /// <Startdate>23/05/2016</Startdate>
        /// <returns></returns>
        public void ClickDeleteProfile(string profileName)
        {
            By xpath = By.XPath("//a[.='" + profileName + "']/ancestor::tr//a[.='Delete']");
            MyFindElement(xpath).Click();
            WebDriverWait wait = new WebDriverWait(_driverDataProfile, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.AlertIsPresent());
        }


        /// <summary>
        /// Deletes a profile.
        /// </summary>
        /// <param name="profileName">Name of the profile.</param>
        /// <Author>Phat</Author>
        /// <Startdate>23/05/2016</Startdate>
        /// <returns></returns>
        public DataProfilePage DeleteProfile(string profileName)
        {
            By xpath = By.XPath("//a[.='" + profileName + "']/ancestor::tr//a[.='Delete']");
            if (this.IsElementExist(xpath))
            {
                this.SelectMenuItem("Administer", "Data Profiles");
            }
            ClickDeleteProfile(profileName);
            IAlert alert = _driverDataProfile.SwitchTo().Alert();
            alert.Accept();
            return this;
        }


        /// <summary>
        /// Determine if a data profile exist.
        /// </summary>
        /// <param name="dataProfiles"></param>
        /// <Author>Long</Author>
        /// <Startdate>02/06/2016</Startdate>
        /// <returns></returns>
        public bool DoesPresetDataProfileExist(DataProfiles dataProfiles)
        {
            bool doesPresetDataProfileExist = false;
            ReadOnlyCollection<IWebElement> RowCollection = _driverDataProfile.FindElements(By.XPath("//table[@class = 'GridView']/tbody/tr"));
            for (int i_RowNum = 2; i_RowNum <= RowCollection.Count; i_RowNum++)
            {
                ReadOnlyCollection<IWebElement> ColCollection = _driverDataProfile.FindElements(By.XPath(string.Format("//table[@class = 'GridView']/tbody/tr[{0}]/td", i_RowNum)));
                for (int i_ColNum = 1; i_ColNum <= ColCollection.Count; i_ColNum++ )
                {
                    IWebElement ColElement = MyFindElement(By.XPath(string.Format("//table[@class = 'GridView']/tbody/tr[{0}]/td[{1}]", i_RowNum, i_ColNum))); 
                    if (dataProfiles.DataProfileName == ColElement.Text)
                    {
                        doesPresetDataProfileExist = true;
                        if (dataProfiles.ItemType == null)
                            break;
                        else
                        { 
                            ColElement = MyFindElement(By.XPath(string.Format("//table[@class = 'GridView']/tbody/tr[{0}]/td[{1}]", i_RowNum, i_ColNum + 1)));
                            if (dataProfiles.ItemType == ColElement.Text)
                            {
                                doesPresetDataProfileExist = true;
                                if (dataProfiles.RelatedData == null)
                                    break;
                                else
                                {
                                    ColElement = MyFindElement(By.XPath(string.Format("//table[@class = 'GridView']/tbody/tr[{0}]/td[{1}]", i_RowNum, i_ColNum + 2)));
                                    if (dataProfiles.RelatedData == ColElement.Text)
                                    {
                                        doesPresetDataProfileExist = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                if (doesPresetDataProfileExist == true)
                    break;
            }
            return doesPresetDataProfileExist;
        }

        /// <summary>
        /// Gets the table cell value in Data Profile table
        /// </summary>
        /// <param name="row_number">The row_number.</param>
        /// <param name="column_number">The column_number.</param>
        /// <returns></returns>
        /// <Author>Long</Author>
        /// <Startdate>02/06/2016</Startdate>
        public string GetTableCellValue(int row_number, int column_number)
        {
            string cellValue = "";
            ReadOnlyCollection<IWebElement> RowCollection = _driverDataProfile.FindElements(By.XPath("//table[@class = 'GridView']/tbody/tr"));
            ReadOnlyCollection<IWebElement> ColCollection = _driverDataProfile.FindElements(By.XPath(string.Format("//table[@class = 'GridView']/tbody/tr[{0}]/td", row_number)));
            IWebElement ColElement = MyFindElement(By.XPath(string.Format("//table[@class = 'GridView']/tbody/tr[{0}]/td[{1}]", row_number, column_number)));
            cellValue = ColElement.Text;
            return cellValue;                                      
        }

        /// <summary>
        /// Gets the index of table cell value in Data Profile table
        /// </summary>
        /// <param name="cellValue">The value of one cell.</param>
        /// <param name="row_number">The row_number.</param>
        /// <param name="column_number">The column_number.</param>
        /// <Author>Long</Author>
        /// <Startdate>02/06/2016</Startdate>
        public void GetIndexOfTableCellValue(string cellValue, out int row_number, out int column_number)
        {
            row_number = column_number = -1;
            ReadOnlyCollection<IWebElement> RowCollection = _driverDataProfile.FindElements(By.XPath("//table[@class = 'GridView']/tbody/tr"));
            for (int i_RowNum = 2; i_RowNum <= RowCollection.Count; i_RowNum++)
            {
                ReadOnlyCollection<IWebElement> ColCollection = _driverDataProfile.FindElements(By.XPath(string.Format("//table[@class = 'GridView']/tbody/tr[{0}]/td", i_RowNum)));
                for (int i_ColNum = 1; i_ColNum <= ColCollection.Count; i_ColNum++)
                {
                    IWebElement ColElement = MyFindElement(By.XPath(string.Format("//table[@class = 'GridView']/tbody/tr[{0}]/td[{1}]", i_RowNum, i_ColNum)));
                    if (cellValue == ColElement.Text)
                    {
                        row_number = i_RowNum;
                        column_number = i_ColNum;
                        break;
                    }
                }
                if (row_number != -1 && column_number != -1)
                    break;
            }
        }

        /// <summary>
        /// Determines if data profile is a link.
        /// </summary>
        /// <param name="dataProfile">The data profile.</param>
        /// <returns></returns>
        /// <Author>Long</Author>
        /// <Startdate>02/06/2016</Startdate>
        public bool IsDataProfileLink(string dataProfile)
        {
            bool isDataProfileLink;
            int row_number, column_number;
            GetIndexOfTableCellValue(dataProfile, out row_number, out column_number);
            IWebElement ProfileName = MyFindElement(By.XPath(string.Format("//table[@class = 'GridView']/tbody/tr[{0}]/td[{1}]", row_number, column_number)));
            isDataProfileLink = ProfileName.IsLink(_driverDataProfile);
            return isDataProfileLink;
        }

        public bool DoesCheckboxAppearInTheLeftOfDataProfile(string dataProfile)
        {
            int row_number, column_number;
            GetIndexOfTableCellValue(dataProfile, out row_number, out column_number);
            IWebElement TheLeftOfProfileName = MyFindElement(By.XPath(string.Format("//table[@class = 'GridView']/tbody/tr[{0}]/td[{1}]", row_number, column_number - 1)));
            if (TheLeftOfProfileName.GetAttribute("class") == "box")
                return true;
            else
                return false;
        }
        #endregion
    }
}
