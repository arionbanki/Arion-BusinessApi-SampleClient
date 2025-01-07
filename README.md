![Logo](https://github.com/arionbanki/Arion-BusinessApi-SampleClient/blob/main/doc-images/01%20-%20arionlogoblue.png?raw=true)

# Arion Business API Sample Client
A basic .net sample client that shows examples on how to consume Business API

## Endpoints
### 1. api/v1/cards
Reads a list of cards potentially with additional information, e.g. balance information.

### 2. api/v1/cards/{cardId}
Reads details about a card.

### 3. api/v1/cards/{cardid}/transactions
Reads transaction list or transaction report from a given card addressed by "card-id".

# Arion's Open Banking platform


## Documentation

  ### Getting Started
  
  Login/Register to the Arion WebPortal

Go to https://developer.arionbanki.is/ and click "Get Started"

![App Screenshot](https://github.com/arionbanki/Arion-BusinessApi-SampleClient/blob/main/doc-images/02%20-%20Getting%20Started.png)

After you've successfully logged in, fill in your information:

![App Screenshot](https://github.com/arionbanki/Arion-BusinessApi-SampleClient/blob/main/doc-images/03%20-%20Lets%20Get%20To%20Know%20You.png?raw=true)

Now you need to go to registered developer's email and confirm it

![App Screenshot](https://github.com/arionbanki/Arion-BusinessApi-SampleClient/blob/main/doc-images/04%20-%20Confirm%20Email.png?raw=true)

After you've confirmed your email you will get a notification like this and you'll be redirected to the main page to create your first application

![App Screenshot](https://github.com/arionbanki/Arion-BusinessApi-SampleClient/blob/main/doc-images/05%20-%20Email%20Confirmed.png?raw=true)

Click "Add Application"

![App Screenshot](https://github.com/arionbanki/Arion-BusinessApi-SampleClient/blob/main/doc-images/06%20-%20Create%20Application.png?raw=true)

Enter the name of your Application

Enter a public description of your Application

Then hit the "Next Step" button

After that you'll have to select your product, in this case the Business API.

Click "Next Step"

![App Screenshot](https://github.com/arionbanki/Arion-BusinessApi-SampleClient/blob/main/doc-images/07%20-%20Create%20Application2.png?raw=true)

Next fill out your company details

Click "Next Step"

After that you'll be enrolled in our Business API Sandbox, and you'll see the app details, e.g. ApiKey and the scopes your App has access to.

You will use this data in your code to connect to Arion's OpenBanking services.

![App Screenshot](https://github.com/arionbanki/Arion-BusinessApi-SampleClient/blob/main/doc-images/08%20-%20CreateApplication3.png?raw=true)

Next head into the Data menu and create your Users

![App Screenshot](https://github.com/arionbanki/Arion-OpenBanking-Sandbox/blob/main/doc-images/14%20-%20Manage%20Users.png?raw=true)

Hit "Add User" - then select your newly created user in the grid to generate data for him/her

To create data for that user, hit "Create Credit Card":

![App Screenshot](https://github.com/arionbanki/Arion-BusinessApi-SampleClient/blob/main/doc-images/09%20-%20Create%20Sandbox%20Cards.png?raw=true)

Next you need to generate a token. For the Sandbox, it's possible to do that by hitting opening your product and selecting "Create Token"

This token will be used to connect to our OpenBanking Api services:

![App screenshot](https://github.com/arionbanki/Arion-BusinessApi-SampleClient/blob/main/doc-images/10%20-%20Generate%20Sandbox%20Token.png?raw=true)

Select your App and the person you want to generate a token for. If you've created multiple users, you will get multiple users from this dropdown. If not, you'll only see one user.

Click "Create Token" tok get your OpenId token to use in your client for our OpenBanking services

Your token will be created and you can copy the value to use in your application, and set it as bearer in the http header of your application:

( please note this token will only be displayed once in this page - you can however create a new token anytime you like)
![App screenshot](https://github.com/arionbanki/Arion-BusinessApi-SampleClient/blob/main/doc-images/11%20-%20Generate%20Sandbox%20Token2.png?raw=true)

Next you can download our sample client code from 

[https://github.com/arionbanki/Arion-OpenBanking-Sandbox/tree/main/IsIT.OpenBanking.Sandbox.DeveloperConsole](https://github.com/arionbanki/Arion-BusinessApi-SampleClient/tree/main/Arion-BusinessApi-SampleClient)

and put the data you've created in the previous steps into the Program.cs client to connect:

Or try out our Postman Collection. You can add the token you created and add it Postman like this:

![App screenshot](https://github.com/arionbanki/Arion-BusinessApi-SampleClient/blob/main/doc-images/12%20-%20Postman%20Sandbox%20Token.png?raw=true)

You're all set! ;-)
