#!/bin/bash

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" <<-EOSQL
	CREATE USER d2broker PASSWORD 'd2broker';
	CREATE DATABASE "D2.ServiceBroker" OWNER d2broker;
EOSQL

psql -v ON_ERROR_STOP=1 --username d2broker --dbname D2.ServiceBroker <<-EOSQL
CREATE TABLE applications (
    name character varying(255) NOT NULL,
    version integer NOT NULL,
    patch integer NOT NULL
);


ALTER TABLE applications OWNER TO d2broker;

CREATE TABLE services (
    application_name character varying(255) NOT NULL,
    application_version integer NOT NULL,
    name character varying(255) NOT NULL,
    version integer NOT NULL,
    patch integer NOT NULL,
    route character varying(512) NOT NULL,
    endpoints jsonb NOT NULL
);


ALTER TABLE services OWNER TO d2broker;

ALTER TABLE ONLY applications
    ADD CONSTRAINT applications_pkey PRIMARY KEY (name, version);


ALTER TABLE ONLY services
    ADD CONSTRAINT services_pkey PRIMARY KEY (application_name, application_version, name, version);

ALTER TABLE ONLY services
    ADD CONSTRAINT applications_services FOREIGN KEY (application_name, application_version) REFERENCES applications(name, version) DEFERRABLE INITIALLY DEFERRED;
EOSQL