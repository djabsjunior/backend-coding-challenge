# Coveo Backend Coding Challenge

# SUGGESTIONS REST API


## Description

The REST API endpoint provides auto-complete suggestions for large cities.

- The endpoint is exposed at `https://suggestionsapicoveobackendchallenge.azurewebsites.net/suggestions/suggestions` (Microsoft Azure).
- The partial (or complete) search term is passed as a querystring parameter `q`
- The optionals querystring parameters `latitude` and `longitude` help improve the relative scores.
- The cities data are collected from the Geonames API at `http://api.geonames.org/searchJSON?name_startsWith={q}&cities=cities5000&maxRows=10&country=US&country=CA&style=MEDIUM&username=jbvouma` where `q` is the partial (or complete) search term (see second point).
- The endpoint returns a JSON response with an array of scored suggested matches
    - The suggestions are sorted by descending score
    - Each suggestion has a score between 0 and 1 (inclusive) indicating confidence in the suggestion (1 is most confident)
    - Each suggestion has a name which can be used to disambiguate between similarly named locations
    - Each suggestion has a latitude and longitude



## Sample responses


## HTTP Status Code

- The client application will receive an HTTP 400 - Bad request response with the message `Bad Request: Invalid parameters. 'q' must be a string, 'longitude' and 'latitude' values must be numbers.` when the querystrings parameters have bad formats.

- The client application will receive an HTTP 200 - Ok when the request was fulfilled


**Near match**

    GET https://suggestionsapicoveobackendchallenge.azurewebsites.net/suggestions?q=Londo&latitude=43.70011&longitude=-79.4163

```json
{
    "suggestions": [
        {
            "name": "London, ON, CA",
            "latitude": "42.98339",
            "longitude": "-81.23304",
            "score": 0.9
        },
        {
            "name": "London, OH, US",
            "latitude": "39.88645",
            "longitude": "-83.44825",
            "score": 0.5
        },
        {
            "name": "London, KY, US",
            "latitude": "37.12898",
            "longitude": "-84.08326",
            "score": 0.5
        },
        {
            "name": "Londonderry, NH, US",
            "latitude": "42.86509",
            "longitude": "-71.37395",
            "score": 0.3
        },
        {
            "name": "Londontowne, MD, US",
            "latitude": "38.93345",
            "longitude": "-76.54941",
            "score": 0.3
        }
    ]
}
```

**No match**

    GET https://suggestionsapicoveobackendchallenge.azurewebsites.net/suggestions?q=SomeRandomCityInTheMiddleOfNowhere

```json
{
  "suggestions": []
}
```

## References

- Geonames provides city lists Canada and the USA http://download.geonames.org/export/dump/readme.txt

