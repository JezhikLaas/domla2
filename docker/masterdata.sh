#!/bin/bash

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" <<-EOSQL
	CREATE USER d2data CREATEDB PASSWORD 'd2data';
EOSQL
