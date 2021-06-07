# String Calculator TDD Workshop
## General requirements

You may use whatever programming language/platform you prefer.

**IMPORTANT**: Implement the requirements focusing on writing the best code you can produce.

## Step 1 
Create a simple String calculator with a method int Add(string numbers).

1. The method can take 0, 1 or 2 numbers, separated by commas, and will return their sum, examples: 
  * **“”** should return **0**
  * **“1”** should return **1**
  * **“1,2”** should return **3**
2. Start with the simplest test case of an empty string and move to one and two numbers.
3. Remember to solve things as simply as possible so that you force yourself to write tests you did not think about.
4. Remember to refactor after each passing test.


## Step 2
Allow the Add method to handle an unknown amount of numbers.

## Step 3
Calling Add with a negative number will throw an exception **“negatives not allowed”** - and the negative that was passed.

If there are multiple negatives, show all of them in the exception message


## Step 4
Numbers bigger than 1000 should be ignored, for example **“1000,2”** should return **2**


## Step 5
Support different delimiters
Delimiters can be of any length with the following format: 
  
* **“//[delimiter]//”**
  
for example: 

* **“//[\*\*\*]//1\*\*\*2\*\*\*3”**
  
should return **6**


## Step 6
Allow multiple delimiters like this:  

* **“//[delim1][delim2]//”**

for example 

* **“//[\*][%]//1\*2%3”**

should return **6**


## Step 7
Make sure you can also handle multiple delimiters with length longer than one char
