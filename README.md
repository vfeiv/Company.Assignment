<a name="readme-top"></a>

<h1 align="center">Aggregation Api</h1>


<!-- ABOUT THE PROJECT -->
## About The Project

API aggregation service that consolidates data from multiple
external APIs and provides a unified endpoint to access the aggregated information. 

<!--CONNECTED APIs -->
## Connected APIs

The service currently connects with the following external APIs:


* OpenWeatherMap API: [https://openweathermap.org/api](https://openweathermap.org/api)

This API provides weather data for various locations around the world. You can
request weather data by geographic coordinates.

* Tiingo API: [https://www.tiingo.com/](https://www.tiingo.com/)

This API provides latest or historical price data for a stock. You can specify the stock and the desired date range for the historical data.

* News API: [https://newsapi.org/](https://newsapi.org/)

The News API provides access to headlines and articles from various news sources
around the world. You can use this API to aggregate news articles based on specific
keywords.

<!-- SETUP -->
## Setup

1. Get a free API Key for each one of the supported external APIs you want to utilize
2. Clone the repo
   ```sh
   git clone https://github.com/vfeiv/Company.Assignment.git
   ```
3. Configure your external APIs in `appsettings.json`
      ```
      "ExternalApis": {
        "OpenWeatherMap": {
          ...
          "ApiKey": "YOUR OpenWeatherMap API KEY",
          ...
        },
        "Tiingo": {
          ...
          "ApiKey": "YOUR Tiingo API KEY",
          ...
        },
        "News": {
          ...
          "ApiKey": "YOUR News API KEY",
          ...
        }
      }
      ```



<!-- INTEGRATION OF NEW API -->
## Integration of new API

Below you will find instructions on integrating with a new external API.

1. Configure your new external APIs in `appsettings.json`. 
The base URL and the AuthorizationType are required for the external API to be configured correctly.
    
    ```
      "ExternalApis": {
        ...
        "NEW EXTERNAL API NAME": {
          "BaseUrl": "YOUR NEW EXTERNAL API BASE URL" // this is required
          "ApiKey": "YOUR NEW EXTERNAL API API KEY",
          "AuthorizationType": "YOUR NEW EXTERNAL API AUTHORIZATION TYPE", // QueryParams or Bearer
        }
        ...
      }
    ```
2. Create your external API response models (for both success and error responses) in the `src/Company.Assignment.Core/ExternalApiClients/Models/NEW_EXTERNAL_API_NAME` folder as well as your DTOs in the `src/Company.Assignment.Common/Dtos` folder.

3. Create your model mapper in the `src/Company.Assignment.Core/Mappers` folder.

4. Create filters, if applicable, for your API request/s in the `src/Company.Assignment.Common/Filters` folder and include them to the `src/Company.Assignment.Common.Filters.AggregateFilter.cs` file.

5. Create an interface for you external API operations in the `src/Company.Assignment.Core/src/Abstractions/ExternalApiClients` folder and implement it in the `src/Company.Assignment.Core/ExternalApiClients` folder. Make sure to derive from the `src/Company.Assignment.Core/ExternalApiClients/BaseExternalApiClient.cs` base class.

6. Register your new external Api Client service as well as your model mapper created in step #3 in the `src/Company.Assignment.Core/Extensions/ServiceCollectionExtensions.cs` file.

7. Configure `src/Company.Assignment.Core/Services/AggregateService.cs` to welcome your new API and aggregate the new data.



<!-- AUTHENTICATION -->
## Authentication

The app uses Azure AD to secure the API endpoint using  OAuth 2.0 Authorization Code Grant Type.



<p align="right">(<a href="#readme-top">back to top</a>)</p>


