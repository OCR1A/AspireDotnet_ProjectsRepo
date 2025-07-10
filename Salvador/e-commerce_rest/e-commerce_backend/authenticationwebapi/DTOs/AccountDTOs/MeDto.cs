namespace Account.DTOs.AccountDTOs
{

    public class MeDto
    {

        public string? Name { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }

        /*
"name": "Salvador",
"dateCreated": "2025-07-09T00:00:00",
"id": "7906a8e0-af61-461f-a38f-7c90b38dc66d",
"userName": "test1@gmail.com",
"normalizedUserName": "TEST1@GMAIL.COM",
"email": "test1@gmail.com",
"normalizedEmail": "TEST1@GMAIL.COM",
"emailConfirmed": true,
"passwordHash": "AQAAAAIAAYagAAAAEP/uhL7NKhZM3zdvo9eCvqKIkd0gI4IQOmqABp7My+z4SxRfcscX7RS32LAW8i2fgA==",
"securityStamp": "4LL2ZVAMIHG2ZWUKJAOOV3LNRSTAMCVW",
"concurrencyStamp": "ef361368-2bfc-40f6-8101-1dd3d964b9b1",
"phoneNumber": null,
"phoneNumberConfirmed": false,
"twoFactorEnabled": false,
"lockoutEnd": null,
"lockoutEnabled": true,
"accessFailedCount": 0
        */

    }

}