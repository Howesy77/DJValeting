[![.NET](https://github.com/Howesy77/DJValeting/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Howesy77/DJValeting/actions/workflows/dotnet.yml)

## DJ Valeting
6B Digital Car Valeting Technical Test

# Setup
1. Open command prompt 
2. Change to `src` directory within this repository
3. Execute `dotnet restore`
4. Install EF toolkit if required `dotnet tool install --global dotnet-ef`
5. Set database connection string in `SixB.CarValeting.Web\appsettings.json`
6. Deploy the database `dotnet ef database update --project SixB.CarValeting.Web`
7. Execute `dotnet run --project SixB.CarValeting.Web`
8. Browse to `https://localhost:7272`

# Notes
The default login credentials are :-
* Username: test@email.com
* Password: password
