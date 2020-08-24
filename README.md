# InsuranceApp

Web Application in .Net Core to manage insurance policies-

This web site have an Auth0 login to authenticate the users, the web API is proctected with Authorization with bearer access token.

When you have a sucessful login you can see the Customers and Policies pages, in there you can add, edit or delete registers, also you can see the policies by Customer going to customer details, when you create a policy you can attach it to a customer by setting the CustomerId or just leave it empty to attach it later, to deattach a policy from a customer just delete the customer Id.

## Requirements

* .[NET Core 3.1 SDK](https://www.microsoft.com/net/download/core)
* Visual Studio 2019

## To run this project

1. Get a copy of the projec: To grab a copy of the sample code, you can download the zip or clone it locally.

2. Run the InsuranceWebAPI and the InsuranceWebApp projects.

3. You can check the InsuranceWebAPI swagger at `https://localhost:44383/`.

4. Then you can go to `https://localhost:44316/` in your web browser to view the website. click login and enter these credetials
user: demo@insuranceapp.com
password: InsuranceApp2020

you can enter with a google account too.

5. Also you can run the Unit Test on the InsuranceWebAPI Project directly in Visual Studio or using these commands to collect the coverage and make a report in html.

dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput="./TestResults/"
dotnet reportgenerator "-reports:TestResults\coverage.cobertura.xml" "-targetdir:TestResults\html" -reporttypes:HTML;
