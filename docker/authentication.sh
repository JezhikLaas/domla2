#!/bin/bash

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" <<-EOSQL
	CREATE USER d2admin PASSWORD 'd2admin';
	CREATE DATABASE "D2.Authentication" OWNER d2admin;
EOSQL


psql -v ON_ERROR_STOP=1 --username d2admin --dbname D2.Authentication <<-EOSQL
CREATE TABLE api_resources (
    name character varying(255) NOT NULL,
    data jsonb NOT NULL
);


ALTER TABLE api_resources OWNER TO d2admin;

CREATE TABLE clients (
    id character varying(255) NOT NULL,
    data jsonb NOT NULL
);


ALTER TABLE clients OWNER TO d2admin;

CREATE TABLE identity_resources (
    name character varying(255) NOT NULL,
    data jsonb NOT NULL
);


ALTER TABLE identity_resources OWNER TO d2admin;

CREATE TABLE persisted_grants (
    key character varying(255) NOT NULL,
    type character varying(255) NOT NULL,
    subject_id character varying(255),
    client_id character varying(255) NOT NULL,
    creation_time timestamp without time zone NOT NULL,
    expiration timestamp without time zone,
    data text NOT NULL
);


ALTER TABLE persisted_grants OWNER TO d2admin;

CREATE TABLE registrations (
    login character varying(255) NOT NULL,
    first_name character varying(255),
    last_name character varying(255) NOT NULL,
    email character varying(255) NOT NULL,
    salutation character varying(50),
    title character varying(50),
	mail_sent timestamp without time zone,
    id uuid NOT NULL
);


ALTER TABLE registrations OWNER TO d2admin;

CREATE TABLE users (
    id uuid NOT NULL,
    login character varying(255) NOT NULL,
    first_name character varying(255),
    last_name character varying(255) NOT NULL,
    title character varying,
    salutation character varying,
    email character varying(255) NOT NULL,
    claims jsonb,
    password character varying(255) NOT NULL,
    logged_in timestamp without time zone,
	privacy_accepted timestamp without time zone
);


ALTER TABLE users OWNER TO d2admin;

ALTER TABLE ONLY api_resources
    ADD CONSTRAINT api_resources_pkey PRIMARY KEY (name);

ALTER TABLE ONLY clients
    ADD CONSTRAINT clients_pkey PRIMARY KEY (id);

ALTER TABLE ONLY identity_resources
    ADD CONSTRAINT identity_resources_pkey PRIMARY KEY (name);

ALTER TABLE ONLY persisted_grants
    ADD CONSTRAINT persisted_grants_pkey PRIMARY KEY (key);

ALTER TABLE ONLY registrations
    ADD CONSTRAINT registrations_pkey PRIMARY KEY (login);

ALTER TABLE ONLY users
    ADD CONSTRAINT users_pkey PRIMARY KEY (id);

CREATE INDEX idx_expiration ON persisted_grants USING btree (expiration);

CREATE INDEX idx_subject_client_type ON persisted_grants USING btree (subject_id, client_id, type);

CREATE INDEX idxscopes ON api_resources USING gin ((((data -> 'Scopes'::text) -> 'Name'::text)));

COPY api_resources (name, data) FROM stdin;
api	{"Name": "api", "Scopes": [{"Name": "api", "Required": false, "Emphasize": false, "UserClaims": [], "Description": null, "DisplayName": "REST Api", "ShowInDiscoveryDocument": true}], "Enabled": true, "ApiSecrets": [{"Type": "SharedSecret", "Value": "TKYCKA5tJA77gVUH5VTdrirPrMDTEz22K8+pNLodlQo=", "Expiration": null, "Description": null}], "UserClaims": ["name", "role", "id"], "Description": null, "DisplayName": "REST Api"}
\.

COPY clients (id, data) FROM stdin;
service	{"Claims": [], "Enabled": true, "LogoUri": null, "ClientId": "service", "ClientUri": null, "ClientName": null, "Properties": {}, "RequirePkce": false, "IncludeJwtId": false, "ProtocolType": "oidc", "RedirectUris": [], "AllowedScopes": ["api"], "ClientSecrets": [{"Type": "SharedSecret", "Value": "ZAzamJJz9RPgrQzv+dcDVHg0/en8V0PxvRsAlTX5NxA=", "Expiration": null, "Description": null}], "RequireConsent": true, "AccessTokenType": 1, "ConsentLifetime": null, "EnableLocalLogin": true, "AllowedGrantTypes": ["client_credentials"], "RefreshTokenUsage": 1, "AllowOfflineAccess": false, "AllowPlainTextPkce": false, "AllowedCorsOrigins": [], "ClientClaimsPrefix": "client_", "AccessTokenLifetime": 3600, "PairWiseSubjectSalt": null, "RequireClientSecret": true, "AllowRememberConsent": true, "BackChannelLogoutUri": null, "FrontChannelLogoutUri": null, "IdentityTokenLifetime": 300, "AlwaysSendClientClaims": false, "PostLogoutRedirectUris": [], "RefreshTokenExpiration": 1, "AuthorizationCodeLifetime": 300, "AllowAccessTokensViaBrowser": false, "SlidingRefreshTokenLifetime": 1296000, "AbsoluteRefreshTokenLifetime": 2592000, "IdentityProviderRestrictions": [], "AlwaysIncludeUserClaimsInIdToken": false, "BackChannelLogoutSessionRequired": true, "UpdateAccessTokenClaimsOnRefresh": false, "FrontChannelLogoutSessionRequired": true}
interactive	{"Claims": [], "Enabled": true, "LogoUri": null, "ClientId": "interactive", "ClientUri": null, "ClientName": "Interactive user", "Properties": {}, "RequirePkce": false, "IncludeJwtId": false, "ProtocolType": "oidc", "RedirectUris": ["http://localhost:8130/signin-oidc", "http://localhost:8133/signin-oidc"], "AllowedScopes": ["openid", "profile", "role.profile", "api"], "ClientSecrets": [{"Type": "SharedSecret", "Value": "xlzqKcSAjrXFVep3c1H+n21Ty+rb60GLW2rgobOgfbQ=", "Expiration": null, "Description": null}], "RequireConsent": false, "AccessTokenType": 1, "ConsentLifetime": null, "EnableLocalLogin": true, "AllowedGrantTypes": ["hybrid", "client_credentials"], "RefreshTokenUsage": 1, "AllowOfflineAccess": true, "AllowPlainTextPkce": false, "AllowedCorsOrigins": ["http://localhost:8130", "http://localhost:8133"], "ClientClaimsPrefix": "client_", "AccessTokenLifetime": 3600, "PairWiseSubjectSalt": null, "RequireClientSecret": true, "AllowRememberConsent": true, "BackChannelLogoutUri": null, "FrontChannelLogoutUri": null, "IdentityTokenLifetime": 300, "AlwaysSendClientClaims": false, "PostLogoutRedirectUris": ["http://localhost:8130/signout-callback-oidc", "http://localhost:8133/signout-callback-oidc"], "RefreshTokenExpiration": 1, "AuthorizationCodeLifetime": 300, "AllowAccessTokensViaBrowser": false, "SlidingRefreshTokenLifetime": 1296000, "AbsoluteRefreshTokenLifetime": 2592000, "IdentityProviderRestrictions": [], "AlwaysIncludeUserClaimsInIdToken": true, "BackChannelLogoutSessionRequired": true, "UpdateAccessTokenClaimsOnRefresh": false, "FrontChannelLogoutSessionRequired": true}
d2-cfe	{"Claims": [], "Enabled": true, "LogoUri": null, "ClientId": "d2-cfe", "ClientUri": null, "ClientName": "Customer frontend", "Properties": {}, "RequirePkce": false, "IncludeJwtId": false, "ProtocolType": "oidc", "RedirectUris": ["http://localhost:8130/signin-oidc", "http://localhost:8133/signin-oidc"], "AllowedScopes": ["openid", "profile", "role.profile", "api"], "ClientSecrets": [{"Type": "SharedSecret", "Value": "xlzqKcSAjrXFVep3c1H+n21Ty+rb60GLW2rgobOgfbQ=", "Expiration": null, "Description": null}], "RequireConsent": false, "AccessTokenType": 1, "ConsentLifetime": null, "EnableLocalLogin": true, "AllowedGrantTypes": ["implicit"], "RefreshTokenUsage": 1, "AllowOfflineAccess": true, "AllowPlainTextPkce": false, "AllowedCorsOrigins": ["http://localhost:8130", "http://localhost:8133"], "ClientClaimsPrefix": "client_", "AccessTokenLifetime": 3600, "PairWiseSubjectSalt": null, "RequireClientSecret": true, "AllowRememberConsent": true, "BackChannelLogoutUri": null, "FrontChannelLogoutUri": null, "IdentityTokenLifetime": 300, "AlwaysSendClientClaims": false, "PostLogoutRedirectUris": ["http://localhost:8130/signout-callback-oidc", "http://localhost:8133/signout-callback-oidc"], "RefreshTokenExpiration": 1, "AuthorizationCodeLifetime": 300, "AllowAccessTokensViaBrowser": false, "SlidingRefreshTokenLifetime": 1296000, "AbsoluteRefreshTokenLifetime": 2592000, "IdentityProviderRestrictions": [], "AlwaysIncludeUserClaimsInIdToken": true, "BackChannelLogoutSessionRequired": true, "UpdateAccessTokenClaimsOnRefresh": false, "FrontChannelLogoutSessionRequired": true}
admin-client	{"Claims": [], "Enabled": true, "LogoUri": null, "ClientId": "admin-client", "ClientUri": null, "ClientName": "Admin frontend", "Properties": {}, "RequirePkce": false, "IncludeJwtId": false, "ProtocolType": "oidc", "RedirectUris": ["http://localhost:8133"], "AllowedScopes": ["openid", "profile", "role.profile", "api"], "ClientSecrets": [], "RequireConsent": false, "AccessTokenType": 1, "ConsentLifetime": null, "EnableLocalLogin": true, "AllowedGrantTypes": ["implicit"], "RefreshTokenUsage": 1, "AllowOfflineAccess": true, "AllowPlainTextPkce": false, "AllowedCorsOrigins": ["http://localhost:8130"], "ClientClaimsPrefix": "client_", "AccessTokenLifetime": 3600, "PairWiseSubjectSalt": null, "RequireClientSecret": true, "AllowRememberConsent": true, "BackChannelLogoutUri": null, "FrontChannelLogoutUri": null, "IdentityTokenLifetime": 300, "AlwaysSendClientClaims": false, "PostLogoutRedirectUris": ["http://localhost:8133/"], "RefreshTokenExpiration": 1, "AuthorizationCodeLifetime": 300, "AllowAccessTokensViaBrowser": true, "SlidingRefreshTokenLifetime": 1296000, "AbsoluteRefreshTokenLifetime": 2592000, "IdentityProviderRestrictions": [], "AlwaysIncludeUserClaimsInIdToken": true, "BackChannelLogoutSessionRequired": true, "UpdateAccessTokenClaimsOnRefresh": false, "FrontChannelLogoutSessionRequired": true}
\.

COPY identity_resources (name, data) FROM stdin;
role.profile	{"Name": "role.profile", "Enabled": true, "Required": false, "Emphasize": false, "UserClaims": ["role", "id"], "Description": null, "DisplayName": "Role profile", "ShowInDiscoveryDocument": true}
openid	{"Name": "openid", "Enabled": true, "Required": true, "Emphasize": false, "UserClaims": ["sub"], "Description": null, "DisplayName": "Your user identifier", "ShowInDiscoveryDocument": true}
profile	{"Name": "profile", "Enabled": true, "Required": false, "Emphasize": true, "UserClaims": ["name", "family_name", "given_name", "middle_name", "nickname", "preferred_username", "profile", "picture", "website", "gender", "birthdate", "zoneinfo", "locale", "updated_at"], "Description": "Your user profile information (first name, last name, etc.)", "DisplayName": "User profile", "ShowInDiscoveryDocument": true}
email	{"Name": "email", "Enabled": true, "Required": false, "Emphasize": true, "UserClaims": ["email", "email_verified"], "Description": null, "DisplayName": "Your email address", "ShowInDiscoveryDocument": true}
phone	{"Name": "phone", "Enabled": true, "Required": false, "Emphasize": true, "UserClaims": ["phone_number", "phone_number_verified"], "Description": null, "DisplayName": "Your phone number", "ShowInDiscoveryDocument": true}
address	{"Name": "address", "Enabled": true, "Required": false, "Emphasize": true, "UserClaims": ["address"], "Description": null, "DisplayName": "Your postal address", "ShowInDiscoveryDocument": true}
\.

COPY users (id, login, first_name, last_name, title, salutation, email, claims, password, logged_in) FROM stdin;
0d7e5c06-6a16-4126-b854-ca29cf622044	admin	\N	<unknown>	\N	\N	<unknown>	[{"Type": "role", "Value": "admin", "Issuer": "LOCAL AUTHORITY", "Subject": null, "ValueType": "http://www.w3.org/2001/XMLSchema#string", "Properties": {}, "OriginalIssuer": "LOCAL AUTHORITY"}, {"Type": "name", "Value": "admin", "Issuer": "LOCAL AUTHORITY", "Subject": null, "ValueType": "http://www.w3.org/2001/XMLSchema#string", "Properties": {}, "OriginalIssuer": "LOCAL AUTHORITY"}, {"Type": "id", "Value": "a4ed96cf0a5e485fa57453f2b1b11454", "Issuer": "LOCAL AUTHORITY", "Subject": null, "ValueType": "http://www.w3.org/2001/XMLSchema#string", "Properties": {}, "OriginalIssuer": "LOCAL AUTHORITY"}]	\$2a\$10\$0aNeUUZKGm4FHI2mUYRdIe0UNyhPMbbFtTs62kvw5kCE1363TqbK.	\N
\.

EOSQL
