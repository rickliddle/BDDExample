using System;
using TechTalk.SpecFlow;
using BDDExample.Lib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BDDExample.Specs.Steps
{
    [Binding]
    public class CalculatorSteps
    {
        private Calculator calculator = new Calculator();
        private int result;

        [Given(@"I have entered (.*) into the calculator")]
        public void GivenIHaveEnteredIntoTheCalculator(int p0)
        {
            calculator.Value = p0;
        }
        
        [When(@"I add (.*)")]
        public void WhenIAdd(int p0)
        {
            result = calculator.Add(p0);
        }
        
        [When(@"I subtract (.*)")]
        public void WhenISubtract(int p0)
        {
            result = calculator.Subtract(p0);
        }
        
        [Then(@"the result should be (.*)")]
        public void ThenTheResultShouldBe(int p0)
        {
            Assert.AreEqual(p0, result);
        }
    }
}
