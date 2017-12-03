--
-- PostgreSQL database dump
--

-- Dumped from database version 10.0
-- Dumped by pg_dump version 10.0

-- Started on 2017-11-18 10:36:07

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
-- TOC entry 2201 (class 0 OID 0)
-- Dependencies: 1
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET search_path = public, pg_catalog;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 196 (class 1259 OID 16497)
-- Name: authorization_codes; Type: TABLE; Schema: public; Owner: d2admin
--

CREATE TABLE authorization_codes (
    id character varying(255) NOT NULL,
    data jsonb NOT NULL
);


ALTER TABLE authorization_codes OWNER TO d2admin;

--
-- TOC entry 197 (class 1259 OID 16503)
-- Name: clients; Type: TABLE; Schema: public; Owner: d2admin
--

CREATE TABLE clients (
    id character varying(255) NOT NULL,
    data jsonb NOT NULL
);


ALTER TABLE clients OWNER TO d2admin;

--
-- TOC entry 198 (class 1259 OID 16509)
-- Name: consents; Type: TABLE; Schema: public; Owner: d2admin
--

CREATE TABLE consents (
    subjectid character varying(255) NOT NULL,
    clientid character varying(255) NOT NULL,
    data jsonb NOT NULL
);


ALTER TABLE consents OWNER TO d2admin;

--
-- TOC entry 199 (class 1259 OID 16515)
-- Name: refresh_tokens; Type: TABLE; Schema: public; Owner: d2admin
--

CREATE TABLE refresh_tokens (
    id character varying(255) NOT NULL,
    data jsonb NOT NULL
);


ALTER TABLE refresh_tokens OWNER TO d2admin;

--
-- TOC entry 203 (class 1259 OID 16715)
-- Name: registrations; Type: TABLE; Schema: public; Owner: d2admin
--

CREATE TABLE registrations (
    login character varying(255) NOT NULL,
    first_name character varying(255),
    last_name character varying(255) NOT NULL,
    email character varying(255) NOT NULL,
    salutation character varying(50),
    title character varying(50),
    id uuid NOT NULL
);


ALTER TABLE registrations OWNER TO d2admin;

--
-- TOC entry 200 (class 1259 OID 16521)
-- Name: scopes; Type: TABLE; Schema: public; Owner: d2admin
--

CREATE TABLE scopes (
    name character varying(255) NOT NULL,
    data jsonb NOT NULL
);


ALTER TABLE scopes OWNER TO d2admin;

--
-- TOC entry 201 (class 1259 OID 16527)
-- Name: tokens; Type: TABLE; Schema: public; Owner: d2admin
--

CREATE TABLE tokens (
    id character varying(255) NOT NULL,
    data jsonb NOT NULL
);


ALTER TABLE tokens OWNER TO d2admin;

--
-- TOC entry 202 (class 1259 OID 16533)
-- Name: users; Type: TABLE; Schema: public; Owner: d2admin
--

CREATE TABLE users (
    id uuid NOT NULL,
    login character varying(255) NOT NULL,
    first_name character varying(255),
    last_name character varying(255) NOT NULL,
    title character varying,
    salutation character varying,
    email character varying(255) NOT NULL,
    logged_in time without time zone,
    claims jsonb,
    password character varying(255) NOT NULL
);


ALTER TABLE users OWNER TO d2admin;

--
-- TOC entry 2059 (class 2606 OID 16540)
-- Name: authorization_codes authorization_codes_pkey; Type: CONSTRAINT; Schema: public; Owner: d2admin
--

ALTER TABLE ONLY authorization_codes
    ADD CONSTRAINT authorization_codes_pkey PRIMARY KEY (id);


--
-- TOC entry 2061 (class 2606 OID 16542)
-- Name: clients clients_pkey; Type: CONSTRAINT; Schema: public; Owner: d2admin
--

ALTER TABLE ONLY clients
    ADD CONSTRAINT clients_pkey PRIMARY KEY (id);


--
-- TOC entry 2063 (class 2606 OID 16544)
-- Name: consents consents_pkey; Type: CONSTRAINT; Schema: public; Owner: d2admin
--

ALTER TABLE ONLY consents
    ADD CONSTRAINT consents_pkey PRIMARY KEY (subjectid, clientid);


--
-- TOC entry 2065 (class 2606 OID 16546)
-- Name: refresh_tokens refresh_tokens_pkey; Type: CONSTRAINT; Schema: public; Owner: d2admin
--

ALTER TABLE ONLY refresh_tokens
    ADD CONSTRAINT refresh_tokens_pkey PRIMARY KEY (id);


--
-- TOC entry 2073 (class 2606 OID 16722)
-- Name: registrations registrations_pkey; Type: CONSTRAINT; Schema: public; Owner: d2admin
--

ALTER TABLE ONLY registrations
    ADD CONSTRAINT registrations_pkey PRIMARY KEY (login);


--
-- TOC entry 2067 (class 2606 OID 16548)
-- Name: scopes scopes_pkey; Type: CONSTRAINT; Schema: public; Owner: d2admin
--

ALTER TABLE ONLY scopes
    ADD CONSTRAINT scopes_pkey PRIMARY KEY (name);


--
-- TOC entry 2069 (class 2606 OID 16550)
-- Name: tokens tokens_pkey; Type: CONSTRAINT; Schema: public; Owner: d2admin
--

ALTER TABLE ONLY tokens
    ADD CONSTRAINT tokens_pkey PRIMARY KEY (id);


--
-- TOC entry 2071 (class 2606 OID 16552)
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: d2admin
--

ALTER TABLE ONLY users
    ADD CONSTRAINT users_pkey PRIMARY KEY (id);


-- Completed on 2017-11-18 10:36:10

--
-- PostgreSQL database dump complete
--

