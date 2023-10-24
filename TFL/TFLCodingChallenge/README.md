Developer Notes:
1 - How to build the code:
  #navigate to your project directory i.e TFLCodingChallenge folder. 
  Open appsettings.json, Replace values of app_id and app_key with your Developer Id and Key.
  #navigate to your project sub folder i.e TFLCodingChallenge.specs folder. 
  Open specflow.json, Replace values of app_id and app_key with valid TFL Developer Id and Key.
  #Open a command prompt or terminal and navigate to your project directory i.e TFLCodingChallenge folder 
  #In my case path was:"C:\Users\Temp user\source\repos\TFLCodingChallenge"
  #Either Use the .NET CLI to build your application. Run the following command:
	#dotnet build TFLCodingChallenge.csproj -c Release

  #Altenatively, Open VisualStudio 2022/2019. Click on File in menubar >> Open >> Project/Solution
  #Select the TFLCodingChallenge Solution and Build the solution.

2 - How to run the output:
  #Open a command prompt or terminal and navigate to the release folder of project
  #In my case path was: C:\Users\Temp user\source\repos\TFLCodingChallenge\bin\Release\net6.0
  #Run the command to execute the executable: .\TFLCodingChallenge.exe

  #Altenatively, Open VisualStudio 2022/2019. Click on File in menubar >> Open >> Project/Solution
  #Select the TFLCodingChallenge Solution from Solution explorer and Run the solution by pressing F5.  

	  #Case 1: You will be prompted to enter a roadname. Please enter a Valid Roadname
	  #If you entered a valid road name such as A1. You will see the following console ouput:
	  --------------------------------------------------------------
		The status of the A1 is as follows:
		Road Status is Good
	    Road Status Description is No Exceptional Delays
      --------------------------------------------------------------
	  #Run the command (Incase you are using CommandPrompt cmdline): echo %errorlevel%
	                   (Incase you are using powerhsell):echo $lastexitcode
	  #ExpectedResult is 0
      ====================================================================================
	  #Case 1: You will be prompted to enter a roadname. Please enter a Valid Roadname
	  #If you entered a invalid road name such as A55. You will see the following console ouput:
	  --------------------------------------------------------------
      A55 is not a valid road
      --------------------------------------------------------------
	  #Run the command (Incase you are using CommandPrompt cmdline): echo %errorlevel%
	                   (Incase you are using powerhsell):echo $lastexitcode
	  #ExpectedResult is 1
	  ===================================================================================   

3 - How to run any tests that you have written:
   #Open VisualStudio 2022/2019. Click on File in menubar >> Open >> Project/Solution
   #Select the TFLCodingChallenge Solution.
   #Click on Test in menu bar, go to Test Explorer. Click on first button to run all tests in view
   #(Tests include UnitTests and SpecFlowTests)

4 - Any assumptions that you ve made:
    #Initially, I chose TDD pattern of implementation, later I tried my hands at writing specflow.
	#Although, I have theoritical knowledge of BDD, In my last project we just start writing      integrationtests/specflows for new features.
 We had a plan to eventually improve integration test coverage for all services.
    #For expected behavior of this assignment, I have tried to cover all scenarios as part of 2 specflow scenarios.
	In first scenario, I have covered the flow if displaying valid road DisplayName, StatusSeverity, StatusSeverityDescription under one scenario.
	In second scenario, showing error information for invalid road.

5 - Anything else you think is relevant:
   #I have used HttpClientFactory instead of HttpClient as best practice to 
   avoid socket exhaustion and to delegate management of httpclient instances.
   For any queries about implementation, please drop an email to prathyusha.konda@gmail.com


  