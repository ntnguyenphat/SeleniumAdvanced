﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumAdvance.Common;

namespace SeleniumAdvance.Ultilities
{
    public static class IWebElementExtensions
    {
        public static void MouseTo(this IWebElement element, IWebDriver webDriver)
        {
            Actions actions = new Actions(webDriver);
            actions.MoveToElement(element).Build().Perform();
        }

        public static void SelectItem(this IWebElement element, string item, string selectby = "Text")
        {
            SelectElement selector = new SelectElement(element);
            if (selectby == "Value")
                selector.SelectByValue(item);
            else
                selector.SelectByText(item);
        }

        public static bool IsItemExist(this IWebElement element, string item)
        {
            SelectElement selector = new SelectElement(element);
            List<IWebElement> list = selector.Options.ToList();
            List<string> listItem = new List<string>();

            for (int i = 0; i < list.Count; i++)
            {
                listItem.Add(list[i].Text.ToString());
            }

            return listItem.Contains(item);
        }

        public static void Check(this IWebElement element)
        {
            bool isChecked = element.Selected;
            if (isChecked == false)
            {
                element.Click();
            }
        }
        public static void UnCheck(this IWebElement element)
        {
            bool isChecked = element.Selected;
            if (isChecked == true)
            {
                element.Click();
            }
        }
    }
}
