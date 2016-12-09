# Behavior Driven Development with SpecFlow and Gherkin #

----------

### What is Behavior Driven Development?  ###

BDD is an Agile methodology inspired by the idea that a shared understanding of a system's intended behavior should drive its development. BDD aims to create a common understanding of the desired behavior through the use of a **ubiquitous language** that is easily understood by all participants in the process. The heart of BDD is expressed in only three words of that ubiquitous language: **Given**, **When**, and **Then**. This can be illustrated in a simple template:

> **Given** some initial context
> 
> **When** an event occurs
> 
> **Then** some criteria should be met

At its simplest, BDD takes the given/when/then template and pairs it with a title to create a **Scenario**. A scenario is the complete definition of a specification expressed in ubiquitous language. A complete scenario for the addition function of a simple calculator might look like this:

> **Scenario:** Simple addition
> 
> **Given** a number
> 
> **When** another number is added
> 
> **Then** the result should be the sum of the two numbers

Scenarios are grouped together with a title to form a **Feature**. A complete, well-written feature should define the functionality and acceptance criteria for a functional unit within an application. Extending the calculator example above, a feature for basic arithmetic might contain scenarios for addition, subtraction, multiplication and division of two numbers.

So far we've described a language for forming a common understanding, but that's only the start. BDD takes the ubiquitous language and uses it as a framework for test-driven development based on the common understanding. 

### Gherkin - bridging the gap from feature definition to development to testing ###

The example above is made up of keywords from the **Gherkin** language. By writing features in Gherkin, a team can drive development based on the behavior described by the feature. The software developers use tools to generate acceptance tests based on the feature defined in Gherkin. From that point, the process is similar to test-driven development: just enough code is written to meet the conditions in each step of each scenario. The scenarios become an executable test and when all the scenarios within a feature pass, the feature is complete.

Gherkin is deliberately brief. The entire language consists of only a handful of keywords:

- Feature (the unit of functionality we're describing)
- Background (preconditions for all scenarios in a feature)
- Scenario (one discrete chunk of functionality)
- Given, When, Then, And, But (steps in a scenario)
- Scenario Outline (a special scenario that accepts multiple inputs)
- Examples (the inputs used with Scenario Outline)

In addition Gherkin provides a few operators, mostly for meta functionality

- """ (Doc Strings)
- | (Data Tables)
- @ (Tags)
- # (Comments)

In the .Net world, there are several tools that will generate tests from Gherkin. This article will focus on SpecFlow, one of the most commonly used and well documented. When integrated into Visual Studio, SpecFlow will generate test classes as a framework for tests. The developer can populate those classes with code that exercises the code covered by the feature. The tests can be executed by many common test runner frameworks including MSTest, nUnit, xUnit, mbUnit and others. SpecFlow+ is a test runner that will execute tests within Visual Studio or via the command line and provide robust output beyond what other test runners offer.

### Enough reading. Let's build something! ###

Let's use the idea of the simple calculator mentioned above to demonstrate the process of behavior driven development. In a previous planning session, the stakeholders and developers came up with a simple arithmetic feature. The first part of it looks like this:


	Feature: Calculator
		In order to avoid silly mistakes
		As a math idiot
		I want to have simple math done for me

	Scenario: Add two numbers
		Given I have entered 50 into the calculator
		When I add 70
		Then the result should be 120

	Scenario: Subtract two numbers
		Given I have entered 70 into the calculator
		When I subtract 50
		Then the result should be 20

	...

Looks pretty simple, right? Because of the ubiquitous language that BDD is based on, it is very readable and easily understood. Let's turn it into something more...

Launch Visual Studio and create a new Unit Test Project. I've called mine `BDDExample.Specs` (.Specs is a conventional suffix for BDD test projects) in a solution called `BDDExample`.

Add the NuGet package `SpecRun.SpecFlow` to the project. The package will add some references and create a few files. 

Now let's add a couple of folders to organize our files. Add one folder named `Features` and another named `Steps`. Right-click on the new `Features` folder and select Add > New Item...

In the dialog box, choose `SpecFlow Feature File` from the list and name it `Calculator.feature`. In the new file you should see some text that is familiar. The default template for feature files is a simple calculator. We're going to tweak it some to work the way our team defined it. Copy the block above (excluding the ellipses at the end) and paste it in over the entire contents of the new feature file, then save it. 

Tke a moment to read over the feature file, then right-click somewhere in it and choose `Generate Step Definitions` from the pop-up menu. A new dialog window will appear that looks like this:

-- INSERT Images\generatestep.png HERE --

Make sure all the boxes are checked - there should be six of them - and then click the `Generate` button. When prompted, save the file as `CalculatorSteps.cs` in the `Steps` folder. Notice that the step text changes colors to indicate that corresponding methods have been generated. Open that new file, or jump directly to it by right-clicking in the feature file and choosing `Go to Step Definition`. The newly generated class will look like this:

	using System;
	using TechTalk.SpecFlow;
	
	namespace BDDExample.Specs.Steps
	{
	    [Binding]
	    public class CalculatorSteps
	    {
	        [Given(@"I have entered (.*) into the calculator")]
	        public void GivenIHaveEnteredIntoTheCalculator(int p0)
	        {
	            ScenarioContext.Current.Pending();
	        }
	        
	        [When(@"I add (.*)")]
	        public void WhenIAdd(int p0)
	        {
	            ScenarioContext.Current.Pending();
	        }
	        
	        [When(@"I subtract (.*)")]
	        public void WhenISubtract(int p0)
	        {
	            ScenarioContext.Current.Pending();
	        }
	        
	        [Then(@"the result should be (.*)")]
	        public void ThenTheResultShouldBe(int p0)
	        {
	            ScenarioContext.Current.Pending();
	        }
	    }
	}

A couple of things to notice here: First, because some the steps in the feature were the same, the methods aren't duplicated. Second, the class is decorated with the `Binding` attribute and the methods are decorated with `Given`, `When`, or `Then` attributes. This is the glue that SpecRun uses to match what's defined in a feature file with the methods you'll be implementing. Also, each method has a call to `ScenarioContext.Current.Pending()`. We'll talk more about this later, but for now it's safe to think of it as place-holder text. 

Now that we have a test class in place, we can build the solution and run our tests. If you open the `Test Explorer` pane, you should see two tests: `Add two numbers in Calculator` and `Subtract two numbers in Calculator`. Go ahead and click `Run All` to run the tests.

The tests don't actually run at this point - they're skipped because the calls to `Pending()` makes the runner skip the test. Let's write a little code to fix that. It's time to start implementing our calculator class. Go ahead and add a new `Class Library` project called `BDDExample.Lib` and reference it from the unit test project. Get rid of the default class that was created and add a class called `Calculator.cs` in its place. For now, just mark it `public` and leave it empty.

Back in `CalculatorSteps.cs` add a `using` statement to import the `BDDExample.Lib` namespace. Then add a private member variable to the class body:

	private Calculator calculator = new Calculator();

Delete the text inside the `GivenIHaveEnteredIntoTheCalculator` method body and replace it with code that sets the `Value` property of the calculator to the argument passed to the method:

	calculator.Value = p0;

"Red, green, refactor." Try to build and run the tests and the build will fail. No surprise - `Calculator` doesn't have a `Value` property. So add it:

	public int Value { get; set; }

Run the tests again. They'll still be skipped, but this time there will be more useful output. You can view the information by clicking the first test in `Test Explorer` and then clicking the `Output` link in the result pane. You'll see something like this:

	Test Name:	Add two numbers
	Test Outcome:	Skipped
	Result StandardOutput:	
	Given I have entered 50 into the calculator
	-> done: CalculatorSteps.GivenIHaveEnteredIntoTheCalculator(50) (0.0s)
	
	When I add 70
	-> pending: CalculatorSteps.WhenIAdd(70)
	
	Then the result should be 120
	-> skipped because of previous errors
	-> Pending: One or more step definitions are not implemented yet.
	->   CalculatorSteps.WhenIAdd(70)

We can see that the first step shows as `done` and the next two are still pending. Let's keep going. Add another private variable to the test class:

	private int result;

Then assign it to the return of the `Add()` method of the calculator in `WhenIAdd`:

	result = calculator.Add(p0);

Building the solution will fail again, so let's implement the method:

	public int Add(int val)
    {
        return Value += val;
    }

Checking the test output indicates that we're still pending, but another step indicates done. Let's go ahead and finish out the test by asserting our output is the expected result:

	Assert.AreEqual(p0, result);

Note that you'll have to add the following at the top of the class for `Assert` to work:

	using Microsoft.VisualStudio.TestTools.UnitTesting;

Build and run the tests and you should see the `Add` test now passing.

All that's left is to finish out our `Subtract` test. Let's start with the test body:

	result = calculator.Subtract(p0);

We can build, but it fails so we need to implement the method:

	public int Subtract(int val)
    {
        return Value -= val;
    }

Once that's done, build and run the tests. You should see everything pass.

### What's next? ###

A later revision will include more complex interactions such as `Scenario Outline/Examples`, tabular data, type casting arguments, and dynamics.
