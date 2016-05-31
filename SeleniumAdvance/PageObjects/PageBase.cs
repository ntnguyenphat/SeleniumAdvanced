﻿using OpenQA.Selenium;
using SeleniumAdvance.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeleniumAdvance.Ultilities;
using OpenQA.Selenium.Support.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace SeleniumAdvance.PageObjects
{
    public class PageBase
    {
        protected IWebDriver _driver;

        public PageBase(IWebDriver driver)
        {
            this._driver = driver;
        }

        public IWebElement MyFindElement(By by, long timeout = 30)
        {
            IWebElement Ele = null;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (timeout >= 0)
            {
                try
                {
                    WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeout));
                    //wait.Until(ExpectedConditions.ElementIsVisible(by));
                    wait.Until(driver => _driver.FindElement(by).Displayed);
                    Ele = _driver.FindElement(by);
                    break;
                }
                catch(StaleElementReferenceException)
                {
                    timeout = timeout - stopwatch.Elapsed.Ticks;
                    MyFindElement(by, timeout);
                }
                catch(NullReferenceException)
                {
                    timeout = timeout - stopwatch.Elapsed.Ticks;
                    MyFindElement(by, timeout);
                }
                catch(WebDriverTimeoutException)
                {
                    timeout = timeout - stopwatch.Elapsed.Ticks;
                    MyFindElement(by, timeout);
                }
            }
            stopwatch.Stop();
            return Ele;
        }
    }

}
