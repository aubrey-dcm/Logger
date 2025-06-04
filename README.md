README.md
# Logger API

A simple ASP.NET Core (.NET 8) Web API that logs incoming requests and outgoing responses to JSON files, organized by date, day, and hour. The API provides a `/logger/test` endpoint that accepts any JSON payload and logs both the request and a standard response.

## Features

- Accepts any JSON payload via `POST /logger/test`
- Logs request and response payloads to disk as JSON
- Log files are grouped by day and hour, and further grouped by minute (all logs within the same minute are appended to the same file as a JSON array)
- Automatic folder structure: `Logs/ Request/ yyyyMMdd/ DD/ HH/ yyyyMMddHHmm.json Response/ yyyyMMdd/ DD/ HH/ yyyyMMddHHmm.json`
- Uses Swagger (OpenAPI) for easy testing

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Build and Run

1. Clone the repository or copy the project files.
2. Restore dependencies: `dotnet restore`
3. Build the project: `dotnet build`
4. Run the API: `dotnet run`

5. Open your browser and navigate to `https://localhost:<port>/swagger` to access the Swagger UI.

### Usage

- **POST** `/logger/test`
- **Body:** Any valid JSON
- **Response:** 
 ```json
 {
   "code": "ok",
   "message": "success"
 }
 ```
- Both the request and response will be logged in their respective folders.

### Log File Structure

- Each log file is named by the current minute (`yyyyMMddHHmm.json`).
- If multiple requests occur within the same minute, their payloads are appended to the same file as a JSON array.

### Example

Suppose you send a POST request to `/logger/test` at 2024-06-04 13:37. The request and response will be logged as:

```
Logs/Request/20240604/04/13/202406041337.json
Logs/Response/20240604/04/13/202406041337.json

```


If another request is made within the same minute, its payload will be appended to the same file.

## Notes

- The logging middleware is included but does not perform logging; all logging is handled in the controller.
- The project uses [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) for Swagger support.

## License

This project is licensed under the MIT License.
