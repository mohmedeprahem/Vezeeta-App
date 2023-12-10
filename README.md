# algoriza-internship-42

## Description
Revolutionizing healthcare access with a user-friendly app. From quick appointment scheduling to managing crucial health data, it's your all-in-one solution for effortless CRUD operations and a smoother healthcare journey.

## DataBase Design

Database Design
<div style="text-align:center;">
  <img src="https://cdn.discordapp.com/attachments/443452026739752960/1183414473923502090/image.png?ex=65883f9d&is=6575ca9d&hm=8dd5a6d3712bf012760917865f03ce136882eb6d9877e9af5f15c45e1c37acf9&" alt="Database design" />
</div>

## Documentation

- [Api Design](https://algorizainternship.docs.apiary.io/#reference/0/booking/get-all-bookings-for-doctor)

## Installation

### Clone the Repository:

```bash
# Clone repo
$ git clone https://gitlab.com/mohmedeprahem/algoriza-internship-42.git
```
### Download database backup
- [Database Backup](https://drive.google.com/file/d/13EdIUEnL8pkXMPrj3baxHejmEywHb4Eg/view?usp=sharing)

### Update App Settings:
Open the appsettings.json file and update the configuration settings:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YourDatabaseConnectionString"
  },
  "Jwt": {
    "SecretKey": "YourJwtSecretKey",
    "Issuer": "YourJwtIssuer",
    "Audience": "YourJwtAudience"
  },
  "MailSettings": {
    "RealMail": "YourEmail",
    "Host": "YourHostType",
    "Password": "YourEmailPassword",
    "Port": "YourMailPort",
    "UseSSL": true
  }
}
```

## Restore Dependencies

```bash
$ dotnet restore
```
## Apply Migrations
Execute the following command to apply migrations and update the database:

```bash
$ dotnet ef database update
```

## License

[MIT licensed](LICENSE).
