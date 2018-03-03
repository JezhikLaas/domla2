FROM postgres:latest
RUN localedef -i de_DE -c -f UTF-8 -A /usr/share/locale/locale.alias de_DE.UTF-8
ENV LANG de_DE.utf8
COPY authentication.sh /docker-entrypoint-initdb.d/
COPY servicebroker.sh /docker-entrypoint-initdb.d/