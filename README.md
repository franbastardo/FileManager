to run the project you will need to follow the next steps

install NuGet packages:

AutoMapper.Extensions.Microsoft.DependencyInjection
AWSSDK.S3
BCrypt.Net-Next
Microsoft.AspNetCore.Authentication
Microsoft.AspNetCore.Authentication.JwtBearer
Microsoft.AspNetCore.Mvc.NewtonsoftJson
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.Tools
Serilog.AspNetCore
Swashbuckle.AspNetCore

in the root of the project create .env file with following format

##S3
S3KEY= 
S3SECRET=
S3BUCKETNAME=

##Jwt
KEY=

##UNSPLASH
ACCESSKEY=

##SendGrid
SGKEY=
SGEMAIL=


