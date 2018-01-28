--
-- PostgreSQL database dump
--

-- Dumped from database version 10.1
-- Dumped by pg_dump version 10.1

-- Started on 2018-01-28 16:39:33

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
-- TOC entry 2190 (class 0 OID 0)
-- Dependencies: 1
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET search_path = public, pg_catalog;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 199 (class 1259 OID 19849)
-- Name: api_resources; Type: TABLE; Schema: public; Owner: d2admin
--

CREATE TABLE api_resources (
    name character varying(255) NOT NULL,
    data jsonb NOT NULL
);


ALTER TABLE api_resources OWNER TO d2admin;

--
-- TOC entry 201 (class 1259 OID 20065)
-- Name: clients; Type: TABLE; Schema: public; Owner: d2admin
--

CREATE TABLE clients (
    id character varying(255) NOT NULL,
    data jsonb NOT NULL
);


ALTER TABLE clients OWNER TO d2admin;

--
-- TOC entry 200 (class 1259 OID 19870)
-- Name: identity_resources; Type: TABLE; Schema: public; Owner: d2admin
--

CREATE TABLE identity_resources (
    name character varying(255) NOT NULL,
    data jsonb NOT NULL
);


ALTER TABLE identity_resources OWNER TO d2admin;

--
-- TOC entry 198 (class 1259 OID 19623)
-- Name: persisted_grants; Type: TABLE; Schema: public; Owner: d2admin
--

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

--
-- TOC entry 196 (class 1259 OID 16412)
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
-- TOC entry 197 (class 1259 OID 16430)
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
    claims jsonb,
    password character varying(255) NOT NULL,
    logged_in timestamp without time zone
);


ALTER TABLE users OWNER TO d2admin;

--
-- TOC entry 2057 (class 2606 OID 19856)
-- Name: api_resources api_resources_pkey; Type: CONSTRAINT; Schema: public; Owner: d2admin
--

ALTER TABLE ONLY api_resources
    ADD CONSTRAINT api_resources_pkey PRIMARY KEY (name);


--
-- TOC entry 2062 (class 2606 OID 20072)
-- Name: clients clients_pkey; Type: CONSTRAINT; Schema: public; Owner: d2admin
--

ALTER TABLE ONLY clients
    ADD CONSTRAINT clients_pkey PRIMARY KEY (id);


--
-- TOC entry 2060 (class 2606 OID 19877)
-- Name: identity_resources identity_resources_pkey; Type: CONSTRAINT; Schema: public; Owner: d2admin
--

ALTER TABLE ONLY identity_resources
    ADD CONSTRAINT identity_resources_pkey PRIMARY KEY (name);


--
-- TOC entry 2055 (class 2606 OID 19632)
-- Name: persisted_grants persisted_grants_pkey; Type: CONSTRAINT; Schema: public; Owner: d2admin
--

ALTER TABLE ONLY persisted_grants
    ADD CONSTRAINT persisted_grants_pkey PRIMARY KEY (key);


--
-- TOC entry 2049 (class 2606 OID 16445)
-- Name: registrations registrations_pkey; Type: CONSTRAINT; Schema: public; Owner: d2admin
--

ALTER TABLE ONLY registrations
    ADD CONSTRAINT registrations_pkey PRIMARY KEY (login);


--
-- TOC entry 2051 (class 2606 OID 16451)
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: d2admin
--

ALTER TABLE ONLY users
    ADD CONSTRAINT users_pkey PRIMARY KEY (id);


--
-- TOC entry 2052 (class 1259 OID 19662)
-- Name: idx_expiration; Type: INDEX; Schema: public; Owner: d2admin
--

CREATE INDEX idx_expiration ON persisted_grants USING btree (expiration);


--
-- TOC entry 2053 (class 1259 OID 40850)
-- Name: idx_subject_client_type; Type: INDEX; Schema: public; Owner: d2admin
--

CREATE INDEX idx_subject_client_type ON persisted_grants USING btree (subject_id, client_id, type);


--
-- TOC entry 2058 (class 1259 OID 19878)
-- Name: idxscopes; Type: INDEX; Schema: public; Owner: d2admin
--

CREATE INDEX idxscopes ON api_resources USING gin ((((data -> 'Scopes'::text) -> 'Name'::text)));


-- Completed on 2018-01-28 16:39:33

--
-- PostgreSQL database dump complete
--

