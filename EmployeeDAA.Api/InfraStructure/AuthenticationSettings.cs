﻿namespace EmployeeDAA.Api.InfraStructure
{
    public class AuthenticationSettings
    {
        public string SecretKey { get; set; }
        public string APISecretKey { get; set; }
        public string APIClientId { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
