## store the dependency:
  - dotnet restore

## use gRPC:
  - ADD  
  "Kestrel": {
    "EndpointDefaults": {
      "Protocols": "Http2"
    }
  } 
  IN appsetting.json

## use http:
  - REMOVE
  "Kestrel": {
    "EndpointDefaults": {
      "Protocols": "Http2"
    }
  } 
  IN appsetting.json
