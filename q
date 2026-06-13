[33mcommit 61d960578e93297f4fa2d7ec2a17c5bf95486334[m[33m ([m[1;36mHEAD[m[33m -> [m[1;32mmain[m[33m)[m
Author: CodedByWaheed <waheed20032018@gmail.com>
Date:   Sat Jun 13 17:06:31 2026 +0300

     Refactor QrsData with DTO and CRUD operations , adding Scan Function

[33mcommit 9d6f2abb7c79ab5ee54b02069077acd680ea5485[m
Author: CodedByWaheed <waheed20032018@gmail.com>
Date:   Sat Jun 13 16:51:19 2026 +0300

    Refactor and expand BreadPoints business logic class
    
    Refactored BreadPoints from internal to public and added full CRUD support, property definitions, constructors, and data layer integration using DTOs. Replaced placeholder code with a comprehensive business logic implementation for managing bread points.

[33mcommit 3040bb1fdce9ad524b4f01401547939c2683b4e6[m[33m ([m[1;31morigin/main[m[33m, [m[1;31morigin/HEAD[m[33m)[m
Merge: 5580cfe 10f5679
Author: CodedByWaheed <waheed20032018@gmail.com>
Date:   Sat Jun 13 16:34:55 2026 +0300

    Refactor BreadPointsData with DTO and CRUD operations
    
    Refactored BreadPointsData.cs to introduce a BreadPointDTO model and a public BreadPointsData class with static CRUD methods using stored procedures and ADO.NET. Added support for filtering, pagination, and mapping database rows to DTOs. Cleaned up unused usings and updated dependencies.

[33mcommit 5580cfe22948d43b1a7a1ca7a59e0b8fb3657b60[m
Merge: 51960f3 f71665d
Author: CodedByWaheed <waheed20032018@gmail.com>
Date:   Sat Jun 13 16:17:43 2026 +0300

    (*Solve Conflict* )
    Update DeleteUser to support hard delete option
    
    Refactored DeleteUser in BL to accept a HardDelete flag and pass it to the data layer. Modified UsersData.DeleteUser signature, but parameter usage is now inconsistent and may cause errors. Added @HardDelete parameter to SQL command.

[33mcommit 10f567952c374ed15b67e8668f41f118bc2c6194[m
Merge: 51960f3 f71665d
Author: CodedByWaheed <waheed20032018@gmail.com>
Date:   Sat Jun 13 16:17:43 2026 +0300

    (*Solve Conflict* )
    Update DeleteUser to support hard delete option
    
    Refactored DeleteUser in BL to accept a HardDelete flag and pass it to the data layer. Modified UsersData.DeleteUser signature, but parameter usage is now inconsistent and may cause errors. Added @HardDelete parameter to SQL command.

[33mcommit 51960f3d296ecdd48615aefc48a19a5cb2675db6[m
Author: CodedByWaheed <waheed20032018@gmail.com>
Date:   Sat Jun 13 16:12:57 2026 +0300

    Refactor Users model to use nullable types and add CRUD ops
    
    Refactored the Users class to use nullable types for all properties, updated PublicID to Guid?, and added methods for add, update, delete, and retrieval by various criteria. Updated DTOs in BreadApp_DL to use nullable types and improved consistency. Enhanced user creation error handling and made minor formatting improvements.

[33mcommit a1aabdc14758928b6dba55992386eda5ae60d98f[m
Author: CodedByWaheed <waheed20032018@gmail.com>
Date:   Sat Jun 13 15:25:54 2026 +0300

    (* FINISHED *)Refactor UsersData methods and update DTO usage
    
    Refactored UpdateUser to handle detailed user fields and nulls. Changed DeleteUser to support hard deletes and updated connection string. Modified Authenticate to return UserInfoDTO and use NationalNo. Removed unused commented DTO classes. Fixed LoginDTO constructor.

[33mcommit f71665d3f4f29e6798cbb3c75d6dca123b4922c6[m
Author: CodedByWaheed <waheed20032018@gmail.com>
Date:   Sat Jun 13 15:25:54 2026 +0300

    (* FINISHED *)Refactor UsersData methods and update DTO usage
    
    Refactored UpdateUser to handle detailed user fields and nulls. Changed DeleteUser to support hard deletes and updated connection string. Modified Authenticate to return UserInfoDTO and use NationalNo. Removed unused commented DTO classes. Fixed LoginDTO constructor.

[33mcommit edd425d058e600a972686810ac6365bd76ec8749[m
Author: CodedByWaheed <waheed20032018@gmail.com>
Date:   Sat Jun 13 13:05:27 2026 +0300

    Refactor user DAL: new DTOs, flexible queries, config
    
    Refactored user data access layer:
    - Introduced UserDTO, UserInfoDTO, and LoginDTO with constructors and updated properties.
    - Replaced and commented out old CreateUserDTO, UpdateUserDTO, and PagedResult classes.
    - Changed PublicID from int to Guid.
    - Replaced GetUserById with GetUserBy for multi-parameter queries.
    - Updated GetUsers to return List<UserInfoDTO>.
    - Updated CreateUser and UpdateUser to use new DTOs.
    - Moved connection string to clsConnectionSetting.
    - Updated stored procedure calls and parameter handling.
    - Removed or commented out obsolete methods.

[33mcommit 44e41ad77120133ca80fbe7487c1161887ba07c8[m
Merge: 22cbfce 97d1967
Author: CodedByWaheed <waheed20032018@gmail.com>
Date:   Wed Jun 10 12:01:16 2026 +0300

    Merge https://github.com/CodedByWaheed/BreadAppSystem

[33mcommit 22cbfced67b96e79a3bdd64311ca9900d1de72d6[m
Author: CodedByWaheed <waheed20032018@gmail.com>
Date:   Wed Jun 10 11:59:56 2026 +0300

    Add BL/DL layers, user model, and DAL with SQL support
    
    Added BreadApp_BL and BreadApp_DL projects with proper references. Updated API controller routes to explicit names and renamed StudentsControllers for consistency. Introduced Users class in BL and UsersData class in DL with CRUD/auth methods and DTOs
    ***********
    The UsersData Not Finished yet just to same the work before laptop battery die
    ***********
    , using Microsoft.Data.SqlClient for SQL Server access.

[33mcommit 97d196776e5d286dded7fad77c116326812d9b25[m
Author: Waheed <waheed20032018@gmail.com>
Date:   Wed Jun 10 11:35:18 2026 +0300

    Adding ReadMe.md
    
    This README provides an overview of the Bread App, its features, system architecture, technology stack, core workflow, security features, future enhancements, and contributors.

[33mcommit 8f5ea113db513959083c42b09a6537b692951e0d[m
Author: CodedByWaheed <waheed20032018@gmail.com>
Date:   Wed Jun 10 11:32:07 2026 +0300

    Initial .NET 8 Web API solution and project scaffolding
    
    Set up BreadAppSystem with API, BL, and DL projects. Added .gitignore, solution/project files, Swagger, controllers, config files, and placeholder classes for business and data layers. Included HTTP test file and launch settings for development.
