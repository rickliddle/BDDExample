
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