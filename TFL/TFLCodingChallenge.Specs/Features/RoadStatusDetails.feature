Feature: RoadStatusDetails

As a developer,
I want to retrieve the status of a roads in London
So that I can use the information in my application.

 Scenario: Retrieve RoadStatus for Valid RoadId
 Given  I make an HTTP GET request to TflRoad Api by passing roadid "A2"
 When the http status code is "OK"
 Then the response should be in JSON format
 And the response should contain the following information:
    | road_id | PropertyName              | ExpectedValue         |
    | A2      | displayName               | A2                    |
    | A2      | statusSeverity            | Good                  |
    | A2      | statusSeverityDescription | No Exceptional Delays |


 Scenario: Retrieve Road Status for Invalid RoadId
 Given  I make an HTTP GET request to TflRoad Api by passing roadid "A233"
 When the http status code is "NotFound"
 Then the response should be in JSON format
 And the response should return an informative error "The following road id is not recognised: A233"