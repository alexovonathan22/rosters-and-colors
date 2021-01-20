# Rosters-and-Colors


Describe Solution:
My first steps in building any solution is to set up Authorization and Authentication using jwt.

Then set up a logging mechanism to help understand what is happening in the application
my goto for this is Serilog. I also set up for api documention with swagger/postman.
Swagger is used in this application.

Then I break up my application to have different aspects to it:

#API project: This is the entry point for client applications.
#Core project: Bears the responsibilty managing all business logic and data access processing.
#Test project: This is bears the responsibility of handling the unit tests of the application majorly 
              the endpoints implemented.
              
#Then to solve this problem i will create the endpoints below.

              
Endpoints
There are endpoints
1. To get all rosters and their unique colors created by an authenticated user.
2. To create a roster and its unique color.
3. To delete roster.
4. To delete color from roster.
5.To create a user.
6.To authenticate the user.

Roles:
Church Admin can create / add / delete rosters and colors
Members can only get all rosters and colors.


To Run the Application:
Clone the Project on your machine and restore all dependencies.
Then run the application a Swagger UI will pop up on your browser then you can test the app as a Church admin or Member.
To test as admin use { "username": "adminovo", "password": "01234Admin" } to login obtain a token and pass that
token to the authorize button on the top right of the UI.

Expected Error Codes:
401: UnAuthenticated user,
403: UnAuthorised user,
404: BadResquest and Request not succesful.
200: Successful request
