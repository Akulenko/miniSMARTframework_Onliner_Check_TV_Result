﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using NUnit.Core;
using Rhino.Mocks;

namespace demo.framework.Elements
{
    public abstract class BaseElement : BaseEntity
    {
        private readonly RemoteWebElement _element;
        private readonly String _name;
        private readonly By _locator;

        protected BaseElement(By locator, String name)
        {
            this._name = name;
            this._locator = locator;
        }

        protected RemoteWebElement GetElement()
        {
            WaitForElementPresent();
            return (RemoteWebElement) Browser.GetDriver().FindElement(_locator);
            ;
        }

        protected String GetName()
        {
            return _name;
        }

        protected By GetLocator()
        {
            return _locator;
        }

        public void Select(string element)
        {
            WaitForElementPresent();
            SelectElement select = new SelectElement(GetElement());
            select.SelectByText(element);
            Log.Info(String.Format("{0} :: click", GetName()));
        }

        public void Check()
        {
            WebElementExist();
            GetElement().Click();
            Browser.WaitForPageToLoad();
            Log.Info(String.Format("{0} :: click", GetName()));
        }

        public void Click()
        {
            WaitForElementPresent();
            GetElement().Click();
            Browser.WaitForPageToLoad();
            Log.Info(String.Format("{0} :: click", GetName()));
        }

        public Boolean IsPresent()
        {
            bool isPresent = Browser.GetDriver().FindElements(_locator).Count > 0;
            Log.Info(GetName() + " : is present : " + isPresent);
            return isPresent;
        }
      
        protected void WaitForElementPresent()
        {
            var millisec = Configuration.GetTimeout();

            var wait = new WebDriverWait(Browser.GetDriver(),
                TimeSpan.FromMilliseconds(Convert.ToDouble(Configuration.GetTimeout())));
            try
            {
                wait.Until(waiting =>
                {
                    var webElements = Browser.GetDriver().FindElements(_locator);
                    return webElements.Count != 0;
                });
            }
            catch (TimeoutException)
            {
                Log.Fatal(string.Format("Element with locator: '{0}' does not exists!", _locator));
            }
        }

        public static void WaitForElementPresent(By locator, String name)

        {
            var millisec = Configuration.GetTimeout();

            var wait = new WebDriverWait(Browser.GetDriver(),
                TimeSpan.FromMilliseconds(Convert.ToDouble(Configuration.GetTimeout())));
            try
            {
                wait.Until(waiting =>
                {
                    try
                    {
                        var webElements = Browser.GetDriver().FindElements(locator);
                        return webElements.Count != 0;
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                });
            }
            catch (TimeoutException)
            {
                Log.Fatal(string.Format("Element with locator: '{0}' does not exists!", locator));
            }
        }

        public Boolean WebElementExist()
        {
            try
            {
                Browser.GetDriver().FindElements(_locator);
                return true;
            }
            catch (NoSuchElementException e)
            {
                Log.Fatal(string.Format("Element with locator: '{0}' does not exists!", _locator));
                return false;

            }
        }
    }

}

