# Rosters-and-Colors


Describe Solution:
My first steps in building any solution is to set up Authorization and Authentication using jwt.

Then set up a logging mechanism to help understand what is happening in the application
my goto for this is Serilog.

Then I break up my application to have different aspects to it:

API project: This is the entry point for client applications.
Core project: Bears the responsibilty managing all business logic and data access processing
Test project: This is bears the responsibility of handling the unit tests of the application majorly 
              the endpoints implemented.
Then to solve this problem i will create the endpoints below.

              
Endpoints
There are endpoints
1. To get all rosters and their unique colors created by an authenticated user.
2. To create a roster and its unique color.
3. To delete roster.
4. To delete color from roster.
5.To create a user.
6.To authenticate the user.
