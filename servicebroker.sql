--
-- PostgreSQL database dump
--

-- Dumped from database version 10.0
-- Dumped by pg_dump version 10.0

-- Started on 2017-11-18 10:36:51

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 1 (class 3079 OID 12278)
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- TOC entry 2159 (class 0 OID 0)
-- Dependencies: 1
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET search_path = public, pg_catalog;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 196 (class 1259 OID 16556)
-- Name: applications; Type: TABLE; Schema: public; Owner: d2broker
--

CREATE TABLE applications (
    name character varying(255) NOT NULL,
    version integer NOT NULL,
    patch integer NOT NULL
);


ALTER TABLE applications OWNER TO d2broker;

--
-- TOC entry 197 (class 1259 OID 16561)
-- Name: services; Type: TABLE; Schema: public; Owner: d2broker
--

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

--
-- TOC entry 2028 (class 2606 OID 16560)
-- Name: applications applications_pkey; Type: CONSTRAINT; Schema: public; Owner: d2broker
--

ALTER TABLE ONLY applications
    ADD CONSTRAINT applications_pkey PRIMARY KEY (name, version);


--
-- TOC entry 2030 (class 2606 OID 16568)
-- Name: services services_pkey; Type: CONSTRAINT; Schema: public; Owner: d2broker
--

ALTER TABLE ONLY services
    ADD CONSTRAINT services_pkey PRIMARY KEY (application_name, application_version, name, version);


--
-- TOC entry 2031 (class 2606 OID 16576)
-- Name: services applications_services; Type: FK CONSTRAINT; Schema: public; Owner: d2broker
--

ALTER TABLE ONLY services
    ADD CONSTRAINT applications_services FOREIGN KEY (application_name, application_version) REFERENCES applications(name, version) DEFERRABLE INITIALLY DEFERRED;


-- Completed on 2017-11-18 10:36:52

--
-- PostgreSQL database dump complete
--

